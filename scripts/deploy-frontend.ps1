# ============================================================
# deploy-frontend.ps1 — Despliega el frontend Next.js (chatbot-front)
# Proyecto: chatbots-452017 | Region: us-south1
# Uso: .\scripts\deploy-frontend.ps1
# ============================================================

$PROJECT_ID   = "chatbots-452017"
$REGION       = "us-south1"
$SERVICE      = "chatbot-front"
$IMAGE_NAME   = "cpc-frontend"
$TAG          = "v$(Get-Date -Format 'yyyyMMdd-HHmm')"
$IMAGE_FULL   = "gcr.io/$PROJECT_ID/${IMAGE_NAME}:$TAG"
$SOURCE_DIR   = "$PSScriptRoot\..\bot_web-main\bot_web-main"
# URL del backend C# en produccion
$BACKEND_URL  = "https://chatbot-back-hv7c7xkcnq-vp.a.run.app"

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
    --memory 512Mi `
    --cpu 1 `
    --timeout 60 `
    --set-env-vars "NEXT_PUBLIC_BACKEND_URL=$BACKEND_URL" `
    --quiet

if ($LASTEXITCODE -ne 0) { Write-Host "ERROR en el deploy" -ForegroundColor Red; exit 1 }

Write-Host "`n=====================================" -ForegroundColor Green
Write-Host " Deploy completado exitosamente" -ForegroundColor Green
Write-Host " Servicio: $SERVICE" -ForegroundColor Green
Write-Host " Tag:      $TAG" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
