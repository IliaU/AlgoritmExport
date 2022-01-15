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
    public partial class Form_Start : Form
    {
        private Common.Com MyCom;
        /// <summary>
        /// Имя задачи, которая стратует автоматически
        /// </summary>
        private string AutoStart;
        /// <summary>
        /// Статус отправки письма по умолчанию
        /// </summary>
        private Common.Com.MyEvent SendMailStat = Common.Com.MyEvent.Success;


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="MyCom">Основной объект</param>
        public Form_Start(Common.Com MyCom, string AutoStart)
        {
            this.MyCom = MyCom;
            this.AutoStart = AutoStart;
            InitializeComponent();
  
            this.MyCom.Provider.onSaveConnectStr += new EventHandler<Common.Com.onComProviderArg>(Provider_onSaveConnectStr);
            this.MyCom.Threader.onChengStatus += new EventHandler<Common.Com.onComThreadrArg>(Threader_onChengStatus);
            this.MyCom.Threader.onRowAffweted += new EventHandler<Common.Com.onComThreadrRowAffwetedArg>(Threader_onRowAffweted);
            this.MyCom.Threader.onEvenvSendMail += new EventHandler<Common.EvenvSmtpEmail>(Threader_onEvenvSendMail);
        }


        private void Form_Start_Load(object sender, EventArgs e)
        {
            if (this.MyCom.Provider != null && this.MyCom.Provider.ConnectString() != null)
            {
                this.TS_StatusLabel.ForeColor = Color.Green;
                this.TS_StatusLabel.Text = @"Подключение успешно установлено";
            }
            else
            {
                this.TS_StatusLabel.ForeColor = Color.Red;
                this.TS_StatusLabel.Text = @"Подключение к базе, не установлено";
            }

            // обновление списка возможных заданий
            LoadSprWorkfow();
            LoadPannelLog();

            // Проверка на точ, что есть задание для старта
            if (AutoStart != null)
            {
                try
                {
                    this.comboBox_CreateThread.SelectedIndex = this.MyCom.SprWorkflow[AutoStart].Index;
                    btn_CreateThread_Click(null, null);
                }
                catch (Exception ex) {MessageBox.Show(ex.Message);}
                AutoStart = null;
            }
        }

        /// <summary>
        /// Чтение списка возможных заданий
        /// </summary>
        private void LoadSprWorkfow()
        {
            // только если объект comboBox_CreateThread доступен пользователю
            if (this.panel_CreateThread.Visible)
            {
                this.comboBox_CreateThread.Items.Clear();

                foreach (Common.MyWorkflow item in this.MyCom.SprWorkflow)
	            {
                    this.comboBox_CreateThread.Items.Add(item.LongName);
	            }
                
            }
        }

        /// <summary>
        /// Чтение панели с логом выполнения задачи
        /// </summary>
        private void LoadPannelLog()
        {
            if (this.panel_LogThread.Visible && this.MyCom.Threader.CurrentWorkflow!=null)
            {
                string WorkName = this.MyCom.Threader.CurrentWorkflow.LongName;
                if (this.listBox_Log.Items.Count == 0) this.listBox_Log.Items.Add(WorkName);
                else this.listBox_Log.Items[0] = WorkName;

                string NewStatus = @"Статус: (" +  this.MyCom.Threader.Status + @")";
                if (this.listBox_Log.Items.Count >= 1)
                {
                    if (this.listBox_Log.Items.Count != 2) this.listBox_Log.Items.Add(NewStatus);
                    else this.listBox_Log.Items[1] = NewStatus;
                }
            }
        }

        // Произошло изменение статуса выполняемой задачи
        private delegate void MonStatusChange(object sender, Common.Com.onComThreadrArg e);
        void Threader_onChengStatus(object sender, Common.Com.onComThreadrArg e)
        {
            if (this.listBox_Log.InvokeRequired)
            {
                MonStatusChange tsDelegate = new MonStatusChange(Threader_onChengStatus);
                this.listBox_Log.Invoke(tsDelegate, new object[] { sender, e });
            }
            else
            {
                // цвет
                switch (e.Thread.Status)
                {
                    case "ERROR":
                            this.listBox_Log.ForeColor = Color.Red;
                            this.TS_StatusThreadLabel.ForeColor = Color.Red;
                        break;
                    case "Completed":
                        if (this.SendMailStat == Common.Com.MyEvent.ERROR)
                        {
                            this.listBox_Log.ForeColor = Color.Red;
                            this.TS_StatusThreadLabel.ForeColor = Color.Red;
                        }
                        else
                        {
                            this.listBox_Log.ForeColor = Color.Green;
                            this.TS_StatusThreadLabel.ForeColor = Color.Green;
                            this.panel_CreateThread.Visible = true;
                        }
                        break;
                    default:
                            this.TS_StatusThreadLabel.ForeColor = Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
                            this.listBox_Log.ForeColor = Color.Black;
                        break;
                }

                // текст статуса в нижней части формы
                this.TS_StatusThreadLabel.Text = e.Thread.Status;

                if (this.panel_LogThread.Visible && this.MyCom.Threader.CurrentWorkflow != null)
                {
                    if (this.listBox_Log.Items.Count < 2) LoadPannelLog();
                    else this.listBox_Log.Items[1] = @"Статус: (" + e.Thread.Status + @")" + (e.ErMessage != null ? " " + e.ErMessage : "");
                }

                // Изменился статус на Running значит уже получен список объектов который нужно выгружать
                if (e.Thread.Status == "Running" && this.listBox_Log.Items.Count == 2)
                {
                    foreach (Common.MyTask item in e.Thread.CurrentWorkflow)
                    {
                        if (item.FILE_PATH != null) this.listBox_Log.Items.Add(item.FILE_PATH);
                        else
                            if (item.LIST_NAME != null) this.listBox_Log.Items.Add(item.LIST_NAME);

                    }
                }
            }
        }

        // Произошло изменение статуса по заливке объекта
        private delegate void MonRowAffweted(object sender, Common.Com.onComThreadrRowAffwetedArg e);
        void Threader_onRowAffweted(object sender, Common.Com.onComThreadrRowAffwetedArg e)
        {
            if (this.listBox_Log.InvokeRequired)
            {
                MonRowAffweted tsDelegate = new MonRowAffweted(Threader_onRowAffweted);
                this.listBox_Log.Invoke(tsDelegate, new object[] { sender, e });
            }
            else
            {
                if (this.MyCom.Threader.Status == "Running" && this.listBox_Log.Items.Count >= 2+e.Task.Index+1)
                {
                    string s = "";
                    for (int i = 0; i < this.MyCom.Threader.CurrentWorkflow.MaxLenFilePath - (e.Task.FILE_PATH != null ? e.Task.FILE_PATH.Length : (e.Task.LIST_NAME != null ? e.Task.LIST_NAME.Length : 0)) + 8; i++)
                    {
                        s = s + " ";
                    }

                    this.listBox_Log.Items[2 + e.Task.Index] = (e.Task.FILE_PATH != null ? e.Task.FILE_PATH : e.Task.LIST_NAME) + s + e.RowAffweted.ToString() + @" строк. Статус (" + e.Stat + @")";
                }
            }
        }

        // Произошло событие отправки сообщения
        private delegate void MonSendMail(object sender, Common.EvenvSmtpEmail e);
        void Threader_onEvenvSendMail(object sender, Common.EvenvSmtpEmail e)
        {
            if (this.listBox_Log.InvokeRequired)
            {
                MonSendMail tsDelegate = new MonSendMail(Threader_onEvenvSendMail);
                this.listBox_Log.Invoke(tsDelegate, new object[] { sender, e });
            }
            else
            {
                if (this.MyCom.Threader.Status == "Running")
                {
                    string s = null;
                    if (e.Status != Common.Com.MyEvent.ERROR) s = string.Format("Отправка сообщения на почтовый ящик {0} завершилась со статусом {1}", e.Email.To, e.Status.ToString());
                    else
                    {
                        s = string.Format("Отправка сообщения на почтовый ящик {0} завершилась со статусом {1} ({2})", e.Email.To, e.Status.ToString(), e.Message);
                        this.SendMailStat = e.Status;
                    }

                    if (s!=null) this.listBox_Log.Items.Add(s);
                }
            }
        }

        // Произошло событие изменения строки подключения к базе данных
        void Provider_onSaveConnectStr(object sender, Common.Com.onComProviderArg e)
        {
            Form_Start_Load(null, null);
        }

        // Пользователь вызвал окно для создания подключения к базе данных
        private void TSMI_ConnectFromOracle_Click(object sender, EventArgs e)
        {
            try
            {
                Form_Add_Source F_Add_S = new Form_Add_Source(this.MyCom);
                if (F_Add_S.ShowDialog() == DialogResult.OK)
                {
                    this.MyCom.MyConfig.SetProvider(F_Add_S.Provider);
                    this.MyCom.Provider = F_Add_S.Provider;
                    this.Form_Start_Load(null, null);
                }
                else { }

                //F_Add_S.Dispose();
            }
            catch (Exception ex)
            {
                ex.Source += Common.Com.MyEvent.UserControl.ToString();
                this.MyCom.SystemEvent(ex.Source + @": " + ex.Message, Common.Com.MyEvent.ERROR);
                //throw ex;
            }

        }

        // Пользователь решил запустить выбранное задание
        private void btn_CreateThread_Click(object sender, EventArgs e)
        {
            if (this.panel_CreateThread.Visible && this.comboBox_CreateThread.Visible)
            {
                if (this.comboBox_CreateThread.SelectedIndex == -1) MessageBox.Show("Вы не выбрали задание которое нужно запустить.");
                else
                {
                    try
                    {
                        this.listBox_Log.Items.Clear();
                        this.panel_LogThread.Visible = true;
                        // Если есть пустые параметры
                        if (this.MyCom.SprWorkflow[this.comboBox_CreateThread.SelectedIndex].HashEmptyPar())
                        {
                            MessageBox.Show("Не определены параметры для выбранного задания");
                            btn_SetParam_Click(null, null);
                        }
                        else
                        {
                            this.MyCom.Threader.StartWorkflow(this.comboBox_CreateThread.SelectedIndex);
                            this.panel_CreateThread.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        // Пользователь решил запустить справку
        private void TSMI_Info_Click(object sender, EventArgs e)
        {
            Form_Info F_Info = new Form_Info();
            if (F_Info.ShowDialog() == DialogResult.OK)
            {
            }
            else { }

            F_Info.Dispose();
        }

        // Пользователь запустил форму для ввода параметров
        private void btn_SetParam_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBox_CreateThread.SelectedIndex != -1)
                {
                    Form_Param F_Add_S = new Form_Param(this.MyCom, this.comboBox_CreateThread.SelectedIndex);
                    if (F_Add_S.ShowDialog() == DialogResult.OK)
                    {

                    }
                    else { }
                }

                //F_Add_S.Dispose();
            }
            catch (Exception ex)
            {
                ex.Source += Common.Com.MyEvent.UserControl.ToString();
                this.MyCom.SystemEvent(ex.Source + @": " + ex.Message, Common.Com.MyEvent.ERROR);
                throw ex;
            }
        }

        // Пользователь выбрал задание для запуска
        private void comboBox_CreateThread_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox_CreateThread.SelectedIndex != -1 && (this.MyCom.SprWorkflow[this.comboBox_CreateThread.SelectedIndex].GetParam()).Count > 0)
            {
                // отчистка от текущих параметров
                foreach (Common.MyParam item in this.MyCom.SprParam)
                {
                    item.Clear();
                }
                
                // У этой задачи существуют параметры
                this.btn_SetParam.Visible = true;

                // Собственно запуск формы для ввода параметров
                btn_SetParam_Click(null, null);
            }
            else this.btn_SetParam.Visible = false;
        }

        // Пользователь вызвал форму с лицензией
        private void лицензияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Lic FLic = new Form_Lic(this.MyCom);
            FLic.ShowDialog();
        }
    }
}
