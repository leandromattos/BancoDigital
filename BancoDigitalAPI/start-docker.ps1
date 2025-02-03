param (
    [string]$SolutionPath = $(Get-Location)  # Define o caminho do projeto como padrão
)

Write-Host "=> Diretório da solução: $SolutionPath" -ForegroundColor yellow

# Verifica se o arquivo .sln existe no diretório
if (-not (Test-Path $SolutionPath)) {
    Write-Host "❌ O arquivo BancoDigital.sln não foi encontrado no diretório do projeto!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Arquivo da solução encontrados! Continuando..." -ForegroundColor Green

$projectPath =  Join-Path $SolutionPath "\BancoDigitalAPI\"
Write-Host "=> Diretório do projeto: $projectPath" -ForegroundColor yellow

# Verifica se o arquivo .csproj diretório do projeto
$csprojPath = Join-Path $projectPath "BancoDigitalAPI.csproj"
Write-Host "=> Caminho do arquivo .csproj: $csprojPath" -ForegroundColor yellow

if (-not (Test-Path $csprojPath)) {
    Write-Host "❌ O arquivo BancoDigitalAPI.csproj não foi encontrado no diretório do projeto!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Arquivo do projeto encontrado! Continuando..." -ForegroundColor Green


# Verificar se o Docker Desktop está instalado
$dockerPath = $dockerPath = (Get-Command "docker" -ErrorAction SilentlyContinue).Source
Write-Host "=> Caminho do Docker: $dockerPath" -ForegroundColor yellow

if (-not $dockerPath) {
    Write-Host "❌ Docker não está instalado. Instale o Docker Desktop e tente novamente." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Docker instalado! Continuando..." -ForegroundColor Green

# Verificar se o Docker está rodando
$dockerRunning = & docker info | Select-String "Server:" 

if ($dockerRunning) {
    Write-Host "✅ Docker está ativo! Continuando..." -ForegroundColor Green
} else {
    Write-Host "❌ Docker não está ativo. Certifique-se de que o Docker Desktop está aberto." -ForegroundColor Red
}

# Navega até o diretório do projeto
Set-Location $projectPath

# Executar os comandos do Docker
Write-Host "🚀 Construindo e iniciando os containers..." -ForegroundColor Green

docker-compose build --no-cache
Write-Host "✅ Build: OK! Continuando..." -ForegroundColor Green
docker-compose up -d --build
Write-Host "✅ Aplicação: OK!" -ForegroundColor Green
Write-Host "🌐 Acessando o swagger: http://localhost:8080/swagger/" -ForegroundColor blue
Start-Process "http://localhost:8080/swagger/"

