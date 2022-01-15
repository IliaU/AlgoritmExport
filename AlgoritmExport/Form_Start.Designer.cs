namespace AlgoritmExport
{
    partial class Form_Start
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.TSMI_Config = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_ConnectFromOracle = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Info = new System.Windows.Forms.ToolStripMenuItem();
            this.лицензияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.TS_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TS_StatusThreadLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel_CreateThread = new System.Windows.Forms.Panel();
            this.btn_SetParam = new System.Windows.Forms.Button();
            this.btn_CreateThread = new System.Windows.Forms.Button();
            this.comboBox_CreateThread = new System.Windows.Forms.ComboBox();
            this.lbl_CreateThread = new System.Windows.Forms.Label();
            this.panel_LogThread = new System.Windows.Forms.Panel();
            this.listBox_Log = new System.Windows.Forms.ListBox();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel_CreateThread.SuspendLayout();
            this.panel_LogThread.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Config,
            this.TSMI_Info,
            this.лицензияToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(950, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // TSMI_Config
            // 
            this.TSMI_Config.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_ConnectFromOracle});
            this.TSMI_Config.Name = "TSMI_Config";
            this.TSMI_Config.Size = new System.Drawing.Size(87, 21);
            this.TSMI_Config.Text = "Настройка";
            // 
            // TSMI_ConnectFromOracle
            // 
            this.TSMI_ConnectFromOracle.Name = "TSMI_ConnectFromOracle";
            this.TSMI_ConnectFromOracle.Size = new System.Drawing.Size(209, 22);
            this.TSMI_ConnectFromOracle.Text = "Подключение к базе";
            this.TSMI_ConnectFromOracle.Click += new System.EventHandler(this.TSMI_ConnectFromOracle_Click);
            // 
            // TSMI_Info
            // 
            this.TSMI_Info.Name = "TSMI_Info";
            this.TSMI_Info.Size = new System.Drawing.Size(73, 21);
            this.TSMI_Info.Text = "Справка";
            this.TSMI_Info.Click += new System.EventHandler(this.TSMI_Info_Click);
            // 
            // лицензияToolStripMenuItem
            // 
            this.лицензияToolStripMenuItem.Name = "лицензияToolStripMenuItem";
            this.лицензияToolStripMenuItem.Size = new System.Drawing.Size(81, 21);
            this.лицензияToolStripMenuItem.Text = "Лицензия";
            this.лицензияToolStripMenuItem.Click += new System.EventHandler(this.лицензияToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TS_StatusLabel,
            this.TS_StatusThreadLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 400);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(950, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // TS_StatusLabel
            // 
            this.TS_StatusLabel.Name = "TS_StatusLabel";
            this.TS_StatusLabel.Size = new System.Drawing.Size(85, 17);
            this.TS_StatusLabel.Text = "TS_StatusLabel";
            // 
            // TS_StatusThreadLabel
            // 
            this.TS_StatusThreadLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.TS_StatusThreadLabel.Name = "TS_StatusThreadLabel";
            this.TS_StatusThreadLabel.Size = new System.Drawing.Size(22, 17);
            this.TS_StatusThreadLabel.Text = "     ";
            // 
            // panel_CreateThread
            // 
            this.panel_CreateThread.Controls.Add(this.btn_SetParam);
            this.panel_CreateThread.Controls.Add(this.btn_CreateThread);
            this.panel_CreateThread.Controls.Add(this.comboBox_CreateThread);
            this.panel_CreateThread.Controls.Add(this.lbl_CreateThread);
            this.panel_CreateThread.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_CreateThread.Location = new System.Drawing.Point(0, 25);
            this.panel_CreateThread.Name = "panel_CreateThread";
            this.panel_CreateThread.Size = new System.Drawing.Size(950, 52);
            this.panel_CreateThread.TabIndex = 2;
            // 
            // btn_SetParam
            // 
            this.btn_SetParam.ForeColor = System.Drawing.Color.Navy;
            this.btn_SetParam.Location = new System.Drawing.Point(571, 19);
            this.btn_SetParam.Name = "btn_SetParam";
            this.btn_SetParam.Size = new System.Drawing.Size(182, 26);
            this.btn_SetParam.TabIndex = 3;
            this.btn_SetParam.Text = "Установка параметров";
            this.btn_SetParam.UseVisualStyleBackColor = true;
            this.btn_SetParam.Visible = false;
            this.btn_SetParam.Click += new System.EventHandler(this.btn_SetParam_Click);
            // 
            // btn_CreateThread
            // 
            this.btn_CreateThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_CreateThread.ForeColor = System.Drawing.Color.Navy;
            this.btn_CreateThread.Location = new System.Drawing.Point(759, 19);
            this.btn_CreateThread.Name = "btn_CreateThread";
            this.btn_CreateThread.Size = new System.Drawing.Size(179, 26);
            this.btn_CreateThread.TabIndex = 2;
            this.btn_CreateThread.Text = "Запустить задание";
            this.btn_CreateThread.UseVisualStyleBackColor = true;
            this.btn_CreateThread.Click += new System.EventHandler(this.btn_CreateThread_Click);
            // 
            // comboBox_CreateThread
            // 
            this.comboBox_CreateThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox_CreateThread.FormattingEnabled = true;
            this.comboBox_CreateThread.Location = new System.Drawing.Point(7, 22);
            this.comboBox_CreateThread.Name = "comboBox_CreateThread";
            this.comboBox_CreateThread.Size = new System.Drawing.Size(558, 23);
            this.comboBox_CreateThread.TabIndex = 1;
            this.comboBox_CreateThread.SelectedIndexChanged += new System.EventHandler(this.comboBox_CreateThread_SelectedIndexChanged);
            // 
            // lbl_CreateThread
            // 
            this.lbl_CreateThread.AutoSize = true;
            this.lbl_CreateThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_CreateThread.ForeColor = System.Drawing.Color.Navy;
            this.lbl_CreateThread.Location = new System.Drawing.Point(4, 4);
            this.lbl_CreateThread.Name = "lbl_CreateThread";
            this.lbl_CreateThread.Size = new System.Drawing.Size(191, 15);
            this.lbl_CreateThread.TabIndex = 0;
            this.lbl_CreateThread.Text = "Выберите задание для запуска:";
            // 
            // panel_LogThread
            // 
            this.panel_LogThread.Controls.Add(this.listBox_Log);
            this.panel_LogThread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_LogThread.Location = new System.Drawing.Point(0, 77);
            this.panel_LogThread.Name = "panel_LogThread";
            this.panel_LogThread.Size = new System.Drawing.Size(950, 323);
            this.panel_LogThread.TabIndex = 3;
            this.panel_LogThread.Visible = false;
            // 
            // listBox_Log
            // 
            this.listBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Log.FormattingEnabled = true;
            this.listBox_Log.Location = new System.Drawing.Point(0, 0);
            this.listBox_Log.Name = "listBox_Log";
            this.listBox_Log.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBox_Log.Size = new System.Drawing.Size(950, 323);
            this.listBox_Log.TabIndex = 0;
            // 
            // Form_Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 422);
            this.Controls.Add(this.panel_LogThread);
            this.Controls.Add(this.panel_CreateThread);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form_Start";
            this.Text = "Algoritm Export";
            this.Load += new System.EventHandler(this.Form_Start_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel_CreateThread.ResumeLayout(false);
            this.panel_CreateThread.PerformLayout();
            this.panel_LogThread.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Config;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ConnectFromOracle;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel TS_StatusLabel;
        private System.Windows.Forms.Panel panel_CreateThread;
        private System.Windows.Forms.Label lbl_CreateThread;
        private System.Windows.Forms.Button btn_CreateThread;
        private System.Windows.Forms.ComboBox comboBox_CreateThread;
        private System.Windows.Forms.Panel panel_LogThread;
        private System.Windows.Forms.ListBox listBox_Log;
        private System.Windows.Forms.ToolStripStatusLabel TS_StatusThreadLabel;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Info;
        private System.Windows.Forms.Button btn_SetParam;
        private System.Windows.Forms.ToolStripMenuItem лицензияToolStripMenuItem;
    }
}

