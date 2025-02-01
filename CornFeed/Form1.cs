using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
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

        public Form1()
        {
            InitializeComponent();

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

        private void SaveConfig()
        {
            try
            {
                string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CornFeed");
                string configFilePath = Path.Combine(appDataFolder, "CornFeed.ini");
                Directory.CreateDirectory(appDataFolder);

                var lines = new List<string>
                {
                    $"LogPath={tb_log_path.Text}",
                    $"ShowOldEntries={cb_show_old.Checked}",
                    $"LogNPCs={cb_show_npc_kills.Checked}",
                    $"FullNPCNames={cb_show_full_npc_names.Checked}",
                    $"Width={this.Width}",
                    $"Height={this.Height}",
                    $"Left={this.Left}",
                    $"Top={this.Top}",
                    $"FeedColor={rtb_feed.ForeColor.ToArgb()}",
                    $"FeedBackground={rtb_feed.BackColor.ToArgb()}",
                    $"FeedFont={rtb_feed.Font.FontFamily.Name}",
                    $"FeedFontSize={rtb_feed.Font.Size}",
                    $"soundMode={soundSettings}",
                    $"WAVFilePath={soundWAVPath}"
                };
                File.WriteAllLines(configFilePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save config: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadConfig()
        {
            try
            {
                string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CornFeed");
                string configFilePath = Path.Combine(appDataFolder, "CornFeed.ini");

                if (!File.Exists(configFilePath)) return;

                var lines = File.ReadAllLines(configFilePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("LogPath="))
                    {
                        tb_log_path.Text = line.Substring("LogPath=".Length);
                    }
                    else if (line.StartsWith("ShowOldEntries="))
                    {
                        if (bool.TryParse(line.Substring("ShowOldEntries=".Length), out bool showOld))
                        {
                            cb_show_old.Checked = showOld;
                        }
                    }
                    else if (line.StartsWith("LogNPCs="))
                    {
                        if (bool.TryParse(line.Substring("LogNPCs=".Length), out bool showOld))
                        {
                            cb_show_npc_kills.Checked = showOld;
                            cb_show_full_npc_names.Enabled = showOld;
                        }
                    }
                    else if (line.StartsWith("FullNPCNames="))
                    {
                        if (bool.TryParse(line.Substring("FullNPCNames=".Length), out bool showOld))
                        {
                            cb_show_full_npc_names.Checked = showOld;
                        }
                    }
                    else if (line.StartsWith("Width="))
                    {
                        if (int.TryParse(line.Substring("Width=".Length), out int width))
                        {
                            this.Width = width;
                        }
                    }
                    else if (line.StartsWith("Height="))
                    {
                        if (int.TryParse(line.Substring("Height=".Length), out int height))
                        {
                            this.Height = height;
                        }
                    }
                    else if (line.StartsWith("Left="))
                    {
                        if (int.TryParse(line.Substring("Left=".Length), out int left))
                        {
                            this.Left = left;
                        }
                    }
                    else if (line.StartsWith("Top="))
                    {
                        if (int.TryParse(line.Substring("Top=".Length), out int top))
                        {
                            this.Top = top;
                        }
                    }
                    else if (line.StartsWith("FeedColor="))
                    {
                        string FeedColorString = line.Substring("FeedColor=".Length);
                        try
                        {
                            rtb_feed.ForeColor = Color.FromArgb(int.Parse(FeedColorString));
                        }
                        catch (Exception ex)
                        {
                            // DEFAULT
                        }
                    }
                    else if (line.StartsWith("FeedBackground="))
                    {
                        string FeedBackColorString = line.Substring("FeedBackground=".Length);
                        try
                        {
                            rtb_feed.BackColor = Color.FromArgb(int.Parse(FeedBackColorString));

                        }
                        catch (Exception ex)
                        {
                            // DEFAULT
                        }
                    }
                    else if (line.StartsWith("FeedFont="))
                    {
                        string FeedFontFamily = line.Substring("FeedFont=".Length);
                        try
                        {
                            rtb_feed.Font = new Font(FeedFontFamily, rtb_feed.Font.Size);
                        }
                        catch (Exception ex)
                        {
                            // DEFAULT
                        }
                    }
                    else if (line.StartsWith("FeedFontSize="))
                    {
                        if (int.TryParse(line.Substring("FeedFontSize=".Length), out int FeedSize))
                        {
                            rtb_feed.Font = new Font(rtb_feed.Font.FontFamily, FeedSize);
                        }
                    }
                    else if (line.StartsWith("soundMode="))
                    {
                        if (int.TryParse(line.Substring("soundMode=".Length), out int soundMode))
                        {
                            soundSettings = soundMode;
                        }
                    }
                    else if (line.StartsWith("WAVFilePath="))
                    {
                        soundWAVPath = line.Substring("WAVFilePath=".Length);
                    }
                }
            }
            catch (Exception ex)
            {
                // NO
            }
        }

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
