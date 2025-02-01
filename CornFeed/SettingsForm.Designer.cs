using System.Windows.Forms;

namespace CornFeed
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.cob_font = new System.Windows.Forms.ComboBox();
            this.num_fontSize = new System.Windows.Forms.NumericUpDown();
            this.btn_background_color = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btn_font_color = new System.Windows.Forms.Button();
            this.gb_Appearence = new System.Windows.Forms.GroupBox();
            this.lb_settings_font = new System.Windows.Forms.Label();
            this.lb_settings_color = new System.Windows.Forms.Label();
            this.gb_Audio = new System.Windows.Forms.GroupBox();
            this.bt_settings_select_wav_file = new System.Windows.Forms.Button();
            this.tb_settings_wav_file = new System.Windows.Forms.TextBox();
            this.lb_settings_WAV_path = new System.Windows.Forms.Label();
            this.lb_settings_sound_mode = new System.Windows.Forms.Label();
            this.cob_sound_mode = new System.Windows.Forms.ComboBox();
            this.bt_settings_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_fontSize)).BeginInit();
            this.gb_Appearence.SuspendLayout();
            this.gb_Audio.SuspendLayout();
            this.SuspendLayout();
            // 
            // cob_font
            // 
            this.cob_font.FormattingEnabled = true;
            this.cob_font.Location = new System.Drawing.Point(6, 89);
            this.cob_font.Name = "cob_font";
            this.cob_font.Size = new System.Drawing.Size(201, 21);
            this.cob_font.TabIndex = 0;
            this.cob_font.SelectedIndexChanged += new System.EventHandler(this.cob_font_Changed);
            // 
            // num_fontSize
            // 
            this.num_fontSize.Location = new System.Drawing.Point(213, 90);
            this.num_fontSize.Name = "num_fontSize";
            this.num_fontSize.Size = new System.Drawing.Size(43, 20);
            this.num_fontSize.TabIndex = 1;
            this.num_fontSize.ValueChanged += new System.EventHandler(this.num_fontSIze_Changed);
            // 
            // btn_background_color
            // 
            this.btn_background_color.Location = new System.Drawing.Point(87, 38);
            this.btn_background_color.Name = "btn_background_color";
            this.btn_background_color.Size = new System.Drawing.Size(75, 23);
            this.btn_background_color.TabIndex = 2;
            this.btn_background_color.Text = "Background";
            this.btn_background_color.UseVisualStyleBackColor = true;
            this.btn_background_color.Click += new System.EventHandler(this.btn_color_click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(536, 156);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 3;
            this.btn_save.Text = "save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_font_color
            // 
            this.btn_font_color.Location = new System.Drawing.Point(6, 38);
            this.btn_font_color.Name = "btn_font_color";
            this.btn_font_color.Size = new System.Drawing.Size(75, 23);
            this.btn_font_color.TabIndex = 4;
            this.btn_font_color.Text = "Text";
            this.btn_font_color.UseVisualStyleBackColor = true;
            this.btn_font_color.Click += new System.EventHandler(this.btn_font_color_Click);
            // 
            // gb_Appearence
            // 
            this.gb_Appearence.Controls.Add(this.lb_settings_font);
            this.gb_Appearence.Controls.Add(this.lb_settings_color);
            this.gb_Appearence.Controls.Add(this.btn_background_color);
            this.gb_Appearence.Controls.Add(this.btn_font_color);
            this.gb_Appearence.Controls.Add(this.num_fontSize);
            this.gb_Appearence.Controls.Add(this.cob_font);
            this.gb_Appearence.ForeColor = System.Drawing.Color.Black;
            this.gb_Appearence.Location = new System.Drawing.Point(12, 12);
            this.gb_Appearence.Name = "gb_Appearence";
            this.gb_Appearence.Size = new System.Drawing.Size(262, 129);
            this.gb_Appearence.TabIndex = 5;
            this.gb_Appearence.TabStop = false;
            this.gb_Appearence.Text = "Appearance";
            // 
            // lb_settings_font
            // 
            this.lb_settings_font.AutoSize = true;
            this.lb_settings_font.Location = new System.Drawing.Point(6, 72);
            this.lb_settings_font.Name = "lb_settings_font";
            this.lb_settings_font.Size = new System.Drawing.Size(62, 13);
            this.lb_settings_font.TabIndex = 6;
            this.lb_settings_font.Text = "Font / Size:";
            // 
            // lb_settings_color
            // 
            this.lb_settings_color.AutoSize = true;
            this.lb_settings_color.Location = new System.Drawing.Point(6, 22);
            this.lb_settings_color.Name = "lb_settings_color";
            this.lb_settings_color.Size = new System.Drawing.Size(39, 13);
            this.lb_settings_color.TabIndex = 5;
            this.lb_settings_color.Text = "Colors:";
            // 
            // gb_Audio
            // 
            this.gb_Audio.Controls.Add(this.bt_settings_select_wav_file);
            this.gb_Audio.Controls.Add(this.tb_settings_wav_file);
            this.gb_Audio.Controls.Add(this.lb_settings_WAV_path);
            this.gb_Audio.Controls.Add(this.lb_settings_sound_mode);
            this.gb_Audio.Controls.Add(this.cob_sound_mode);
            this.gb_Audio.Location = new System.Drawing.Point(292, 12);
            this.gb_Audio.Name = "gb_Audio";
            this.gb_Audio.Size = new System.Drawing.Size(319, 129);
            this.gb_Audio.TabIndex = 6;
            this.gb_Audio.TabStop = false;
            this.gb_Audio.Text = "Sound";
            // 
            // bt_settings_select_wav_file
            // 
            this.bt_settings_select_wav_file.Location = new System.Drawing.Point(237, 89);
            this.bt_settings_select_wav_file.Name = "bt_settings_select_wav_file";
            this.bt_settings_select_wav_file.Size = new System.Drawing.Size(75, 23);
            this.bt_settings_select_wav_file.TabIndex = 4;
            this.bt_settings_select_wav_file.Text = "select";
            this.bt_settings_select_wav_file.UseVisualStyleBackColor = true;
            this.bt_settings_select_wav_file.Click += new System.EventHandler(this.bt_settings_select_wav_file_Click);
            // 
            // tb_settings_wav_file
            // 
            this.tb_settings_wav_file.Location = new System.Drawing.Point(6, 90);
            this.tb_settings_wav_file.Name = "tb_settings_wav_file";
            this.tb_settings_wav_file.Size = new System.Drawing.Size(225, 20);
            this.tb_settings_wav_file.TabIndex = 3;
            // 
            // lb_settings_WAV_path
            // 
            this.lb_settings_WAV_path.AutoSize = true;
            this.lb_settings_WAV_path.Location = new System.Drawing.Point(7, 72);
            this.lb_settings_WAV_path.Name = "lb_settings_WAV_path";
            this.lb_settings_WAV_path.Size = new System.Drawing.Size(89, 13);
            this.lb_settings_WAV_path.TabIndex = 2;
            this.lb_settings_WAV_path.Text = "Custom WAV file:";
            // 
            // lb_settings_sound_mode
            // 
            this.lb_settings_sound_mode.AutoSize = true;
            this.lb_settings_sound_mode.Location = new System.Drawing.Point(7, 21);
            this.lb_settings_sound_mode.Name = "lb_settings_sound_mode";
            this.lb_settings_sound_mode.Size = new System.Drawing.Size(37, 13);
            this.lb_settings_sound_mode.TabIndex = 1;
            this.lb_settings_sound_mode.Text = "Mode:";
            // 
            // cob_sound_mode
            // 
            this.cob_sound_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cob_sound_mode.FormattingEnabled = true;
            this.cob_sound_mode.Items.AddRange(new object[] {
            "Disabled",
            "All"});
            this.cob_sound_mode.Location = new System.Drawing.Point(6, 38);
            this.cob_sound_mode.Name = "cob_sound_mode";
            this.cob_sound_mode.Size = new System.Drawing.Size(121, 21);
            this.cob_sound_mode.TabIndex = 0;
            this.cob_sound_mode.SelectedIndexChanged += new System.EventHandler(this.cob_sound_mode_SelectedIndexChanged);
            // 
            // bt_settings_cancel
            // 
            this.bt_settings_cancel.Location = new System.Drawing.Point(455, 156);
            this.bt_settings_cancel.Name = "bt_settings_cancel";
            this.bt_settings_cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_settings_cancel.TabIndex = 7;
            this.bt_settings_cancel.Text = "cancel";
            this.bt_settings_cancel.UseVisualStyleBackColor = true;
            this.bt_settings_cancel.Click += new System.EventHandler(this.bt_settings_cancel_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(623, 191);
            this.Controls.Add(this.bt_settings_cancel);
            this.Controls.Add(this.gb_Audio);
            this.Controls.Add(this.gb_Appearence);
            this.Controls.Add(this.btn_save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CornFeed - Settings";
            ((System.ComponentModel.ISupportInitialize)(this.num_fontSize)).EndInit();
            this.gb_Appearence.ResumeLayout(false);
            this.gb_Appearence.PerformLayout();
            this.gb_Audio.ResumeLayout(false);
            this.gb_Audio.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown num_fontSize;
        private System.Windows.Forms.Button btn_background_color;
        private System.Windows.Forms.Button btn_save;
        private ColorDialog colorDialog1;
        public ComboBox cob_font;
        private Button btn_font_color;
        private GroupBox gb_Appearence;
        private Label lb_settings_font;
        private Label lb_settings_color;
        private GroupBox gb_Audio;
        private Label lb_settings_sound_mode;
        private Button bt_settings_select_wav_file;
        private Label lb_settings_WAV_path;
        public ComboBox cob_sound_mode;
        public TextBox tb_settings_wav_file;
        private Button bt_settings_cancel;
    }
}