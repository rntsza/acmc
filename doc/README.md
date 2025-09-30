# AutoClicker Minecraft

Um autoclicker desenvolvido em C# WPF especificamente para Minecraft, permitindo farmar peixes AFK sem precisar manter a janela em foco.

## Funcionalidades

- **Detecção automática de janelas do Minecraft**: Identifica janelas do Minecraft, GLFW e LWJGL
- **Seleção de botão**: Escolha entre clique esquerdo ou direito
- **Intervalo configurável**: Defina o tempo entre cliques em milissegundos
- **Funcionamento em background**: Não precisa manter a janela do Minecraft em foco
- **Verificação de janela**: Opção para parar quando a janela do Minecraft for fechada
- **Interface simples**: UI intuitiva e fácil de usar

## Como usar

1. **Compile o projeto**:
   ```bash
   dotnet build
   ```

2. **Execute o aplicativo**:
   ```bash
   dotnet run
   ```

3. **Configure o autoclicker**:
   - Selecione a janela do Minecraft na lista
   - Escolha o botão (esquerdo ou direito)
   - Defina o intervalo em milissegundos
   - Marque "Somente quando a janela existir" se desejar

4. **Inicie o farm**:
   - Clique em "Iniciar"
   - O autoclicker começará a funcionar automaticamente
   - Use "Parar" para interromper

## Requisitos

- .NET 8.0 ou superior
- Windows (usa APIs do Windows)
- Minecraft em execução

## Características técnicas

- **API Windows**: Utiliza `user32.dll` para controle de janelas
- **Threading**: Usa `Timer` para cliques periódicos
- **PostMessage**: Envia mensagens diretamente para a janela do Minecraft
- **Detecção inteligente**: Identifica janelas do Minecraft por título

## Segurança

- Funciona apenas com janelas específicas do Minecraft
- Não interfere com outras aplicações
- Pode ser interrompido a qualquer momento
- Não modifica arquivos do jogo

## Limitações

- Funciona apenas no Windows
- Requer que o Minecraft esteja em execução
- O intervalo mínimo recomendado é 100ms para evitar sobrecarga
