using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoClickerMC
{
    public partial class MainForm : Form
    {
        private ComboBox windowCombo = null!;
        private ComboBox buttonCombo = null!;
        private TextBox intervalBox = null!;
        private CheckBox onlyWhenOpenCheck = null!;
        private Button refreshBtn = null!;
        private Button startBtn = null!;
        private Button stopBtn = null!;
        private System.Windows.Forms.Timer? clickTimer;
        private System.Windows.Forms.Timer? escapeTimer;
        private Dictionary<string, IntPtr> windowHandles = new Dictionary<string, IntPtr>();
        private bool isClicking = false;
        private Point clickPosition = new Point(0, 0);
        private bool useCustomPosition = false;
        private bool autoEscape = true;
        private IntPtr targetWindow = IntPtr.Zero;
        private string configFile = "autoclicker_config.txt";

        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;
        const uint WM_RBUTTONDOWN = 0x0204;
        const uint WM_RBUTTONUP = 0x0205;
        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const uint VK_ESCAPE = 0x1B; // Tecla ESC

        [StructLayout(LayoutKind.Sequential)]
        struct RECT { public int Left; public int Top; public int Right; public int Bottom; }

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")] static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll")] static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")] static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")] static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")] static extern IntPtr GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll")] static extern IntPtr WindowFromPoint(Point point);
        [DllImport("user32.dll")] static extern IntPtr ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool IsWindowEnabled(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public MainForm()
        {
            InitializeComponent();
            CreateInterface();
            RefreshWindows();
            LoadSettings();
            RegisterHotKeys();
            
            // Definir ícone do formulário
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MineCraft.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    // Tentar no diretório atual
                    if (File.Exists("MineCraft.ico"))
                    {
                        this.Icon = new Icon("MineCraft.ico");
                    }
                }
            }
            catch (Exception)
            {
                // Se não conseguir carregar o ícone, continua sem ele
            }
        }

        private void CreateInterface()
        {
            // Janela
            var windowLabel = new Label
            {
                Text = "Janela:",
                Location = new Point(10, 15),
                Size = new Size(50, 20)
            };
            this.Controls.Add(windowLabel);

            windowCombo = new ComboBox
            {
                Location = new Point(70, 12),
                Size = new Size(280, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            windowCombo.SelectedIndexChanged += (s, e) => SaveSettings();
            this.Controls.Add(windowCombo);

            refreshBtn = new Button
            {
                Text = "Atualizar",
                Location = new Point(360, 10),
                Size = new Size(80, 30)
            };
            refreshBtn.Click += RefreshBtn_Click;
            this.Controls.Add(refreshBtn);

            // Botão
            var buttonLabel = new Label
            {
                Text = "Botão:",
                Location = new Point(10, 50),
                Size = new Size(50, 20)
            };
            this.Controls.Add(buttonLabel);

            buttonCombo = new ComboBox
            {
                Location = new Point(70, 47),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            buttonCombo.Items.AddRange(new[] { "Esquerdo", "Direito" });
            buttonCombo.SelectedIndex = 0;
            buttonCombo.SelectedIndexChanged += (s, e) => SaveSettings();
            this.Controls.Add(buttonCombo);

            // Intervalo
            var intervalLabel = new Label
            {
                Text = "Intervalo (ms):",
                Location = new Point(200, 50),
                Size = new Size(80, 20)
            };
            this.Controls.Add(intervalLabel);

            intervalBox = new TextBox
            {
                Text = "1000",
                Location = new Point(290, 47),
                Size = new Size(80, 25)
            };
            intervalBox.TextChanged += (s, e) => SaveSettings();
            this.Controls.Add(intervalBox);

            // Checkbox
            onlyWhenOpenCheck = new CheckBox
            {
                Text = "Somente quando a janela existir",
                Location = new Point(10, 85),
                Size = new Size(250, 20),
                Checked = true
            };
            onlyWhenOpenCheck.CheckedChanged += (s, e) => SaveSettings();
            this.Controls.Add(onlyWhenOpenCheck);

            // Checkbox para ESC automático
            var autoEscapeCheck = new CheckBox
            {
                Text = "ESC automático quando o jogo pausar",
                Location = new Point(10, 105),
                Size = new Size(300, 20),
                Checked = true
            };
            autoEscapeCheck.CheckedChanged += (s, e) => 
            {
                autoEscape = autoEscapeCheck.Checked;
                SaveSettings();
            };
            this.Controls.Add(autoEscapeCheck);

            // Instrução sobre F3+P
            var instructionLabel = new Label
            {
                Text = "💡 DICA: Pressione F3+P no Minecraft para desabilitar o pause automático!",
                Location = new Point(10, 130),
                Size = new Size(400, 20),
                ForeColor = Color.DarkGreen,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.Controls.Add(instructionLabel);

            // Posição do clique
            var positionLabel = new Label
            {
                Text = "Posição do clique:",
                Location = new Point(10, 155),
                Size = new Size(120, 20)
            };
            this.Controls.Add(positionLabel);

            var positionBtn = new Button
            {
                Text = "Definir Posição (F10)",
                Location = new Point(140, 152),
                Size = new Size(150, 25)
            };
            positionBtn.Click += (s, e) => SetClickPosition();
            this.Controls.Add(positionBtn);

            var positionInfo = new Label
            {
                Text = "Centro da janela",
                Location = new Point(300, 155),
                Size = new Size(150, 20),
                ForeColor = Color.Gray
            };
            this.Controls.Add(positionInfo);

            // Teclas de atalho
            var hotkeyLabel = new Label
            {
                Text = "Teclas: F9 = Iniciar/Parar | F10 = Definir posição",
                Location = new Point(10, 180),
                Size = new Size(450, 20),
                ForeColor = Color.Blue,
                Font = new Font("Arial", 8)
            };
            this.Controls.Add(hotkeyLabel);

            // Botões
            startBtn = new Button
            {
                Text = "Iniciar (F9)",
                Location = new Point(300, 200),
                Size = new Size(100, 30)
            };
            startBtn.Click += StartBtn_Click;
            this.Controls.Add(startBtn);

            stopBtn = new Button
            {
                Text = "Parar (F9)",
                Location = new Point(410, 200),
                Size = new Size(80, 30),
                Enabled = false
            };
            stopBtn.Click += StopBtn_Click;
            this.Controls.Add(stopBtn);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                switch (m.WParam.ToInt32())
                {
                    case 1: // F9 - Iniciar/Parar
                        if (isClicking)
                            StopBtn_Click(this, EventArgs.Empty);
                        else
                            StartBtn_Click(this, EventArgs.Empty);
                        break;
                    case 2: // F10 - Definir posição
                        SetClickPosition();
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void RegisterHotKeys()
        {
            RegisterHotKey(this.Handle, 1, 0, 0x78); // F9
            RegisterHotKey(this.Handle, 2, 0, 0x79); // F10
        }

        private void SetClickPosition()
        {
            GetCursorPos(out Point cursorPos);
            IntPtr windowAtCursor = WindowFromPoint(cursorPos);
            
            if (windowCombo.SelectedItem != null && 
                windowHandles.TryGetValue(windowCombo.SelectedItem.ToString() ?? "", out var targetWindow))
            {
                if (windowAtCursor == targetWindow)
                {
                    Point clientPos = cursorPos;
                    ScreenToClient(targetWindow, ref clientPos);
                    clickPosition = clientPos;
                    useCustomPosition = true;
                    MessageBox.Show($"Posição definida: {clientPos.X}, {clientPos.Y}", "Posição do Clique", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Posicione o cursor sobre a janela selecionada!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Selecione uma janela primeiro!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void RefreshWindows()
        {
            windowHandles.Clear();
            windowCombo.Items.Clear();

            int totalWindows = 0;
            int visibleWindows = 0;
            int validWindows = 0;

            EnumWindows((h, l) =>
            {
                totalWindows++;

                if (!IsWindowVisible(h)) return true;
                visibleWindows++;

                int len = GetWindowTextLength(h);
                if (len == 0) return true;

                var sb = new StringBuilder(len + 1);
                GetWindowText(h, sb, sb.Capacity);
                var title = sb.ToString();

                if (!string.IsNullOrEmpty(title) && title.Length > 1)
                {
                    validWindows++;

                    if (!title.Contains("AutoClicker") &&
                        !title.Contains("Program Manager") &&
                        !title.Contains("Desktop") &&
                        !title.Contains("Taskbar") &&
                        !title.Contains("Start"))
                    {
                        windowCombo.Items.Add(title);
                        windowHandles[title] = h;
                    }
                }
                return true;
            }, IntPtr.Zero);

            this.Text = $"AutoClicker Minecraft - Total: {totalWindows}, Visíveis: {visibleWindows}, Válidas: {validWindows}, Lista: {windowCombo.Items.Count}";

            if (windowCombo.Items.Count > 0)
            {
                windowCombo.SelectedIndex = 0;
            }
        }

        private void RefreshBtn_Click(object? sender, EventArgs e)
        {
            RefreshWindows();
        }

        private void SaveSettings()
        {
            try
            {
                var lines = new List<string>
                {
                    $"Window={windowCombo.SelectedItem?.ToString() ?? ""}",
                    $"Button={buttonCombo.SelectedItem?.ToString() ?? ""}",
                    $"Interval={intervalBox.Text}",
                    $"OnlyWhenOpen={onlyWhenOpenCheck.Checked}",
                    $"AutoEscape={autoEscape}",
                    $"UseCustomPosition={useCustomPosition}",
                    $"ClickPositionX={clickPosition.X}",
                    $"ClickPositionY={clickPosition.Y}"
                };
                File.WriteAllLines(configFile, lines);
            }
            catch (Exception)
            {
                // Silenciosamente ignora erros de salvamento
            }
        }

        private void LoadSettings()
        {
            try
            {
                if (!File.Exists(configFile)) return;

                var lines = File.ReadAllLines(configFile);
                foreach (var line in lines)
                {
                    var parts = line.Split('=', 2);
                    if (parts.Length != 2) continue;

                    switch (parts[0])
                    {
                        case "Window":
                            if (!string.IsNullOrEmpty(parts[1]))
                            {
                                for (int i = 0; i < windowCombo.Items.Count; i++)
                                {
                                    if (windowCombo.Items[i]?.ToString() == parts[1])
                                    {
                                        windowCombo.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        case "Button":
                            if (!string.IsNullOrEmpty(parts[1]))
                            {
                                for (int i = 0; i < buttonCombo.Items.Count; i++)
                                {
                                    if (buttonCombo.Items[i]?.ToString() == parts[1])
                                    {
                                        buttonCombo.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        case "Interval":
                            intervalBox.Text = parts[1];
                            break;
                        case "OnlyWhenOpen":
                            onlyWhenOpenCheck.Checked = bool.Parse(parts[1]);
                            break;
                        case "AutoEscape":
                            autoEscape = bool.Parse(parts[1]);
                            break;
                        case "UseCustomPosition":
                            useCustomPosition = bool.Parse(parts[1]);
                            break;
                        case "ClickPositionX":
                            if (int.TryParse(parts[1], out int x))
                                clickPosition.X = x;
                            break;
                        case "ClickPositionY":
                            if (int.TryParse(parts[1], out int y))
                                clickPosition.Y = y;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // Silenciosamente ignora erros de carregamento
            }
        }

        private void StartBtn_Click(object? sender, EventArgs e)
        {
            if (windowCombo.SelectedItem == null) return;
            if (!windowHandles.TryGetValue(windowCombo.SelectedItem?.ToString() ?? "", out var hwnd)) return;
            if (!int.TryParse(intervalBox.Text, out var interval) || interval < 1) return;

            targetWindow = hwnd;
            var right = buttonCombo.SelectedItem?.ToString()?.StartsWith("Direito", StringComparison.OrdinalIgnoreCase) ?? false;

            // Timer para ESC contínuo (mais frequente)
            escapeTimer?.Stop();
            if (autoEscape)
            {
                escapeTimer = new System.Windows.Forms.Timer();
                escapeTimer.Interval = 2000; // ESC a cada 2 segundos
                escapeTimer.Tick += (s, args) =>
                {
                    if (!IsWindow(targetWindow)) return;
                    PostMessage(targetWindow, WM_KEYDOWN, (IntPtr)VK_ESCAPE, IntPtr.Zero);
                    PostMessage(targetWindow, WM_KEYUP, (IntPtr)VK_ESCAPE, IntPtr.Zero);
                };
                escapeTimer.Start();
            }

            // Timer para cliques
            clickTimer?.Stop();
            clickTimer = new System.Windows.Forms.Timer();
            clickTimer.Interval = interval;
            clickTimer.Tick += (s, args) =>
            {
                if (!IsWindow(hwnd))
                {
                    if (onlyWhenOpenCheck.Checked) return;
                }

                int x, y;
                if (useCustomPosition)
                {
                    x = clickPosition.X;
                    y = clickPosition.Y;
                }
                else
                {
                    RECT r;
                    if (!GetClientRect(hwnd, out r)) return;
                    x = Math.Max(1, (r.Right - r.Left) / 2);
                    y = Math.Max(1, (r.Bottom - r.Top) / 2);
                }

                int lParam = (y << 16) | (x & 0xFFFF);

                uint down = right ? WM_RBUTTONDOWN : WM_LBUTTONDOWN;
                uint up = right ? WM_RBUTTONUP : WM_LBUTTONUP;

                PostMessage(hwnd, down, IntPtr.Zero, (IntPtr)lParam);
                PostMessage(hwnd, up, IntPtr.Zero, (IntPtr)lParam);
            };

            clickTimer.Start();
            isClicking = true;

            startBtn.Enabled = false;
            stopBtn.Enabled = true;
            refreshBtn.Enabled = false;
            windowCombo.Enabled = false;
            buttonCombo.Enabled = false;
            intervalBox.Enabled = false;
            onlyWhenOpenCheck.Enabled = false;
        }

        private void StopBtn_Click(object? sender, EventArgs e)
        {
            clickTimer?.Stop();
            clickTimer = null;
            escapeTimer?.Stop();
            escapeTimer = null;
            isClicking = false;

            startBtn.Enabled = true;
            stopBtn.Enabled = false;
            refreshBtn.Enabled = true;
            windowCombo.Enabled = true;
            buttonCombo.Enabled = true;
            intervalBox.Enabled = true;
            onlyWhenOpenCheck.Enabled = true;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SaveSettings();
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            base.OnFormClosed(e);
        }
    }
}
