# Guia de Instalação - AutoClicker Minecraft

## Pré-requisitos

### 1. .NET 8.0 SDK
- Baixe e instale o .NET 8.0 SDK do site oficial da Microsoft
- Verifique a instalação executando: `dotnet --version`

### 2. Visual Studio ou VS Code (opcional)
- Para desenvolvimento: Visual Studio 2022 ou VS Code com extensão C#
- Para apenas executar: não é necessário

## Instalação

### Método 1: Compilação local

1. **Clone ou baixe o projeto**
2. **Abra o terminal na pasta do projeto**
3. **Restaure as dependências**:
   ```bash
   dotnet restore
   ```
4. **Compile o projeto**:
   ```bash
   dotnet build --configuration Release
   ```
5. **Execute o aplicativo**:
   ```bash
   dotnet run
   ```

### Método 2: Executável standalone

1. **Publique o projeto**:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true
   ```
2. **Execute o arquivo gerado** em `bin/Release/net8.0-windows/win-x64/publish/`

## Primeira execução

1. **Inicie o Minecraft**
2. **Execute o AutoClicker**
3. **Clique em "Atualizar"** para detectar a janela do Minecraft
4. **Configure os parâmetros**:
   - Botão: Esquerdo (para pescar) ou Direito
   - Intervalo: 1000ms (1 segundo) é recomendado
5. **Clique em "Iniciar"**

## Solução de problemas

### Janela do Minecraft não aparece
- Certifique-se de que o Minecraft está rodando
- Clique em "Atualizar" na interface
- Verifique se o título da janela contém "Minecraft"

### Erro de compilação
- Verifique se o .NET 8.0 está instalado
- Execute `dotnet --version` para confirmar
- Tente limpar o cache: `dotnet clean && dotnet restore`

### Aplicativo não inicia
- Verifique se está no Windows
- Execute como administrador se necessário
- Verifique se todas as dependências estão instaladas

## Configurações recomendadas

### Para pescar AFK
- **Botão**: Esquerdo
- **Intervalo**: 1000-2000ms
- **Verificação de janela**: Ativada

### Para outras atividades
- **Botão**: Direito (para colocar blocos)
- **Intervalo**: 500-1000ms
- **Verificação de janela**: Ativada
