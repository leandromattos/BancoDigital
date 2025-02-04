# Obtém as variáveis de ambiente PATH do Sistema e do Usuário
$systemPath = [System.Environment]::GetEnvironmentVariable("Path", [System.EnvironmentVariableTarget]::Machine)
$userPath = [System.Environment]::GetEnvironmentVariable("Path", [System.EnvironmentVariableTarget]::User)

# Lista de caminhos esperados
$expectedPaths = @(
    "$env:USERPROFILE\.dotnet\tools",
    "C:\Program Files\dotnet\",
    "C:\Program Files\Docker\Docker\resources\bin"
)

# Função para verificar se um caminho existe na variável PATH
function Check-PathExists($path, $envPath) {
    return ($envPath -split ";" -contains $path)
}

# Verifica se caminho existe no PATH do Sistema
function Add-ToSystemPath($path) {
    if (-Not (Check-PathExists $path $systemPath)) {
        Write-Host "❌ O Caminho '$path' não existe no PATH do Sistema." -ForegroundColor Red
    } else {
        Write-Host "✅ O caminho '$path' já está presente no PATH do Sistema." -ForegroundColor Green
    }
}

# Loop pelos caminhos esperados e adiciona ao PATH se necessário
foreach ($path in $expectedPaths) {   
    Add-ToSystemPath $path    
}

Write-Host "`n🚀 **Checagem concluída! **"
