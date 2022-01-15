namespace AlgoritmExport
{
    partial class Form_Param
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
            this.lBox_Param = new System.Windows.Forms.ListBox();
            this.lbl_Param = new System.Windows.Forms.Label();
            this.Calendar_Tek = new System.Windows.Forms.MonthCalendar();
            this.panel_value = new System.Windows.Forms.Panel();
            this.ComboBox_Tek = new System.Windows.Forms.ComboBox();
            this.lbl_Calendar_Tek = new System.Windows.Forms.Label();
            this.panel_ListBox = new System.Windows.Forms.Panel();
            this.checkBox_ForListBox = new System.Windows.Forms.CheckBox();
            this.listBox_ForListBox = new System.Windows.Forms.ListBox();
            this.panel_value.SuspendLayout();
            this.panel_ListBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lBox_Param
            // 
            this.lBox_Param.FormattingEnabled = true;
            this.lBox_Param.Location = new System.Drawing.Point(3, 24);
            this.lBox_Param.Name = "lBox_Param";
            this.lBox_Param.Size = new System.Drawing.Size(234, 433);
            this.lBox_Param.TabIndex = 0;
            this.lBox_Param.SelectedIndexChanged += new System.EventHandler(this.lBox_Param_SelectedIndexChanged);
            // 
            // lbl_Param
            // 
            this.lbl_Param.AutoSize = true;
            this.lbl_Param.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Param.Location = new System.Drawing.Point(3, 5);
            this.lbl_Param.Name = "lbl_Param";
            this.lbl_Param.Size = new System.Drawing.Size(58, 13);
            this.lbl_Param.TabIndex = 1;
            this.lbl_Param.Text = "Параметр";
            // 
            // Calendar_Tek
            // 
            this.Calendar_Tek.Location = new System.Drawing.Point(9, 9);
            this.Calendar_Tek.Name = "Calendar_Tek";
            this.Calendar_Tek.TabIndex = 2;
            this.Calendar_Tek.Visible = false;
            this.Calendar_Tek.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.Calendar_Tek_DateSelected);
            // 
            // panel_value
            // 
            this.panel_value.Controls.Add(this.panel_ListBox);
            this.panel_value.Controls.Add(this.ComboBox_Tek);
            this.panel_value.Controls.Add(this.lbl_Calendar_Tek);
            this.panel_value.Controls.Add(this.Calendar_Tek);
            this.panel_value.Location = new System.Drawing.Point(243, 24);
            this.panel_value.Name = "panel_value";
            this.panel_value.Size = new System.Drawing.Size(213, 433);
            this.panel_value.TabIndex = 3;
            // 
            // ComboBox_Tek
            // 
            this.ComboBox_Tek.FormattingEnabled = true;
            this.ComboBox_Tek.Location = new System.Drawing.Point(9, 10);
            this.ComboBox_Tek.MaxDropDownItems = 28;
            this.ComboBox_Tek.Name = "ComboBox_Tek";
            this.ComboBox_Tek.Size = new System.Drawing.Size(200, 21);
            this.ComboBox_Tek.TabIndex = 4;
            this.ComboBox_Tek.Visible = false;
            this.ComboBox_Tek.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Tek_SelectedIndexChanged);
            // 
            // lbl_Calendar_Tek
            // 
            this.lbl_Calendar_Tek.AutoSize = true;
            this.lbl_Calendar_Tek.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Calendar_Tek.Location = new System.Drawing.Point(6, 180);
            this.lbl_Calendar_Tek.Name = "lbl_Calendar_Tek";
            this.lbl_Calendar_Tek.Size = new System.Drawing.Size(90, 13);
            this.lbl_Calendar_Tek.TabIndex = 3;
            this.lbl_Calendar_Tek.Text = "Выбранная дата";
            this.lbl_Calendar_Tek.Visible = false;
            // 
            // panel_ListBox
            // 
            this.panel_ListBox.Controls.Add(this.listBox_ForListBox);
            this.panel_ListBox.Controls.Add(this.checkBox_ForListBox);
            this.panel_ListBox.Location = new System.Drawing.Point(9, 206);
            this.panel_ListBox.Name = "panel_ListBox";
            this.panel_ListBox.Size = new System.Drawing.Size(175, 155);
            this.panel_ListBox.TabIndex = 7;
            // 
            // checkBox_ForListBox
            // 
            this.checkBox_ForListBox.AutoSize = true;
            this.checkBox_ForListBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBox_ForListBox.Location = new System.Drawing.Point(0, 138);
            this.checkBox_ForListBox.Name = "checkBox_ForListBox";
            this.checkBox_ForListBox.Size = new System.Drawing.Size(175, 17);
            this.checkBox_ForListBox.TabIndex = 7;
            this.checkBox_ForListBox.Text = "Выбрать всё.";
            this.checkBox_ForListBox.UseVisualStyleBackColor = true;
            this.checkBox_ForListBox.Click += new System.EventHandler(this.checkBox_ForListBox_Click);
            // 
            // listBox_ForListBox
            // 
            this.listBox_ForListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_ForListBox.FormattingEnabled = true;
            this.listBox_ForListBox.Location = new System.Drawing.Point(0, 0);
            this.listBox_ForListBox.Name = "listBox_ForListBox";
            this.listBox_ForListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_ForListBox.Size = new System.Drawing.Size(175, 138);
            this.listBox_ForListBox.TabIndex = 8;
            this.listBox_ForListBox.Click += new System.EventHandler(this.listBox_ForListBox_Click);
            // 
            // Form_Param
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 461);
            this.Controls.Add(this.panel_value);
            this.Controls.Add(this.lbl_Param);
            this.Controls.Add(this.lBox_Param);
            this.Name = "Form_Param";
            this.Text = "Установка параметров";
            this.Load += new System.EventHandler(this.Form_Param_Load);
            this.panel_value.ResumeLayout(false);
            this.panel_value.PerformLayout();
            this.panel_ListBox.ResumeLayout(false);
            this.panel_ListBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lBox_Param;
        private System.Windows.Forms.Label lbl_Param;
        private System.Windows.Forms.MonthCalendar Calendar_Tek;
        private System.Windows.Forms.Panel panel_value;
        private System.Windows.Forms.Label lbl_Calendar_Tek;
        private System.Windows.Forms.ComboBox ComboBox_Tek;
        private System.Windows.Forms.Panel panel_ListBox;
        private System.Windows.Forms.ListBox listBox_ForListBox;
        private System.Windows.Forms.CheckBox checkBox_ForListBox;
    }
}