# AutoClicker Minecraft

Um autoclicker desenvolvido em C# Windows Forms especificamente para Minecraft, permitindo farmar peixes AFK sem precisar manter a janela em foco.

## 🎯 Funcionalidades

- **Detecção automática de janelas do Minecraft**: Identifica janelas do Minecraft, Java, MultiMC, Lunar, Badlion, Forge, Fabric, OptiFine
- **Seleção de botão**: Escolha entre clique esquerdo ou direito
- **Intervalo configurável**: Defina o tempo entre cliques em milissegundos
- **Funcionamento em background**: Não precisa manter a janela do Minecraft em foco
- **Posição personalizada**: Defina uma posição específica para clicar (F10)
- **Teclas de atalho**: F9 para iniciar/parar, F10 para definir posição
- **ESC automático**: Envia ESC automaticamente para evitar pausa do jogo
- **Configurações persistentes**: Salva suas preferências automaticamente
- **Verificação de janela**: Opção para parar quando a janela do Minecraft for fechada

## 🚀 Como usar

### 📥 Download Rápido (Recomendado)
1. **Baixe o executável**:
   - Vá para a seção [Releases](https://github.com/rntsza/acmc/releases)
   - Baixe o arquivo `AutoClickerMC.zip`
   - Extraia e execute `AutoClickerMC.exe`

### 🔧 Instalação para Desenvolvimento
Se você quiser modificar o código ou compilar você mesmo:

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/rntsza/acmc.git
   cd acmc
   ```

2. **Compile o projeto**:
   ```bash
   dotnet build
   ```

3. **Execute o aplicativo**:
   ```bash
   dotnet run
   ```

### Configuração
1. **Selecione a janela do Minecraft** na lista (clique em "Atualizar" se necessário)
2. **Escolha o botão** (esquerdo ou direito)
3. **Defina o intervalo** em milissegundos (recomendado: 1000ms)
4. **Configure opções adicionais**:
   - ✅ "Somente quando a janela existir" - Para automaticamente se a janela fechar
   - ✅ "ESC automático quando o jogo pausar" - Envia ESC para evitar pausa
5. **Defina posição personalizada** (opcional):
   - Clique em "Definir Posição (F10)" ou pressione F10
   - Posicione o mouse onde deseja clicar
   - Clique para confirmar

### Uso
- **Iniciar**: Clique em "Iniciar" ou pressione **F9**
- **Parar**: Clique em "Parar" ou pressione **F9** novamente
- **Definir posição**: Pressione **F10** a qualquer momento

### 💡 Dica Importante
**Pressione F3+P no Minecraft** para desabilitar o pause automático do jogo!

## 📦 Releases

### Versão Atual: v1.0.0
- **Download**: [AutoClickerMC.zip](https://github.com/rntsza/acmc/releases/latest)
- **Tamanho**: ~2MB
- **Requisitos**: Windows 10/11, .NET 8.0 Runtime

### Histórico de Versões
- **v1.0.0** - Versão inicial com todas as funcionalidades
  - Detecção automática de janelas do Minecraft
  - Teclas de atalho F9/F10
  - Configurações persistentes
  - ESC automático
  - Posição personalizada de clique

## 📋 Requisitos

- **.NET 8.0** ou superior
- **Windows 10/11** (usa APIs do Windows)
- **Minecraft** em execução
- **Visual Studio 2022** ou **VS Code** (para desenvolvimento)

## 🔧 Características técnicas

- **API Windows**: Utiliza `user32.dll` para controle de janelas
- **Threading**: Usa `System.Windows.Forms.Timer` para cliques periódicos
- **PostMessage**: Envia mensagens diretamente para a janela do Minecraft
- **Hotkeys globais**: F9 e F10 funcionam em qualquer lugar
- **Configurações persistentes**: Salva em `autoclicker_config.txt`
- **Detecção inteligente**: Identifica janelas do Minecraft por título

## 🛡️ Segurança

- ✅ Funciona apenas com janelas específicas do Minecraft
- ✅ Não interfere com outras aplicações
- ✅ Pode ser interrompido a qualquer momento
- ✅ Não modifica arquivos do jogo
- ✅ Código aberto e auditável

## ⚠️ Limitações

- Funciona apenas no Windows
- Requer que o Minecraft esteja em execução
- O intervalo mínimo recomendado é 100ms para evitar sobrecarga
- Não funciona com Minecraft Bedrock Edition

## 📁 Estrutura do Projeto

```
autoclicker-minecraft/
├── MainForm.cs              # Lógica principal da aplicação
├── MainForm.Designer.cs     # Código gerado pelo designer
├── Program.cs               # Ponto de entrada
├── AutoClickerMC.csproj    # Arquivo de projeto
├── create-release.ps1       # Script para criar releases
├── LICENSE                  # Licença MIT
├── .gitignore              # Arquivos ignorados pelo Git
├── autoclicker_config.txt   # Configurações salvas
```

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 🚀 Para Desenvolvedores - Criando Releases

### Script Automatizado
```powershell
# Execute o script para criar release automaticamente
.\create-release.ps1 -Version "1.0.0"
```

### Manual
1. **Compile o projeto**:
   ```bash
   dotnet build -c Release
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
   ```
   
## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## ⚠️ Aviso Legal

Este software é fornecido "como está", sem garantias. Use por sua conta e risco. O autor não se responsabiliza por qualquer uso indevido ou consequências do uso deste software.
