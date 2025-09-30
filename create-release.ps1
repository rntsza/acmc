# Script para criar release do AutoClicker Minecraft
# Execute: .\create-release.ps1

param(
    [string]$Version = "1.0.0",
    [string]$Configuration = "Release"
)

Write-Host "🚀 Criando release v$Version..." -ForegroundColor Green

# Limpar builds anteriores
Write-Host "🧹 Limpando builds anteriores..." -ForegroundColor Yellow
if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" }
if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" }
if (Test-Path "release") { Remove-Item -Recurse -Force "release" }

# Compilar projeto
Write-Host "🔨 Compilando projeto..." -ForegroundColor Yellow
dotnet build -c $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro na compilação!" -ForegroundColor Red
    exit 1
}

# Publicar como executável único
Write-Host "📦 Criando executável..." -ForegroundColor Yellow
dotnet publish -c $Configuration -r win-x64 --self-contained true -p:PublishSingleFile=true -o "release"

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro na publicação!" -ForegroundColor Red
    exit 1
}

# Copiar ícone (necessário para o aplicativo funcionar)
Write-Host "🎨 Copiando ícone..." -ForegroundColor Yellow
if (Test-Path "MineCraft.ico") {
    copy MineCraft.ico release\
} else {
    Write-Host "⚠️ Arquivo MineCraft.ico não encontrado!" -ForegroundColor Yellow
}

# Criar arquivo de configuração padrão
Write-Host "📝 Criando arquivos de configuração..." -ForegroundColor Yellow
@"
Window=
Button=Esquerdo
Interval=1000
OnlyWhenOpen=True
AutoEscape=True
UseCustomPosition=False
ClickPositionX=0
ClickPositionY=0
"@ | Out-File -FilePath "release\autoclicker_config.txt" -Encoding UTF8

# Criar README para o release
Write-Host "📖 Criando README do release..." -ForegroundColor Yellow
@"
# AutoClicker Minecraft v$Version

## 🎯 Como usar
1. Execute `AutoClickerMC.exe`
2. Selecione a janela do Minecraft na lista
3. Configure suas opções (botão, intervalo, etc.)
4. Pressione F9 para iniciar/parar

## ⌨️ Teclas de atalho
- **F9**: Iniciar/Parar o autoclicker
- **F10**: Definir posição personalizada do clique

## 💡 Dica importante
**Pressione F3+P no Minecraft** para desabilitar o pause automático do jogo!

## ⚠️ Requisitos
- Windows 10/11
- .NET 8.0 Runtime (se não funcionar, baixe em: https://dotnet.microsoft.com/download)

## 🐛 Problemas?
- Certifique-se de que o Minecraft está rodando
- Execute como administrador se necessário
- Verifique se o .NET 8.0 Runtime está instalado
"@ | Out-File -FilePath "release\README.txt" -Encoding UTF8

# Criar ZIP
Write-Host "📦 Criando arquivo ZIP..." -ForegroundColor Yellow
$zipName = "AutoClickerMC-v$Version.zip"
if (Test-Path $zipName) { Remove-Item $zipName }

Compress-Archive -Path "release\*" -DestinationPath $zipName -Force

# Verificar tamanho
$zipSize = (Get-Item $zipName).Length / 1MB
Write-Host "✅ Release criado: $zipName ($([math]::Round($zipSize, 2)) MB)" -ForegroundColor Green

# Mostrar próximos passos
Write-Host "`n🎯 Próximos passos:" -ForegroundColor Cyan
Write-Host "1. Vá para: https://github.com/seu-usuario/autoclicker-minecraft/releases" -ForegroundColor White
Write-Host "2. Clique em 'Create a new release'" -ForegroundColor White
Write-Host "3. Tag version: v$Version" -ForegroundColor White
Write-Host "4. Release title: AutoClicker Minecraft v$Version" -ForegroundColor White
Write-Host "5. Anexe o arquivo: $zipName" -ForegroundColor White
Write-Host "6. Publique o release!" -ForegroundColor White

Write-Host "`n✨ Release pronto para publicação!" -ForegroundColor Green
