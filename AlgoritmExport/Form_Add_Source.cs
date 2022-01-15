using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlgoritmExport
{
    public partial class Form_Add_Source : Form
    {
        private Common.Com MyCom;

        public Lib.ProviderI Provider;
        //public string NewConnectionString;
        
        // Конструктор
        public Form_Add_Source(Common.Com MyCom)
        {
            this.MyCom = MyCom;
            InitializeComponent();

            this.cmbBox_lbl_Prov_Typ.Items.Clear();
            foreach (Lib.Provider_En item in Enum.GetValues(typeof(Lib.Provider_En)))
            {
                if (item != Lib.Provider_En.Empty) this.cmbBox_lbl_Prov_Typ.Items.Add(item);
            }

        }

        // Чтение формы
        private void Form_Add_Source_Load(object sender, EventArgs e)
        {
            this.pnl_Ora.Visible = false;
            this.pnl_ODBC.Visible = false;
            this.pnl_MSSQL.Visible = false;
            this.pnl_RPro8.Visible = false;
            this.pnl_Bottom.Visible = true;

            switch ((e != null ? this.MyCom.Provider.GetTyp() : Lib.EnConverter.Convert(this.cmbBox_lbl_Prov_Typ.SelectedItem.ToString(), Lib.Provider_En.Empty)))
            {
                case AlgoritmExport.Lib.Provider_En.Oracle:
                        this.Provider = new Common.Com.Com_Provider_Ora(this.MyCom);
                        this.pnl_Ora.Visible = true;
                        this.pnl_Ora.Dock = DockStyle.Fill;
                        if (e != null)
                        {
                            this.cmbBox_lbl_Prov_Typ.SelectedIndex = (int)this.MyCom.Provider.GetTyp() - 1;
                            this.txtBox_TNS.Text = ((Common.Com.Com_Provider_Ora)this.MyCom.Provider).ORA_TNS;
                            this.txtBox_User.Text = ((Common.Com.Com_Provider_Ora)this.MyCom.Provider).ORA_User;
                            this.txtBox_Password.Text = ((Common.Com.Com_Provider_Ora)this.MyCom.Provider).ORA_Password;
                        }
                    break;
                case AlgoritmExport.Lib.Provider_En.ODBC:
                    this.Provider = new Common.Com.Com_Provider_ODBC(this.MyCom);
                    this.pnl_ODBC.Visible = true;
                    this.pnl_ODBC.Dock = DockStyle.Fill;
                    if (e != null)
                    {
                        this.cmbBox_lbl_Prov_Typ.SelectedIndex = (int)this.MyCom.Provider.GetTyp() - 1;
                        this.txtBox_ODBC.Text = ((Common.Com.Com_Provider_ODBC)this.MyCom.Provider).ODBC_DSN;
                        this.txtBox_ODBC_User.Text = ((Common.Com.Com_Provider_ODBC)this.MyCom.Provider).ODBC_User;
                        this.txtBox_ODBC_Password.Text = ((Common.Com.Com_Provider_ODBC)this.MyCom.Provider).ODBC_Password;
                    }
                    break;
                case AlgoritmExport.Lib.Provider_En.MSSQL:
                        this.Provider = new Common.Com.Com_Provider_MSSQL(this.MyCom);
                        this.pnl_MSSQL.Visible = true;
                        this.pnl_MSSQL.Dock = DockStyle.Fill;
                        // Если редактируем подключние
                        if (e != null)
                        {
                            this.cmbBox_lbl_Prov_Typ.SelectedIndex = (int)this.MyCom.Provider.GetTyp() - 1;
                            this.txtBox_MSSQL_Server.Text = ((Common.Com.Com_Provider_MSSQL)this.MyCom.Provider).server;
                            this.chkBox_MSSQL_Integr.Checked = ((Common.Com.Com_Provider_MSSQL)this.MyCom.Provider).integratedSecurity;
                            if (!this.chkBox_MSSQL_Integr.Checked)
                            {
                                this.txtBox_MSSQL_user.Text = ((Common.Com.Com_Provider_MSSQL)this.MyCom.Provider).user;
                                this.txtBox_MSSQL_password.Text = ((Common.Com.Com_Provider_MSSQL)this.MyCom.Provider).password;
                            }
                            if (this.cmbBox_MSSQL_DB.Items.Count == 0)
                            {
                                // строим список доступных баз данных
                                string tmp = ((Common.Com.Com_Provider_MSSQL)this.MyCom.Provider).database;
                                foreach (string item in ((Common.Com.Com_Provider_MSSQL)this.MyCom.Provider).GetDatabases(null))
                                {
                                    this.cmbBox_MSSQL_DB.Items.Add(item);
                                    if (tmp != null && tmp != string.Empty && item.ToUpper().Trim() == tmp.ToUpper().Trim()) this.cmbBox_MSSQL_DB.SelectedIndex = this.cmbBox_MSSQL_DB.Items.Count - 1;
                                }
                            }
                        }
                        if (this.chkBox_MSSQL_Integr.Checked)
                        {
                            this.lbl_MSSQL_user.Visible = false;
                            this.txtBox_MSSQL_user.Visible = false;
                            this.lbl_MSSQL_Password.Visible = false;
                            this.txtBox_MSSQL_password.Visible = false;
                        }
                        else
                        {
                            this.lbl_MSSQL_user.Visible = true;
                            this.txtBox_MSSQL_user.Visible = true;
                            this.lbl_MSSQL_Password.Visible = true;
                            this.txtBox_MSSQL_password.Visible = true;
                        }
                    break;
                case AlgoritmExport.Lib.Provider_En.RPro8:
                        this.Provider = new Common.Com.Com_Provider_RPro8(this.MyCom);
                        this.pnl_RPro8.Visible = true;
                        this.pnl_RPro8.Dock = DockStyle.Fill;
                        if (e != null)
                        {
                            this.cmbBox_lbl_Prov_Typ.SelectedIndex = (int)this.MyCom.Provider.GetTyp() - 1;
                            this.lbl_Rpro8_PatchDB.Text = ((Common.Com.Com_Provider_RPro8)this.MyCom.Provider).ConnectString();
                        }
                    break;
                default:
                        this.pnl_Bottom.Visible = false;
                    break;
            }

            
        }

        // Пользователь собирается выбрать базу данных
        private void cmbBox_MSSQL_DB_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                // если ещё не получили список баз данных
                if (this.cmbBox_MSSQL_DB.Items.Count == 0 && this.Provider.GetTyp()== Lib.Provider_En.MSSQL)
                {
                    // получаем список доступных баз данных
                    List<string> db = ((Common.Com.Com_Provider_MSSQL)this.Provider).GetDatabases(this.txtBox_MSSQL_Server.Text, this.chkBox_MSSQL_Integr.Checked, this.txtBox_MSSQL_user.Text, this.txtBox_MSSQL_password.Text);

                    foreach (string item in db)
                    {
                        this.cmbBox_MSSQL_DB.Items.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                this.cmbBox_MSSQL_DB.Items.Clear();
            }

        }

        // Пользователь решил проверить подключение
        private void btn_TestConect_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.Provider.GetTyp())
                {
                    case AlgoritmExport.Lib.Provider_En.Oracle:
                        ((Common.Com.Com_Provider_Ora)this.Provider).TestCon(this.txtBox_TNS.Text, this.txtBox_User.Text, this.txtBox_Password.Text);
                        break;
                    case AlgoritmExport.Lib.Provider_En.ODBC:
                        ((Common.Com.Com_Provider_ODBC)this.Provider).TestCon(this.txtBox_ODBC.Text, this.txtBox_ODBC_User.Text, this.txtBox_ODBC_Password.Text);
                        break;
                    case AlgoritmExport.Lib.Provider_En.MSSQL:
                        ((Common.Com.Com_Provider_MSSQL)this.Provider).TestCon(this.txtBox_MSSQL_Server.Text, this.chkBox_MSSQL_Integr.Checked, this.txtBox_MSSQL_user.Text, this.txtBox_MSSQL_password.Text);
                        break;
                    case AlgoritmExport.Lib.Provider_En.RPro8:
                        ((Common.Com.Com_Provider_RPro8)this.Provider).TestCon(this.lbl_Rpro8_PatchDB.Text);
                        break;
                    default:
                        return;
                }

                MessageBox.Show("Тест подключения прошёл успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Закрытие формы
        private void Form_Add_Source_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                switch (this.Provider.GetTyp())
                {
                    case AlgoritmExport.Lib.Provider_En.Oracle:
                        ((Common.Com.Com_Provider_Ora)this.Provider).SaveConnectStr(this.txtBox_TNS.Text, this.txtBox_User.Text, this.txtBox_Password.Text);
                        break;
                    case AlgoritmExport.Lib.Provider_En.ODBC:
                        ((Common.Com.Com_Provider_ODBC)this.Provider).SaveConnectStr(this.txtBox_ODBC.Text, this.txtBox_ODBC_User.Text, this.txtBox_ODBC_Password.Text);
                        break;
                    case AlgoritmExport.Lib.Provider_En.MSSQL:
                        ((Common.Com.Com_Provider_MSSQL)this.Provider).SaveConnectStr(this.txtBox_MSSQL_Server.Text, this.chkBox_MSSQL_Integr.Checked, this.txtBox_MSSQL_user.Text, this.txtBox_MSSQL_password.Text, (this.cmbBox_MSSQL_DB.SelectedIndex>-1?this.cmbBox_MSSQL_DB.Items[this.cmbBox_MSSQL_DB.SelectedIndex].ToString():null));
                        break;
                    case AlgoritmExport.Lib.Provider_En.RPro8:
                        ((Common.Com.Com_Provider_RPro8)this.Provider).SaveConnectStr(this.lbl_Rpro8_PatchDB.Text);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception) { this.Provider = null; MessageBox.Show("Строка подключения некорректна, изменения сохранены не будут."); }

            // Если тест прошёл успешно
            if (this.Provider != null && this.Provider.ConnectString() != this.MyCom.Provider.ConnectString())
            {
                DialogResult rez = MessageBox.Show("Сохранить новую строку подключения?", "Сохранение строки подключения", MessageBoxButtons.YesNo);
                if (rez == DialogResult.Yes) DialogResult = DialogResult.OK;
            }
        }

        // Пользователь меняет тип провайдера
        private void cmbBox_lbl_Prov_Typ_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form_Add_Source_Load(null, null);
        }

        // Пользователь выбирает интгрированную проверку подленности
        private void chkBox_MSSQL_Integr_CheckedChanged(object sender, EventArgs e)
        {
            Form_Add_Source_Load(null, null);
            this.cmbBox_MSSQL_DB.Items.Clear();
        }

        // Пользователь вводит сервер
        private void txtBox_MSSQL_Server_KeyDown(object sender, KeyEventArgs e)
        {
            this.cmbBox_MSSQL_DB.Items.Clear();
        }

        // Пользователь вводит пользователя
        private void txtBox_MSSQL_user_KeyDown(object sender, KeyEventArgs e)
        {
            this.cmbBox_MSSQL_DB.Items.Clear();
        }

        // Пользователь вводит пароль
        private void txtBox_MSSQL_password_KeyDown(object sender, KeyEventArgs e)
        {
            this.cmbBox_MSSQL_DB.Items.Clear();
        }

        // Пользователь выбирает папку RPro8
        private void btn_Rpro8_SelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult rez = this.folderBrowserDialog.ShowDialog();
            if (rez == DialogResult.OK)
            {
                //this.ConnectionString = this.folderBrowserDialog.SelectedPath;
                this.lbl_Rpro8_PatchDB.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

    }
}
