using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CornFeed
{
    public partial class SettingsForm : Form
    {
        public string SelectedFont { get; private set; }
        public float FontSize { get; private set; }
        public Color BackgroundColor { get; private set; }
        public Color FontColor { get; private set; }
        public string WAVFilePath { get; private set; }
        public int SoundSettings { get; private set; }

        public SettingsForm(string currentFont, float currentFontSize, Color currentBackgroundColor, Color fontColor, int currentSoundSettings, string currentWAVFile)
        {
            InitializeComponent();

            // Ensure controls are initialized
            if (cob_font != null)
            {
                // Populate ComboBox with system fonts
                foreach (FontFamily font in FontFamily.Families)
                {
                    cob_font.Items.Add(font.Name);
                }

                // Set the current font, font size, and background color in the controls
                cob_font.SelectedItem = currentFont;
            }

            if (num_fontSize != null)
            {
                num_fontSize.Value = (decimal)currentFontSize;
            }

            if (colorDialog1 != null)
            {
                colorDialog1.Color = currentBackgroundColor;
            }

            cob_sound_mode.SelectedIndex = currentSoundSettings;

            if (currentWAVFile != null) 
            {
                tb_settings_wav_file.Text = currentWAVFile;
            }

            // Initialize properties
            SelectedFont = currentFont;
            FontSize = currentFontSize;
            BackgroundColor = currentBackgroundColor;
            FontColor = fontColor;
            SoundSettings = currentSoundSettings;
            WAVFilePath = currentWAVFile;
        }

        private void cob_font_Changed(object sender, EventArgs e)
        {
            if (cob_font.SelectedItem != null)
            {
                SelectedFont = cob_font.SelectedItem.ToString();
            }
        }

        private void num_fontSIze_Changed(object sender, EventArgs e)
        {
            FontSize = (float)num_fontSize.Value;
        }

        private void btn_color_click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                BackgroundColor = colorDialog1.Color;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_font_color_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                FontColor = colorDialog1.Color;
            }
        }

        private void bt_settings_select_wav_file_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (!string.IsNullOrWhiteSpace(tb_settings_wav_file.Text) && File.Exists(tb_settings_wav_file.Text))
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(tb_settings_wav_file.Text);
                }
                else
                {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                openFileDialog.Filter = "WAV files (*.wav)|*.wav";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tb_settings_wav_file.Text = openFileDialog.FileName;
                }
            }
        }

        private void cob_sound_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SoundSettings = cob_sound_mode.SelectedIndex;
        }

        private void bt_settings_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
