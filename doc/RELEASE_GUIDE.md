# Guia para Publicar Releases no GitHub

## 📦 Como Criar um Release

### 1. Preparar o Executável
```bash
# Compilar em modo Release
dotnet build -c Release

# Publicar como executável único
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### 2. Criar o Pacote de Release
```bash
# Criar pasta de release
mkdir release

# Copiar executável
copy bin\Release\net8.0-windows\win-x64\publish\AutoClickerMC.exe release\

# Copiar arquivos necessários
copy autoclicker_config.txt release\ 2>nul || echo. > release\autoclicker_config.txt

# Criar arquivo README para o release
echo # AutoClicker Minecraft v1.0.0 > release\README.txt
echo. >> release\README.txt
echo ## Como usar >> release\README.txt
echo 1. Execute AutoClickerMC.exe >> release\README.txt
echo 2. Selecione a janela do Minecraft >> release\README.txt
echo 3. Configure suas opções >> release\README.txt
echo 4. Pressione F9 para iniciar/parar >> release\README.txt
echo. >> release\README.txt
echo ## Teclas de atalho >> release\README.txt
echo - F9: Iniciar/Parar >> release\README.txt
echo - F10: Definir posição do clique >> release\README.txt
echo. >> release\README.txt
echo ## Dica importante >> release\README.txt
echo Pressione F3+P no Minecraft para desabilitar o pause automático! >> release\README.txt

# Criar ZIP
powershell Compress-Archive -Path release\* -DestinationPath AutoClickerMC-v1.0.0.zip
```

### 3. Publicar no GitHub
1. **Vá para o repositório no GitHub**
2. **Clique em "Releases"** (lado direito da página)
3. **Clique em "Create a new release"**
4. **Preencha os campos**:
   - **Tag version**: `v1.0.0`
   - **Release title**: `AutoClicker Minecraft v1.0.0`
   - **Description**:
     ```markdown
     ## 🎯 Funcionalidades
     - Detecção automática de janelas do Minecraft
     - Teclas de atalho F9/F10
     - Configurações persistentes
     - ESC automático para evitar pausa
     - Posição personalizada de clique
     
     ## 📥 Download
     Baixe o arquivo `AutoClickerMC-v1.0.0.zip` e extraia.
     Execute `AutoClickerMC.exe` para usar.
     
     ## ⚠️ Requisitos
     - Windows 10/11
     - .NET 8.0 Runtime (baixe em: https://dotnet.microsoft.com/download)
     ```

5. **Arraste o arquivo ZIP** para a seção "Attach binaries"
6. **Clique em "Publish release"**

## 🔄 Atualizando Releases

### Para Nova Versão
1. **Atualize a versão** no `AutoClickerMC.csproj`:
   ```xml
   <PropertyGroup>
     <Version>1.1.0</Version>
   </PropertyGroup>
   ```

2. **Siga os passos acima** com a nova versão

### Para Corrigir Release Existente
1. **Vá para o release** no GitHub
2. **Clique em "Edit"**
3. **Adicione novos arquivos** ou **substitua existentes**
4. **Atualize a descrição** se necessário
5. **Clique em "Update release"**

## 📋 Checklist do Release

- [ ] Código compilado sem erros
- [ ] Testado em Windows 10/11
- [ ] Executável funciona sem Visual Studio
- [ ] Arquivo ZIP criado
- [ ] README.txt incluído
- [ ] Tag version criada
- [ ] Release description preenchida
- [ ] Arquivo ZIP anexado
- [ ] Release publicado

## 🎯 Dicas Importantes

### Para o Executável
- **Use `--self-contained true`** para incluir o .NET Runtime
- **Use `PublishSingleFile=true`** para criar um único arquivo
- **Teste em máquina limpa** sem Visual Studio instalado

### Para o Release
- **Use tags semânticas**: v1.0.0, v1.1.0, v2.0.0
- **Inclua changelog** na descrição
- **Mencione requisitos** claramente
- **Adicione screenshots** se necessário

### Para o Usuário
- **Instruções claras** de instalação
- **Requisitos mínimos** especificados
- **Troubleshooting** básico incluído
