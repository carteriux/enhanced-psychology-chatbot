# ============================================================
# deploy-backend.ps1 — Despliega el backend C# (chatbot-back)
# Proyecto: chatbots-452017 | Region: us-south1
# Uso: .\scripts\deploy-backend.ps1
# ============================================================

$PROJECT_ID  = "chatbots-452017"
$REGION      = "us-south1"
$SERVICE     = "chatbot-back"
$IMAGE_NAME  = "cpc-api"
$TAG         = "v$(Get-Date -Format 'yyyyMMdd-HHmm')"
$IMAGE_FULL  = "gcr.io/$PROJECT_ID/${IMAGE_NAME}:$TAG"
# El Dockerfile del backend se ejecuta desde la carpeta del solution
$SOURCE_DIR  = "$PSScriptRoot\..\bot_backend-main\bot_backend-main"

Write-Host "======================================" -ForegroundColor Cyan
Write-Host " Desplegando: $SERVICE" -ForegroundColor Cyan
Write-Host " Imagen:      $IMAGE_FULL" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan

# 1. Verificar autenticacion con GCP
Write-Host "`n[1/4] Verificando autenticacion con GCP..." -ForegroundColor Yellow
$account = gcloud config get-value account 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: No estas autenticado. Ejecuta: gcloud auth login" -ForegroundColor Red
    exit 1
}
Write-Host "Autenticado como: $account" -ForegroundColor Green

# 2. Configurar Docker para usar GCR
Write-Host "`n[2/4] Configurando Docker con GCR..." -ForegroundColor Yellow
gcloud auth configure-docker --quiet
if ($LASTEXITCODE -ne 0) { Write-Host "ERROR configurando Docker" -ForegroundColor Red; exit 1 }

# 3. Build de la imagen Docker
# NOTA: El Dockerfile esta en CPC-API/ pero necesita el contexto del solution completo
Write-Host "`n[3/4] Construyendo imagen Docker..." -ForegroundColor Yellow
docker build -t $IMAGE_FULL -f "$SOURCE_DIR\CPC-API\Dockerfile" $SOURCE_DIR
if ($LASTEXITCODE -ne 0) { Write-Host "ERROR en docker build" -ForegroundColor Red; exit 1 }

# Push al Container Registry
docker push $IMAGE_FULL
if ($LASTEXITCODE -ne 0) { Write-Host "ERROR en docker push" -ForegroundColor Red; exit 1 }
Write-Host "Imagen subida: $IMAGE_FULL" -ForegroundColor Green

# 4. Deploy en Cloud Run
Write-Host "`n[4/4] Desplegando en Cloud Run..." -ForegroundColor Yellow
gcloud run deploy $SERVICE `
    --image $IMAGE_FULL `
    --project $PROJECT_ID `
    --region $REGION `
    --platform managed `
    --allow-unauthenticated `
    --memory 512Mi `
    --cpu 1 `
    --min-instances 1 `
    --max-instances 2 `
    --timeout 60 `
    --quiet

if ($LASTEXITCODE -ne 0) { Write-Host "ERROR en el deploy" -ForegroundColor Red; exit 1 }

Write-Host "`n=====================================" -ForegroundColor Green
Write-Host " Deploy completado exitosamente" -ForegroundColor Green
Write-Host " Servicio: $SERVICE" -ForegroundColor Green
Write-Host " Tag:      $TAG" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
