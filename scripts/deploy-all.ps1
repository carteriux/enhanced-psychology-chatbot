# ============================================================
# deploy-all.ps1 — Despliega los 3 servicios en orden
# Orden: bot (Python) → backend (C#) → frontend (Next.js)
# Uso: .\scripts\deploy-all.ps1 [-SkipBot] [-SkipBackend] [-SkipFrontend]
# ============================================================

param(
    [switch]$SkipBot,
    [switch]$SkipBackend,
    [switch]$SkipFrontend
)

$SCRIPTS_DIR = $PSScriptRoot
$ErrorCount  = 0

function Invoke-DeployScript {
    param(
        [string]$Name,
        [string]$Script
    )
    Write-Host "`n" + ("=" * 60) -ForegroundColor Magenta
    Write-Host "  INICIANDO DEPLOY: $Name" -ForegroundColor Magenta
    Write-Host ("=" * 60) -ForegroundColor Magenta

    & "$SCRIPTS_DIR\$Script"
    if ($LASTEXITCODE -ne 0) {
        Write-Host "`nERROR: Fallo el deploy de $Name (exit code $LASTEXITCODE)" -ForegroundColor Red
        return $false
    }
    return $true
}

$StartTime = Get-Date
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host "  DEPLOY COMPLETO - Centro de Psicoterapia Cognitiva" -ForegroundColor Cyan
Write-Host "  Inicio: $StartTime" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan

# --- 1. Bot Python (chatbot-cpc) ---
if (-not $SkipBot) {
    $ok = Invoke-DeployScript -Name "Bot Python (chatbot-cpc)" -Script "deploy-bot.ps1"
    if (-not $ok) { $ErrorCount++ }
} else {
    Write-Host "`n[SKIP] Bot Python (chatbot-cpc)" -ForegroundColor DarkGray
}

# --- 2. Backend C# (chatbot-back) ---
if (-not $SkipBackend) {
    $ok = Invoke-DeployScript -Name "Backend C# (chatbot-back)" -Script "deploy-backend.ps1"
    if (-not $ok) { $ErrorCount++ }
} else {
    Write-Host "`n[SKIP] Backend C# (chatbot-back)" -ForegroundColor DarkGray
}

# --- 3. Frontend Next.js (chatbot-front) ---
if (-not $SkipFrontend) {
    $ok = Invoke-DeployScript -Name "Frontend Next.js (chatbot-front)" -Script "deploy-frontend.ps1"
    if (-not $ok) { $ErrorCount++ }
} else {
    Write-Host "`n[SKIP] Frontend Next.js (chatbot-front)" -ForegroundColor DarkGray
}

# --- Resumen final ---
$EndTime  = Get-Date
$Duration = $EndTime - $StartTime

Write-Host "`n============================================================" -ForegroundColor Cyan
Write-Host "  RESUMEN DEL DEPLOY" -ForegroundColor Cyan
Write-Host "  Duracion: $([math]::Round($Duration.TotalMinutes, 1)) minutos" -ForegroundColor Cyan
if ($ErrorCount -eq 0) {
    Write-Host "  Resultado: TODOS LOS DEPLOYS EXITOSOS" -ForegroundColor Green
} else {
    Write-Host "  Resultado: $ErrorCount SERVICIO(S) FALLARON - revisa los errores arriba" -ForegroundColor Red
}
Write-Host "============================================================" -ForegroundColor Cyan

if ($ErrorCount -gt 0) { exit 1 }
