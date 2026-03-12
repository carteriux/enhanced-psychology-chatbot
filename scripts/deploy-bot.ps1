# ============================================================
# deploy-bot.ps1 — Despliega el bot Python (chatbot-cpc)
# Proyecto: chatbots-452017 | Region: us-south1
# Uso: .\scripts\deploy-bot.ps1
# ============================================================

$PROJECT_ID  = "chatbots-452017"
$REGION      = "us-south1"
$SERVICE     = "chatbot-cpc"
$IMAGE_NAME  = "proyecto-escuela-psicoanalisis"
$TAG         = "v$(Get-Date -Format 'yyyyMMdd-HHmm')"
$IMAGE_FULL  = "gcr.io/$PROJECT_ID/${IMAGE_NAME}:$TAG"
$SOURCE_DIR  = "$PSScriptRoot\..\bot-main\bot-main"

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

# 2. Build con Cloud Build (no requiere Docker local)
Write-Host "`n[2/3] Construyendo imagen con Cloud Build..." -ForegroundColor Yellow
gcloud builds submit $SOURCE_DIR --tag $IMAGE_FULL --project $PROJECT_ID
if ($LASTEXITCODE -ne 0) { Write-Host "ERROR en Cloud Build" -ForegroundColor Red; exit 1 }
Write-Host "Imagen subida: $IMAGE_FULL" -ForegroundColor Green

# 3. Deploy en Cloud Run
Write-Host "`n[3/3] Desplegando en Cloud Run..." -ForegroundColor Yellow
gcloud run deploy $SERVICE `
    --image $IMAGE_FULL `
    --project $PROJECT_ID `
    --region $REGION `
    --platform managed `
    --allow-unauthenticated `
    --memory 1Gi `
    --cpu 1 `
    --timeout 300 `
    --quiet

if ($LASTEXITCODE -ne 0) { Write-Host "ERROR en el deploy" -ForegroundColor Red; exit 1 }

Write-Host "`n=====================================" -ForegroundColor Green
Write-Host " Deploy completado exitosamente" -ForegroundColor Green
Write-Host " Servicio: $SERVICE" -ForegroundColor Green
Write-Host " Tag:  $TAG" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
