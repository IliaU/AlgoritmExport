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
    public partial class Form_Param : Form
    {
        private Common.Com MyCom;

        List<Common.MyParam> Par;

        public Form_Param(Common.Com MyCom, int IndexWorkflow)
        {
            this.MyCom = MyCom;
            InitializeComponent();
            Par=this.MyCom.SprWorkflow[IndexWorkflow].GetParam();
        }

        private void Form_Param_Load(object sender, EventArgs e)
        {
            this.lBox_Param.Items.Clear();

            foreach (Common.MyParam item in Par)
            {
                this.lBox_Param.Items.Add(item.Caption);
            }

            lBox_Param_SelectedIndexChanged(null, null);
        }

        // Пользователь выбрал параметр
        private void lBox_Param_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lBox_Param.SelectedIndex != -1)
            {
                panel_value.Visible = true;

                // Скрываем элементы по умолчанию
                this.Calendar_Tek.Visible = false; this.lbl_Calendar_Tek.Visible = false;
                this.ComboBox_Tek.Visible = false;
                panel_ListBox.Visible = false;

                // Обработка типа поля Calendar
                if (this.Par[this.lBox_Param.SelectedIndex].Typ == "Calendar") 
                { 
                    this.Calendar_Tek.Visible = true;
                    if(!this.Par[this.lBox_Param.SelectedIndex].HechEmpty) this.lbl_Calendar_Tek.Visible = true;
                    else this.lbl_Calendar_Tek.Visible = false;
                    this.Calendar_Tek.SelectionRange = new SelectionRange(this.Par[this.lBox_Param.SelectedIndex]._valdate, this.Par[this.lBox_Param.SelectedIndex]._valdate);
                    this.lbl_Calendar_Tek.Text=this.Par[this.lBox_Param.SelectedIndex]._valdate.ToShortDateString();
                }

                // Обработка типа поля CompoBox
                if (this.Par[this.lBox_Param.SelectedIndex].Typ == "ComboBox")
                {
                    this.ComboBox_Tek.Visible = true;
                    this.ComboBox_Tek.Items.Clear();
                    int CBSelectIndex = -1;
                    int CBI = -1;
                    foreach (Common.MyParamForComboBox item in this.Par[this.lBox_Param.SelectedIndex].getMyParamList())
                    {
                        CBI++;
                        this.ComboBox_Tek.Items.Add(item.TXT);
                        if (item.ID == this.Par[this.lBox_Param.SelectedIndex].Value) CBSelectIndex = CBI;
                    }

                    // Выделяем уже выбранный параметр
                    if (CBSelectIndex>-1) this.ComboBox_Tek.SelectedIndex = CBSelectIndex;
                }

                // Обработка типа поля ListBox
                if (this.Par[this.lBox_Param.SelectedIndex].Typ == "ListBox")
                {
                    panel_ListBox.Visible = true;
                    panel_ListBox.Dock = DockStyle.Fill;
                    this.checkBox_ForListBox.Checked = this.Par[this.lBox_Param.SelectedIndex].ValAll;
                    this.listBox_ForListBox.Items.Clear();
                    this.listBox_ForListBox.SelectedIndex = -1;
                    int CBI = -1;
                    foreach (Common.MyParamForComboBox item in this.Par[this.lBox_Param.SelectedIndex].getMyParamList())
                    {
                        CBI++;
                        this.listBox_ForListBox.Items.Add(item.TXT);
                        if (this.checkBox_ForListBox.Checked) this.listBox_ForListBox.SelectedIndex = CBI;
                    }

                    // Выделяем уже выбранный параметр
                    foreach (string item in this.Par[this.lBox_Param.SelectedIndex].ValList)
                    {
                        this.listBox_ForListBox.SelectedIndex=this.Par[this.lBox_Param.SelectedIndex].getIndexFromValList(item);
                    }
                }
            }
            else
            {
                panel_value.Visible = false;
            }
        }

        // Пользователь выбрал дату
        private void Calendar_Tek_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (this.lBox_Param.SelectedIndex != -1)
            {
                this.Par[this.lBox_Param.SelectedIndex].SetValue(e.Start);
                if (!this.Par[this.lBox_Param.SelectedIndex].HechEmpty) this.lbl_Calendar_Tek.Visible = true;
                this.lbl_Calendar_Tek.Text = e.Start.ToShortDateString();
            }
        }

        // Пользователь выбрал значение в списке
        private void ComboBox_Tek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBox_Tek.SelectedIndex != -1)
            {
                string ID = this.Par[this.lBox_Param.SelectedIndex].getMyParamList(this.ComboBox_Tek.SelectedIndex).ID;
                this.Par[this.lBox_Param.SelectedIndex].SetValue(ID);
            }
        }

        // Пользователь выбрал всё в нашем ListBox
        private void checkBox_ForListBox_Click(object sender, EventArgs e)
        {
            this.Par[this.lBox_Param.SelectedIndex].SetValAll(this.checkBox_ForListBox.Checked);

            // проверяем, что объект доступен
            if (panel_ListBox.Visible)
            {
                // Выделяем всё
                if (this.checkBox_ForListBox.Checked)
                {
                    for (int i = 0; i < this.listBox_ForListBox.Items.Count; i++)
                    {
                        this.listBox_ForListBox.SelectedIndex = i;
                    }
                }
                else
                {   // Снимаем выделение
                    this.listBox_ForListBox.SelectedIndex = -1;

                    // Выделяем уже выбранный параметр
                    foreach (string item in this.Par[this.lBox_Param.SelectedIndex].ValList)
                    {
                        this.listBox_ForListBox.SelectedIndex = this.Par[this.lBox_Param.SelectedIndex].getIndexFromValList(item);
                    }
                }
            }
        }

        // Пользователь выбрал конкретное кначение в нашем ListBox
        private void listBox_ForListBox_Click(object sender, EventArgs e)
        {
            if (!this.checkBox_ForListBox.Checked)
            {
                List<string> rez = new List<string>();

                for (int i = 0; i < this.listBox_ForListBox.Items.Count; i++)
                {
                    // если элемент выбран
                    if (((ListBox)sender).GetSelected(i))
                    {
                        rez.Add(this.Par[this.lBox_Param.SelectedIndex].getMyParamList()[i].ID);
                    }
                }

                // Указываем новый список выбранных элементов у нашего пользователя
                this.Par[this.lBox_Param.SelectedIndex].SetValList(rez);
            }
        }
    }
}
