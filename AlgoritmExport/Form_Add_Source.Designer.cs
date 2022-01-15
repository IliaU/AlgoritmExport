namespace AlgoritmExport
{
    partial class Form_Add_Source
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
            this.lbl_TNS = new System.Windows.Forms.Label();
            this.lbl_User = new System.Windows.Forms.Label();
            this.lbl_Password = new System.Windows.Forms.Label();
            this.txtBox_TNS = new System.Windows.Forms.TextBox();
            this.txtBox_User = new System.Windows.Forms.TextBox();
            this.txtBox_Password = new System.Windows.Forms.TextBox();
            this.btn_TestConect = new System.Windows.Forms.Button();
            this.pnl_Ora = new System.Windows.Forms.Panel();
            this.pnl_Top = new System.Windows.Forms.Panel();
            this.cmbBox_lbl_Prov_Typ = new System.Windows.Forms.ComboBox();
            this.lbl_Prov_Typ = new System.Windows.Forms.Label();
            this.pnl_MSSQL = new System.Windows.Forms.Panel();
            this.lbl_MSSQL_DB = new System.Windows.Forms.Label();
            this.cmbBox_MSSQL_DB = new System.Windows.Forms.ComboBox();
            this.chkBox_MSSQL_Integr = new System.Windows.Forms.CheckBox();
            this.lbl_MSSQL_user = new System.Windows.Forms.Label();
            this.lbl_MSSQL_Server = new System.Windows.Forms.Label();
            this.txtBox_MSSQL_password = new System.Windows.Forms.TextBox();
            this.lbl_MSSQL_Password = new System.Windows.Forms.Label();
            this.txtBox_MSSQL_user = new System.Windows.Forms.TextBox();
            this.txtBox_MSSQL_Server = new System.Windows.Forms.TextBox();
            this.pnl_Bottom = new System.Windows.Forms.Panel();
            this.pnl_RPro8 = new System.Windows.Forms.Panel();
            this.lbl_Rpro8_PatchDB = new System.Windows.Forms.Label();
            this.btn_Rpro8_SelectFolder = new System.Windows.Forms.Button();
            this.lbl_Rpro8_Patch = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.pnl_ODBC = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBox_ODBC_Password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBox_ODBC_User = new System.Windows.Forms.TextBox();
            this.txtBox_ODBC = new System.Windows.Forms.TextBox();
            this.pnl_Ora.SuspendLayout();
            this.pnl_Top.SuspendLayout();
            this.pnl_MSSQL.SuspendLayout();
            this.pnl_Bottom.SuspendLayout();
            this.pnl_RPro8.SuspendLayout();
            this.pnl_ODBC.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_TNS
            // 
            this.lbl_TNS.AutoSize = true;
            this.lbl_TNS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_TNS.ForeColor = System.Drawing.Color.Navy;
            this.lbl_TNS.Location = new System.Drawing.Point(12, 14);
            this.lbl_TNS.Name = "lbl_TNS";
            this.lbl_TNS.Size = new System.Drawing.Size(40, 20);
            this.lbl_TNS.TabIndex = 0;
            this.lbl_TNS.Text = "TNS";
            // 
            // lbl_User
            // 
            this.lbl_User.AutoSize = true;
            this.lbl_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_User.ForeColor = System.Drawing.Color.Navy;
            this.lbl_User.Location = new System.Drawing.Point(12, 46);
            this.lbl_User.Name = "lbl_User";
            this.lbl_User.Size = new System.Drawing.Size(121, 20);
            this.lbl_User.TabIndex = 1;
            this.lbl_User.Text = "Пользователь";
            // 
            // lbl_Password
            // 
            this.lbl_Password.AutoSize = true;
            this.lbl_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Password.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Password.Location = new System.Drawing.Point(12, 78);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(67, 20);
            this.lbl_Password.TabIndex = 2;
            this.lbl_Password.Text = "Пароль";
            // 
            // txtBox_TNS
            // 
            this.txtBox_TNS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_TNS.Location = new System.Drawing.Point(168, 11);
            this.txtBox_TNS.Name = "txtBox_TNS";
            this.txtBox_TNS.Size = new System.Drawing.Size(182, 26);
            this.txtBox_TNS.TabIndex = 10;
            // 
            // txtBox_User
            // 
            this.txtBox_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_User.Location = new System.Drawing.Point(168, 43);
            this.txtBox_User.Name = "txtBox_User";
            this.txtBox_User.Size = new System.Drawing.Size(182, 26);
            this.txtBox_User.TabIndex = 11;
            // 
            // txtBox_Password
            // 
            this.txtBox_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_Password.Location = new System.Drawing.Point(168, 75);
            this.txtBox_Password.Name = "txtBox_Password";
            this.txtBox_Password.Size = new System.Drawing.Size(182, 26);
            this.txtBox_Password.TabIndex = 12;
            this.txtBox_Password.UseSystemPasswordChar = true;
            // 
            // btn_TestConect
            // 
            this.btn_TestConect.Location = new System.Drawing.Point(12, 9);
            this.btn_TestConect.Name = "btn_TestConect";
            this.btn_TestConect.Size = new System.Drawing.Size(164, 23);
            this.btn_TestConect.TabIndex = 26;
            this.btn_TestConect.Text = "Проверка подключения";
            this.btn_TestConect.UseVisualStyleBackColor = true;
            this.btn_TestConect.Click += new System.EventHandler(this.btn_TestConect_Click);
            // 
            // pnl_Ora
            // 
            this.pnl_Ora.Controls.Add(this.lbl_User);
            this.pnl_Ora.Controls.Add(this.lbl_TNS);
            this.pnl_Ora.Controls.Add(this.txtBox_Password);
            this.pnl_Ora.Controls.Add(this.lbl_Password);
            this.pnl_Ora.Controls.Add(this.txtBox_User);
            this.pnl_Ora.Controls.Add(this.txtBox_TNS);
            this.pnl_Ora.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Ora.Location = new System.Drawing.Point(0, 197);
            this.pnl_Ora.Name = "pnl_Ora";
            this.pnl_Ora.Size = new System.Drawing.Size(398, 117);
            this.pnl_Ora.TabIndex = 9;
            this.pnl_Ora.Visible = false;
            // 
            // pnl_Top
            // 
            this.pnl_Top.Controls.Add(this.cmbBox_lbl_Prov_Typ);
            this.pnl_Top.Controls.Add(this.lbl_Prov_Typ);
            this.pnl_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Top.Location = new System.Drawing.Point(0, 0);
            this.pnl_Top.Name = "pnl_Top";
            this.pnl_Top.Size = new System.Drawing.Size(398, 39);
            this.pnl_Top.TabIndex = 1;
            // 
            // cmbBox_lbl_Prov_Typ
            // 
            this.cmbBox_lbl_Prov_Typ.FormattingEnabled = true;
            this.cmbBox_lbl_Prov_Typ.Location = new System.Drawing.Point(168, 7);
            this.cmbBox_lbl_Prov_Typ.Name = "cmbBox_lbl_Prov_Typ";
            this.cmbBox_lbl_Prov_Typ.Size = new System.Drawing.Size(182, 21);
            this.cmbBox_lbl_Prov_Typ.TabIndex = 2;
            this.cmbBox_lbl_Prov_Typ.SelectedIndexChanged += new System.EventHandler(this.cmbBox_lbl_Prov_Typ_SelectedIndexChanged);
            // 
            // lbl_Prov_Typ
            // 
            this.lbl_Prov_Typ.AutoSize = true;
            this.lbl_Prov_Typ.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Prov_Typ.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Prov_Typ.Location = new System.Drawing.Point(12, 5);
            this.lbl_Prov_Typ.Name = "lbl_Prov_Typ";
            this.lbl_Prov_Typ.Size = new System.Drawing.Size(132, 20);
            this.lbl_Prov_Typ.TabIndex = 1;
            this.lbl_Prov_Typ.Text = "Тип провайдера";
            // 
            // pnl_MSSQL
            // 
            this.pnl_MSSQL.Controls.Add(this.lbl_MSSQL_DB);
            this.pnl_MSSQL.Controls.Add(this.cmbBox_MSSQL_DB);
            this.pnl_MSSQL.Controls.Add(this.chkBox_MSSQL_Integr);
            this.pnl_MSSQL.Controls.Add(this.lbl_MSSQL_user);
            this.pnl_MSSQL.Controls.Add(this.lbl_MSSQL_Server);
            this.pnl_MSSQL.Controls.Add(this.txtBox_MSSQL_password);
            this.pnl_MSSQL.Controls.Add(this.lbl_MSSQL_Password);
            this.pnl_MSSQL.Controls.Add(this.txtBox_MSSQL_user);
            this.pnl_MSSQL.Controls.Add(this.txtBox_MSSQL_Server);
            this.pnl_MSSQL.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_MSSQL.Location = new System.Drawing.Point(0, 39);
            this.pnl_MSSQL.Name = "pnl_MSSQL";
            this.pnl_MSSQL.Size = new System.Drawing.Size(398, 158);
            this.pnl_MSSQL.TabIndex = 3;
            this.pnl_MSSQL.Visible = false;
            // 
            // lbl_MSSQL_DB
            // 
            this.lbl_MSSQL_DB.AutoSize = true;
            this.lbl_MSSQL_DB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_MSSQL_DB.ForeColor = System.Drawing.Color.Navy;
            this.lbl_MSSQL_DB.Location = new System.Drawing.Point(12, 128);
            this.lbl_MSSQL_DB.Name = "lbl_MSSQL_DB";
            this.lbl_MSSQL_DB.Size = new System.Drawing.Size(46, 20);
            this.lbl_MSSQL_DB.TabIndex = 14;
            this.lbl_MSSQL_DB.Text = "База";
            this.lbl_MSSQL_DB.Visible = false;
            // 
            // cmbBox_MSSQL_DB
            // 
            this.cmbBox_MSSQL_DB.FormattingEnabled = true;
            this.cmbBox_MSSQL_DB.Location = new System.Drawing.Point(168, 127);
            this.cmbBox_MSSQL_DB.Name = "cmbBox_MSSQL_DB";
            this.cmbBox_MSSQL_DB.Size = new System.Drawing.Size(182, 21);
            this.cmbBox_MSSQL_DB.TabIndex = 8;
            this.cmbBox_MSSQL_DB.MouseEnter += new System.EventHandler(this.cmbBox_MSSQL_DB_MouseEnter);
            // 
            // chkBox_MSSQL_Integr
            // 
            this.chkBox_MSSQL_Integr.AutoSize = true;
            this.chkBox_MSSQL_Integr.Location = new System.Drawing.Point(168, 42);
            this.chkBox_MSSQL_Integr.Name = "chkBox_MSSQL_Integr";
            this.chkBox_MSSQL_Integr.Size = new System.Drawing.Size(211, 17);
            this.chkBox_MSSQL_Integr.TabIndex = 5;
            this.chkBox_MSSQL_Integr.Text = "Встроенная проверка пользователя";
            this.chkBox_MSSQL_Integr.UseVisualStyleBackColor = true;
            this.chkBox_MSSQL_Integr.CheckedChanged += new System.EventHandler(this.chkBox_MSSQL_Integr_CheckedChanged);
            // 
            // lbl_MSSQL_user
            // 
            this.lbl_MSSQL_user.AutoSize = true;
            this.lbl_MSSQL_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_MSSQL_user.ForeColor = System.Drawing.Color.Navy;
            this.lbl_MSSQL_user.Location = new System.Drawing.Point(12, 66);
            this.lbl_MSSQL_user.Name = "lbl_MSSQL_user";
            this.lbl_MSSQL_user.Size = new System.Drawing.Size(121, 20);
            this.lbl_MSSQL_user.TabIndex = 7;
            this.lbl_MSSQL_user.Text = "Пользователь";
            this.lbl_MSSQL_user.Visible = false;
            // 
            // lbl_MSSQL_Server
            // 
            this.lbl_MSSQL_Server.AutoSize = true;
            this.lbl_MSSQL_Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_MSSQL_Server.ForeColor = System.Drawing.Color.Navy;
            this.lbl_MSSQL_Server.Location = new System.Drawing.Point(12, 13);
            this.lbl_MSSQL_Server.Name = "lbl_MSSQL_Server";
            this.lbl_MSSQL_Server.Size = new System.Drawing.Size(65, 20);
            this.lbl_MSSQL_Server.TabIndex = 6;
            this.lbl_MSSQL_Server.Text = "Сервер";
            // 
            // txtBox_MSSQL_password
            // 
            this.txtBox_MSSQL_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_MSSQL_password.Location = new System.Drawing.Point(168, 95);
            this.txtBox_MSSQL_password.Name = "txtBox_MSSQL_password";
            this.txtBox_MSSQL_password.Size = new System.Drawing.Size(182, 26);
            this.txtBox_MSSQL_password.TabIndex = 7;
            this.txtBox_MSSQL_password.UseSystemPasswordChar = true;
            this.txtBox_MSSQL_password.Visible = false;
            this.txtBox_MSSQL_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_MSSQL_password_KeyDown);
            // 
            // lbl_MSSQL_Password
            // 
            this.lbl_MSSQL_Password.AutoSize = true;
            this.lbl_MSSQL_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_MSSQL_Password.ForeColor = System.Drawing.Color.Navy;
            this.lbl_MSSQL_Password.Location = new System.Drawing.Point(12, 98);
            this.lbl_MSSQL_Password.Name = "lbl_MSSQL_Password";
            this.lbl_MSSQL_Password.Size = new System.Drawing.Size(67, 20);
            this.lbl_MSSQL_Password.TabIndex = 8;
            this.lbl_MSSQL_Password.Text = "Пароль";
            this.lbl_MSSQL_Password.Visible = false;
            // 
            // txtBox_MSSQL_user
            // 
            this.txtBox_MSSQL_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_MSSQL_user.Location = new System.Drawing.Point(168, 63);
            this.txtBox_MSSQL_user.Name = "txtBox_MSSQL_user";
            this.txtBox_MSSQL_user.Size = new System.Drawing.Size(182, 26);
            this.txtBox_MSSQL_user.TabIndex = 6;
            this.txtBox_MSSQL_user.Visible = false;
            this.txtBox_MSSQL_user.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_MSSQL_user_KeyDown);
            // 
            // txtBox_MSSQL_Server
            // 
            this.txtBox_MSSQL_Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_MSSQL_Server.Location = new System.Drawing.Point(168, 10);
            this.txtBox_MSSQL_Server.Name = "txtBox_MSSQL_Server";
            this.txtBox_MSSQL_Server.Size = new System.Drawing.Size(182, 26);
            this.txtBox_MSSQL_Server.TabIndex = 4;
            this.txtBox_MSSQL_Server.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_MSSQL_Server_KeyDown);
            // 
            // pnl_Bottom
            // 
            this.pnl_Bottom.Controls.Add(this.btn_TestConect);
            this.pnl_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_Bottom.Location = new System.Drawing.Point(0, 520);
            this.pnl_Bottom.Name = "pnl_Bottom";
            this.pnl_Bottom.Size = new System.Drawing.Size(398, 44);
            this.pnl_Bottom.TabIndex = 25;
            this.pnl_Bottom.Visible = false;
            // 
            // pnl_RPro8
            // 
            this.pnl_RPro8.Controls.Add(this.lbl_Rpro8_PatchDB);
            this.pnl_RPro8.Controls.Add(this.btn_Rpro8_SelectFolder);
            this.pnl_RPro8.Controls.Add(this.lbl_Rpro8_Patch);
            this.pnl_RPro8.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_RPro8.Location = new System.Drawing.Point(0, 421);
            this.pnl_RPro8.Name = "pnl_RPro8";
            this.pnl_RPro8.Size = new System.Drawing.Size(398, 74);
            this.pnl_RPro8.TabIndex = 17;
            this.pnl_RPro8.Visible = false;
            // 
            // lbl_Rpro8_PatchDB
            // 
            this.lbl_Rpro8_PatchDB.AutoSize = true;
            this.lbl_Rpro8_PatchDB.Location = new System.Drawing.Point(13, 42);
            this.lbl_Rpro8_PatchDB.Name = "lbl_Rpro8_PatchDB";
            this.lbl_Rpro8_PatchDB.Size = new System.Drawing.Size(0, 13);
            this.lbl_Rpro8_PatchDB.TabIndex = 9;
            // 
            // btn_Rpro8_SelectFolder
            // 
            this.btn_Rpro8_SelectFolder.ForeColor = System.Drawing.Color.Navy;
            this.btn_Rpro8_SelectFolder.Location = new System.Drawing.Point(275, 12);
            this.btn_Rpro8_SelectFolder.Name = "btn_Rpro8_SelectFolder";
            this.btn_Rpro8_SelectFolder.Size = new System.Drawing.Size(104, 23);
            this.btn_Rpro8_SelectFolder.TabIndex = 18;
            this.btn_Rpro8_SelectFolder.Text = "Выбрать папку";
            this.btn_Rpro8_SelectFolder.UseVisualStyleBackColor = true;
            this.btn_Rpro8_SelectFolder.Click += new System.EventHandler(this.btn_Rpro8_SelectFolder_Click);
            // 
            // lbl_Rpro8_Patch
            // 
            this.lbl_Rpro8_Patch.AutoSize = true;
            this.lbl_Rpro8_Patch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Rpro8_Patch.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Rpro8_Patch.Location = new System.Drawing.Point(12, 12);
            this.lbl_Rpro8_Patch.Name = "lbl_Rpro8_Patch";
            this.lbl_Rpro8_Patch.Size = new System.Drawing.Size(155, 20);
            this.lbl_Rpro8_Patch.TabIndex = 7;
            this.lbl_Rpro8_Patch.Text = "Путь к папке RPro8";
            // 
            // pnl_ODBC
            // 
            this.pnl_ODBC.Controls.Add(this.label1);
            this.pnl_ODBC.Controls.Add(this.label2);
            this.pnl_ODBC.Controls.Add(this.txtBox_ODBC_Password);
            this.pnl_ODBC.Controls.Add(this.label3);
            this.pnl_ODBC.Controls.Add(this.txtBox_ODBC_User);
            this.pnl_ODBC.Controls.Add(this.txtBox_ODBC);
            this.pnl_ODBC.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_ODBC.Location = new System.Drawing.Point(0, 314);
            this.pnl_ODBC.Name = "pnl_ODBC";
            this.pnl_ODBC.Size = new System.Drawing.Size(398, 107);
            this.pnl_ODBC.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Пользователь";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "DSN";
            // 
            // txtBox_ODBC_Password
            // 
            this.txtBox_ODBC_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_ODBC_Password.Location = new System.Drawing.Point(168, 70);
            this.txtBox_ODBC_Password.Name = "txtBox_ODBC_Password";
            this.txtBox_ODBC_Password.Size = new System.Drawing.Size(182, 26);
            this.txtBox_ODBC_Password.TabIndex = 16;
            this.txtBox_ODBC_Password.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Пароль";
            // 
            // txtBox_ODBC_User
            // 
            this.txtBox_ODBC_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_ODBC_User.Location = new System.Drawing.Point(168, 38);
            this.txtBox_ODBC_User.Name = "txtBox_ODBC_User";
            this.txtBox_ODBC_User.Size = new System.Drawing.Size(182, 26);
            this.txtBox_ODBC_User.TabIndex = 15;
            // 
            // txtBox_ODBC
            // 
            this.txtBox_ODBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_ODBC.Location = new System.Drawing.Point(168, 6);
            this.txtBox_ODBC.Name = "txtBox_ODBC";
            this.txtBox_ODBC.Size = new System.Drawing.Size(182, 26);
            this.txtBox_ODBC.TabIndex = 14;
            // 
            // Form_Add_Source
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 564);
            this.Controls.Add(this.pnl_RPro8);
            this.Controls.Add(this.pnl_ODBC);
            this.Controls.Add(this.pnl_Ora);
            this.Controls.Add(this.pnl_Bottom);
            this.Controls.Add(this.pnl_MSSQL);
            this.Controls.Add(this.pnl_Top);
            this.Name = "Form_Add_Source";
            this.Text = "Изменение подключения к базе данных";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Add_Source_FormClosing);
            this.Load += new System.EventHandler(this.Form_Add_Source_Load);
            this.pnl_Ora.ResumeLayout(false);
            this.pnl_Ora.PerformLayout();
            this.pnl_Top.ResumeLayout(false);
            this.pnl_Top.PerformLayout();
            this.pnl_MSSQL.ResumeLayout(false);
            this.pnl_MSSQL.PerformLayout();
            this.pnl_Bottom.ResumeLayout(false);
            this.pnl_RPro8.ResumeLayout(false);
            this.pnl_RPro8.PerformLayout();
            this.pnl_ODBC.ResumeLayout(false);
            this.pnl_ODBC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_TNS;
        private System.Windows.Forms.Label lbl_User;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.TextBox txtBox_TNS;
        private System.Windows.Forms.TextBox txtBox_User;
        private System.Windows.Forms.TextBox txtBox_Password;
        private System.Windows.Forms.Button btn_TestConect;
        private System.Windows.Forms.Panel pnl_Ora;
        private System.Windows.Forms.Panel pnl_Top;
        private System.Windows.Forms.ComboBox cmbBox_lbl_Prov_Typ;
        private System.Windows.Forms.Label lbl_Prov_Typ;
        private System.Windows.Forms.Panel pnl_MSSQL;
        private System.Windows.Forms.Panel pnl_Bottom;
        private System.Windows.Forms.CheckBox chkBox_MSSQL_Integr;
        private System.Windows.Forms.Label lbl_MSSQL_user;
        private System.Windows.Forms.Label lbl_MSSQL_Server;
        private System.Windows.Forms.TextBox txtBox_MSSQL_password;
        private System.Windows.Forms.Label lbl_MSSQL_Password;
        private System.Windows.Forms.TextBox txtBox_MSSQL_user;
        private System.Windows.Forms.TextBox txtBox_MSSQL_Server;
        private System.Windows.Forms.Label lbl_MSSQL_DB;
        private System.Windows.Forms.ComboBox cmbBox_MSSQL_DB;
        private System.Windows.Forms.Panel pnl_RPro8;
        private System.Windows.Forms.Button btn_Rpro8_SelectFolder;
        private System.Windows.Forms.Label lbl_Rpro8_Patch;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label lbl_Rpro8_PatchDB;
        private System.Windows.Forms.Panel pnl_ODBC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBox_ODBC_Password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBox_ODBC_User;
        private System.Windows.Forms.TextBox txtBox_ODBC;
    }
}