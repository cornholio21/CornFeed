using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Reflection;
using System.Diagnostics;

namespace CornFeed
{
    public partial class Form1 : Form
    {
        //Vars

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _processMonitorCts;
        private FileSystemWatcher _fileWatcher;
        private bool isParsingOldLog = false;
        private long lastReadPosition = 0;
        private int soundSettings = 0;
        private string soundWAVPath = null;
        private Task _logMonitorTask;
        private Task _processMonitorTask;
        private bool isLoggerRestarted = false;

        //Styling

        private const int BORDER_SIZE = 8; // Thickness of the resize border
        private const int WM_NCHITTEST = 0x84;
        private const int HTLEFT = 10, HTRIGHT = 11, HTTOP = 12, HTTOPLEFT = 13, HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15, HTBOTTOMLEFT = 16, HTBOTTOMRIGHT = 17;
        private const int HTCAPTION = 2;

        //Window Moving

        [DllImport("user32.dll")]
        private static extern void ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;

        //Functions

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            Pre_exit_run();
            _fileWatcher?.Dispose();
        }

        private void Pre_exit_run()
        {
            SaveConfig();
        }

        //Save config

        private void SaveConfig()
        {
            try
            {
                string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CornFeed", "CornFeed.ini");
                Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));

                File.WriteAllLines(configFilePath, new[]
                {
            $"LogPath={tb_log_path.Text}",
            $"ShowOldEntries={cb_show_old.Checked}",
            $"LogNPCs={cb_show_npc_kills.Checked}",
            $"FullNPCNames={cb_show_full_npc_names.Checked}",
            $"Width={Width}",
            $"Height={Height}",
            $"Left={Left}",
            $"Top={Top}",
            $"FeedColor={rtb_feed.ForeColor.ToArgb()}",
            $"FeedBackground={rtb_feed.BackColor.ToArgb()}",
            $"FeedFont={rtb_feed.Font.FontFamily.Name}",
            $"FeedFontSize={rtb_feed.Font.Size}",
            $"soundMode={soundSettings}",
            $"WAVFilePath={soundWAVPath}"
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save config: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Load config

        private void LoadConfig()
        {
            try
            {
                string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CornFeed", "CornFeed.ini");
                if (!File.Exists(configFilePath)) return;

                foreach (var line in File.ReadLines(configFilePath))
                {
                    string[] parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length < 2) continue;

                    string key = parts[0], value = parts[1];

                    switch (key)
                    {
                        case "LogPath": tb_log_path.Text = value; break;
                        case "ShowOldEntries": cb_show_old.Checked = bool.TryParse(value, out var showOld) && showOld; break;
                        case "LogNPCs":
                            cb_show_npc_kills.Checked = bool.TryParse(value, out var logNpcs) && logNpcs;
                            cb_show_full_npc_names.Enabled = cb_show_npc_kills.Checked;
                            break;
                        case "FullNPCNames": cb_show_full_npc_names.Checked = bool.TryParse(value, out var fullNpc) && fullNpc; break;
                        case "Width": if (int.TryParse(value, out var width)) Width = width; break;
                        case "Height": if (int.TryParse(value, out var height)) Height = height; break;
                        case "Left": if (int.TryParse(value, out var left)) Left = left; break;
                        case "Top": if (int.TryParse(value, out var top)) Top = top; break;
                        case "FeedColor": if (int.TryParse(value, out var feedColor)) rtb_feed.ForeColor = Color.FromArgb(feedColor); break;
                        case "FeedBackground": if (int.TryParse(value, out var feedBackground)) rtb_feed.BackColor = Color.FromArgb(feedBackground); break;
                        case "FeedFont": rtb_feed.Font = new Font(value, rtb_feed.Font.Size); break;
                        case "FeedFontSize": if (float.TryParse(value, out var feedSize)) rtb_feed.Font = new Font(rtb_feed.Font.FontFamily, feedSize); break;
                        case "soundMode": if (int.TryParse(value, out var soundMode)) soundSettings = soundMode; break;
                        case "WAVFilePath": soundWAVPath = value; break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load config: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Window Moving

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                uint lparam32 = (uint)m.LParam.ToInt64();
                short x = (short)((uint)lparam32 & 0xFFFF);
                short y = (short)(((uint)lparam32 >> 16) & 0xFFFF);
                Point cursor = PointToClient(new Point(x, y));

                // Enable resizing
                if (cursor.X <= BORDER_SIZE && cursor.Y <= BORDER_SIZE) m.Result = (IntPtr)HTTOPLEFT; // Top-left
                else if (cursor.X >= Width - BORDER_SIZE && cursor.Y <= BORDER_SIZE) m.Result = (IntPtr)HTTOPRIGHT; // Top-right
                else if (cursor.X <= BORDER_SIZE && cursor.Y >= Height - BORDER_SIZE) m.Result = (IntPtr)HTBOTTOMLEFT; // Bottom-left
                else if (cursor.X >= Width - BORDER_SIZE && cursor.Y >= Height - BORDER_SIZE) m.Result = (IntPtr)HTBOTTOMRIGHT; // Bottom-right
                else if (cursor.X <= BORDER_SIZE) m.Result = (IntPtr)HTLEFT; // Left
                else if (cursor.X >= Width - BORDER_SIZE) m.Result = (IntPtr)HTRIGHT; // Right
                else if (cursor.Y <= BORDER_SIZE) m.Result = (IntPtr)HTTOP; // Top
                else if (cursor.Y >= Height - BORDER_SIZE) m.Result = (IntPtr)HTBOTTOM; // Bottom
                else m.Result = (IntPtr)HTCAPTION; // Allow dragging the form
                return;
            }
            base.WndProc(ref m);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        public static bool IsNPC(string name)
        {
            return Regex.IsMatch(name, @"\d{13}$");
        }

        public static bool IsPlayer(string name)
        {
            return !IsNPC(name) && name != "unknown";
        }

        private void Btn_exit_click(object sender, EventArgs e)
        {
            Close();
        }

        //Select logfile

        private void btn_select_file_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (!string.IsNullOrWhiteSpace(tb_log_path.Text) && File.Exists(tb_log_path.Text))
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(tb_log_path.Text);
                }
                else
                {
                    openFileDialog.InitialDirectory = @"C:\Program Files\Roberts Space Industries";
                }

                openFileDialog.FileName = "Game.log";
                openFileDialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tb_log_path.Text = openFileDialog.FileName;
                }
            }
        }

        //Start/Stop button

        private void btn_status_Click(object sender, EventArgs e)
        {
            if (btn_status.Text == "start")
            {
                if (!File.Exists(tb_log_path.Text))
                {
                    MessageBox.Show("Invalid file path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                rtb_feed.Clear();
                lastReadPosition = 0;
                isLoggerRestarted = true;
                StartLogMonitoring(tb_log_path.Text);
                StartProcessMonitoring();
                UpdateStatus("running", System.Drawing.Color.Lime);
            }
            else
            {
                _processMonitorCts.Cancel();
                _processMonitorTask = null;
                StopLogMonitoring();
                isLoggerRestarted = false;
                UpdateStatus("stopped", System.Drawing.Color.Orange);
            }
        }

        //Start/stop monitoring
        
        private void StartLogMonitoring(string filePath)
        {
            if (_logMonitorTask != null) return;
            _cancellationTokenSource = new CancellationTokenSource();
            _logMonitorTask = Task.Run(() => MonitorLogFile(filePath, _cancellationTokenSource.Token));
        }
        private void StopLogMonitoring()
        {
            _cancellationTokenSource.Cancel();
            _logMonitorTask = null;
        }

        private void RestartLogMonitoring()
        {
            StopLogMonitoring();
            lastReadPosition = 0;
            StartLogMonitoring(tb_log_path.Text);
            UpdateStatus("running", System.Drawing.Color.Lime);
        }

        //Monitor Game.log file

        private async Task MonitorLogFile(string filePath, CancellationToken token)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                {
                    if (cb_show_old.Checked) 
                    { 
                        stream.Seek(lastReadPosition, SeekOrigin.Begin); 
                    } else
                    {
                        stream.Seek(lastReadPosition, SeekOrigin.End);
                    }

                    while (!token.IsCancellationRequested)
                    {
                        string line = await reader.ReadLineAsync();
                        if (line != null)
                        {
                            var result = ParseLogLine(line);
                            if (result != null)
                            {
                                AppendToFeed(result, true);
                            }
                            lastReadPosition = stream.Position; 
                        }
                        else
                        {
                            await Task.Delay(100, token); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!token.IsCancellationRequested)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _cancellationTokenSource.Cancel();
                    _processMonitorCts.Cancel();
                    UpdateStatus("stopped", System.Drawing.Color.Orange);
                }
            }
        }

        private void cb_show_npc_kills_Checked(object sender, EventArgs e)
        {
            if (cb_show_npc_kills.Checked) cb_show_full_npc_names.Enabled = true;
            if (!cb_show_npc_kills.Checked) cb_show_full_npc_names.Enabled = false;
        }

        //Parse lines of the log file, get timestamp and determine if Player or NPC is involved

        private string ParseLogLine(string line)
        {
            var timestampMatch = Regex.Match(line, @"<(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}Z)>");
            if (!timestampMatch.Success) return null;

            string timestamp = timestampMatch.Groups[1].Value;
            DateTime parsedTimestamp;
            if (!DateTime.TryParse(timestamp, out parsedTimestamp)) return null;

            timestamp = parsedTimestamp.ToString("HH:mm");

            var match = Regex.Match(line, @"'(.*?)'.*?killed by '(.*?)'.*?damage type '(.*?)'");
            if (!match.Success) return null;

            string victim = match.Groups[1].Value;
            string killer = match.Groups[2].Value;

            bool victimIsPlayer = IsPlayer(victim);
            bool killerIsPlayer = IsPlayer(killer);

            if (killer == "unknown") return null;

            if (!victimIsPlayer && !killerIsPlayer) return null;

            if (victim == killer && victimIsPlayer)
            {
                return $"[{timestamp}] {victim} died (self-inflicted)";
            }

            if (victimIsPlayer && killerIsPlayer)
            {
                return $"[{timestamp}] {killer} killed {victim}";
            }
            if ((victimIsPlayer || killerIsPlayer) && cb_show_npc_kills.Checked)
            {
                if (cb_show_full_npc_names.Checked)
                {
                    victim = Regex.Replace(victim, @"_\d{13}$", "");
                    killer = Regex.Replace(killer, @"_\d{13}$", "");
                    return $"[{timestamp}] {killer} killed {victim}";
                }
                else
                {
                    if (killerIsPlayer) return $"[{timestamp}] {killer} killed NPC";
                    if (victimIsPlayer) return $"[{timestamp}] NPC killed {victim}";
                }
            }

            return null;
        }

        //UI Handler

        private void UpdateStatus(string status, System.Drawing.Color color)
        {
            if (lb_status.InvokeRequired)
            {
                lb_status.Invoke(new Action(() =>
                {
                    lb_status.Text = $"Status: {status}";
                    lb_status.ForeColor = color;

                    if (status == "running" || status == "waiting for game")
                    {
                        btn_status.Text = "stop";
                    }
                    else
                    {
                        btn_status.Text = "start";
                    }

                    // Disable the controls for "running" and "waiting for game" states
                    bool disableControls = status == "running" || status == "waiting for game";

                    cb_show_full_npc_names.Enabled = !disableControls;
                    cb_show_npc_kills.Enabled = !disableControls;
                    cb_show_old.Enabled = !disableControls;
                    btn_select_file.Enabled = !disableControls;
                }));
            }
            else
            {
                lb_status.Text = $"Status: {status}";
                lb_status.ForeColor = color;

                if (status == "running" || status == "waiting for game")
                {
                    btn_status.Text = "stop";
                }
                else
                {
                    btn_status.Text = "start";
                }

                // Disable the controls for "running" and "waiting for game" states
                bool disableControls = status == "running" || status == "waiting for game";

                cb_show_full_npc_names.Enabled = !disableControls;
                cb_show_npc_kills.Enabled = !disableControls;
                cb_show_old.Enabled = !disableControls;
                btn_select_file.Enabled = !disableControls;
            }
        }

        //Add entry to the feed

        private void AppendToFeed(string message, bool playSound)
        {
            if (rtb_feed.InvokeRequired)
            {
                rtb_feed.BeginInvoke(new Action(() =>
                {
                    rtb_feed.Text = message + Environment.NewLine + rtb_feed.Text;
                    rtb_feed.SelectionStart = 0;
                    rtb_feed.ScrollToCaret();
                    if (playSound && !isParsingOldLog && soundSettings > 0)
                    {
                        PlaySound();
                    }
                }));
            }
            else
            {
                rtb_feed.Text = message + Environment.NewLine + rtb_feed.Text;
                rtb_feed.SelectionStart = 0;
                rtb_feed.ScrollToCaret();
                if (playSound && !isParsingOldLog && soundSettings > 0)
                {
                    PlaySound();
                }
            }
        }

        //Settings button

        private void bt_settings_Click(object sender, EventArgs e)
        {
            string currentFont = rtb_feed.Font.Name;
            float currentFontSize = rtb_feed.Font.Size;
            Color currentBackgroundColor = rtb_feed.BackColor;
            Color currentTextColor = rtb_feed.ForeColor;
            int currentSoundSettings = soundSettings;
            string currentWAVFile = soundWAVPath;

            SettingsForm settingsForm = new SettingsForm(currentFont, currentFontSize, currentBackgroundColor, currentTextColor, currentSoundSettings, currentWAVFile);
            var result = settingsForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                rtb_feed.Font = new Font(settingsForm.SelectedFont, settingsForm.FontSize);
                rtb_feed.BackColor = settingsForm.BackgroundColor;
                rtb_feed.ForeColor = settingsForm.FontColor;
                soundSettings = settingsForm.cob_sound_mode.SelectedIndex;
                if (!string.IsNullOrWhiteSpace(settingsForm.tb_settings_wav_file.Text) && File.Exists(settingsForm.tb_settings_wav_file.Text))
                {
                    soundWAVPath = settingsForm.tb_settings_wav_file.Text;
                }
                else
                {
                    soundWAVPath = null;
                }
            }
        }

        //Sound integration T0 LUL

        private void PlaySound()
        {
            if (!string.IsNullOrEmpty(soundWAVPath) && File.Exists(soundWAVPath))
            {
                using (SoundPlayer player = new SoundPlayer(soundWAVPath))
                {
                    player.Play();
                }
            }
            else
            {
                using (Stream soundStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CornFeed.WAV_Sharp.wav"))
                {
                    if (soundStream != null)
                    {
                        using (SoundPlayer player = new SoundPlayer(soundStream))
                        {
                            player.Play();
                        }
                    }
                }
            }
        }

        //SC process monitoring

        private void StartProcessMonitoring()
        {
            if (_processMonitorTask != null) return; // Prevent multiple tasks from starting
            _processMonitorCts = new CancellationTokenSource();
            _processMonitorTask = Task.Run(() => MonitorStarCitizenProcess(_processMonitorCts.Token));
        }


        private async Task MonitorStarCitizenProcess(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var starCitizenProcess = Process.GetProcessesByName("StarCitizen").FirstOrDefault();

                if (starCitizenProcess != null) // If the process is found
                {
                    if (!isLoggerRestarted) // Check if the logger hasn't been restarted yet
                    {
                        // Process found, wait 10 seconds before restarting logging
                        await Task.Delay(30000, token); // Wait for 10 seconds
                        RestartLogMonitoring(); // Restart log monitoring after 10 seconds
                        isLoggerRestarted = true; // Set the flag to true so the logger won't restart again
                    }
                }
                else
                {
                    // Process not found, stop logging
                    await Task.Delay(5000, token);
                    StopLogMonitoring();
                    UpdateStatus("waiting for game", Color.Yellow);

                    isLoggerRestarted = false; // Reset the flag when the game is not found
                }

                await Task.Delay(1000, token); // Check every 1 second for the process
            }
        }
    }
}
