namespace CornFeed
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_exit = new System.Windows.Forms.Button();
            this.lb_game_log_path = new System.Windows.Forms.Label();
            this.tb_log_path = new System.Windows.Forms.TextBox();
            this.btn_select_file = new System.Windows.Forms.Button();
            this.rtb_feed = new System.Windows.Forms.RichTextBox();
            this.btn_status = new System.Windows.Forms.Button();
            this.lb_status = new System.Windows.Forms.Label();
            this.cb_show_old = new System.Windows.Forms.CheckBox();
            this.cb_show_npc_kills = new System.Windows.Forms.CheckBox();
            this.cb_show_full_npc_names = new System.Windows.Forms.CheckBox();
            this.bt_settings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_exit
            // 
            this.btn_exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_exit.Location = new System.Drawing.Point(494, 378);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 23);
            this.btn_exit.TabIndex = 0;
            this.btn_exit.Text = "exit";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.Btn_exit_click);
            // 
            // lb_game_log_path
            // 
            this.lb_game_log_path.AutoSize = true;
            this.lb_game_log_path.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_game_log_path.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lb_game_log_path.Location = new System.Drawing.Point(12, 9);
            this.lb_game_log_path.Name = "lb_game_log_path";
            this.lb_game_log_path.Size = new System.Drawing.Size(112, 16);
            this.lb_game_log_path.TabIndex = 1;
            this.lb_game_log_path.Text = "Game.log path:";
            // 
            // tb_log_path
            // 
            this.tb_log_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_log_path.Location = new System.Drawing.Point(15, 29);
            this.tb_log_path.Name = "tb_log_path";
            this.tb_log_path.ReadOnly = true;
            this.tb_log_path.Size = new System.Drawing.Size(469, 20);
            this.tb_log_path.TabIndex = 2;
            // 
            // btn_select_file
            // 
            this.btn_select_file.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_select_file.Location = new System.Drawing.Point(494, 27);
            this.btn_select_file.Name = "btn_select_file";
            this.btn_select_file.Size = new System.Drawing.Size(75, 23);
            this.btn_select_file.TabIndex = 3;
            this.btn_select_file.Text = "select";
            this.btn_select_file.UseVisualStyleBackColor = true;
            this.btn_select_file.Click += new System.EventHandler(this.btn_select_file_Click);
            // 
            // rtb_feed
            // 
            this.rtb_feed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_feed.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_feed.Location = new System.Drawing.Point(15, 83);
            this.rtb_feed.Name = "rtb_feed";
            this.rtb_feed.ReadOnly = true;
            this.rtb_feed.Size = new System.Drawing.Size(469, 318);
            this.rtb_feed.TabIndex = 4;
            this.rtb_feed.Text = "";
            // 
            // btn_status
            // 
            this.btn_status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_status.Location = new System.Drawing.Point(494, 83);
            this.btn_status.Name = "btn_status";
            this.btn_status.Size = new System.Drawing.Size(75, 23);
            this.btn_status.TabIndex = 5;
            this.btn_status.Text = "start";
            this.btn_status.UseVisualStyleBackColor = true;
            this.btn_status.Click += new System.EventHandler(this.btn_status_Click);
            // 
            // lb_status
            // 
            this.lb_status.AutoSize = true;
            this.lb_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lb_status.Location = new System.Drawing.Point(12, 63);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(81, 13);
            this.lb_status.TabIndex = 6;
            this.lb_status.Text = "Status: stopped";
            // 
            // cb_show_old
            // 
            this.cb_show_old.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_show_old.AutoSize = true;
            this.cb_show_old.Location = new System.Drawing.Point(494, 113);
            this.cb_show_old.Name = "cb_show_old";
            this.cb_show_old.Size = new System.Drawing.Size(76, 17);
            this.cb_show_old.TabIndex = 7;
            this.cb_show_old.Text = "Old entries";
            this.cb_show_old.UseVisualStyleBackColor = true;
            // 
            // cb_show_npc_kills
            // 
            this.cb_show_npc_kills.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_show_npc_kills.AutoSize = true;
            this.cb_show_npc_kills.Location = new System.Drawing.Point(494, 137);
            this.cb_show_npc_kills.Name = "cb_show_npc_kills";
            this.cb_show_npc_kills.Size = new System.Drawing.Size(74, 17);
            this.cb_show_npc_kills.TabIndex = 8;
            this.cb_show_npc_kills.Text = "Log NPCs";
            this.cb_show_npc_kills.UseVisualStyleBackColor = true;
            this.cb_show_npc_kills.CheckedChanged += new System.EventHandler(this.cb_show_npc_kills_Checked);
            // 
            // cb_show_full_npc_names
            // 
            this.cb_show_full_npc_names.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_show_full_npc_names.AutoSize = true;
            this.cb_show_full_npc_names.Location = new System.Drawing.Point(504, 160);
            this.cb_show_full_npc_names.Name = "cb_show_full_npc_names";
            this.cb_show_full_npc_names.Size = new System.Drawing.Size(76, 17);
            this.cb_show_full_npc_names.TabIndex = 9;
            this.cb_show_full_npc_names.Text = "Full names";
            this.cb_show_full_npc_names.UseVisualStyleBackColor = true;
            // 
            // bt_settings
            // 
            this.bt_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_settings.Location = new System.Drawing.Point(494, 349);
            this.bt_settings.Name = "bt_settings";
            this.bt_settings.Size = new System.Drawing.Size(75, 23);
            this.bt_settings.TabIndex = 10;
            this.bt_settings.Text = "settings";
            this.bt_settings.UseVisualStyleBackColor = true;
            this.bt_settings.Click += new System.EventHandler(this.bt_settings_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(578, 413);
            this.Controls.Add(this.bt_settings);
            this.Controls.Add(this.cb_show_full_npc_names);
            this.Controls.Add(this.cb_show_npc_kills);
            this.Controls.Add(this.cb_show_old);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.btn_status);
            this.Controls.Add(this.rtb_feed);
            this.Controls.Add(this.btn_select_file);
            this.Controls.Add(this.tb_log_path);
            this.Controls.Add(this.lb_game_log_path);
            this.Controls.Add(this.btn_exit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CornFeed 1.1.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Label lb_game_log_path;
        private System.Windows.Forms.TextBox tb_log_path;
        private System.Windows.Forms.Button btn_select_file;
        private System.Windows.Forms.RichTextBox rtb_feed;
        private System.Windows.Forms.Button btn_status;
        private System.Windows.Forms.Label lb_status;
        private System.Windows.Forms.CheckBox cb_show_old;
        private System.Windows.Forms.CheckBox cb_show_npc_kills;
        private System.Windows.Forms.CheckBox cb_show_full_npc_names;
        private System.Windows.Forms.Button bt_settings;
    }
}

