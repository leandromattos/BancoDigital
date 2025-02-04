param (
    [string]$SolutionPath = $(Get-Location)  # Define o caminho do projeto como padrão
)

# Verifica se o dotnet está disponível no PATH
$dotnetPath = (Get-Command dotnet -ErrorAction SilentlyContinue).Source

if ($dotnetPath) {
    Write-Host "✔ O .NET SDK está configurado no PATH: $dotnetPath" -ForegroundColor Green
} else {
    Write-Host "❌ PATH dotnet(variaveis de ambiente) não existe, adicionando..." -ForegroundColor yellow
    exit 1
}

# Verificar se o .NET SDK está instalado e sua versão
$dotnetVersion = dotnet --version 2>$null

if (-not $dotnetVersion) {
    Write-Host "❌ .NET SDK não encontrado. Instale o .NET SDK 8.0 ou superior:" -ForegroundColor Red
    Write-Host "🔗 https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    exit 1
}

# Comparar versão instalada com 8.0
$minVersion = [System.Version]"8.0.0"
$installedVersion = [System.Version]$dotnetVersion

if ($installedVersion -lt $minVersion) {
    Write-Host "❌ Versão do .NET SDK insuficiente ($dotnetVersion). Instale o .NET SDK 8.0 ou superior:" -ForegroundColor Red
    Write-Host "🔗 https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    exit 1
}

Write-Host "✅ .NET SDK versão $dotnetVersion está instalado." -ForegroundColor Green

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
    exit  1
}

# Verifica se o Docker Desktop está em execução
$dockerProcess = Get-Process -Name "Docker Desktop" -ErrorAction SilentlyContinue

if ($dockerProcess) {
    Write-Host "✅ Docker Desktop já está rodando!"
} else {
    Write-Host "❌ Erro: Docker Desktop não está rodando! Inicie o Docker Desktop, e tente novamente." -ForegroundColor Red
    exit 1
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

