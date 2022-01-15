using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoritmExport.Common
{
    public class MyParam
    {
        private Com _MyCom;
        public int Index { get; private set; }
        Com.Com_SprParam SprParam;
        public string Name { get; private set; }
        public string Caption { get; private set; }
        public string Typ { get; private set; }
        public string Format { get; private set; }
        public string ShablonSelected { get; private set; }
        public string ShablonAll { get; private set; }
        public string Default { get; private set; }
        private string DefaultRezStr;
        private DateTime? DefaultRezDt;
        private List<string> DefaultRezLst;
        public string InnerText { get; private set; }
        private DateTime _nullvaldate;
        private DateTime _valdatepriv;
        public DateTime _valdate 
        {
            get 
            {
                if (this._valdatepriv.Year == 1 && DefaultRezDt != null) return DateTime.Parse(DefaultRezDt.ToString());
                else return _valdatepriv;
            }
            private set { _valdatepriv = value; } 
        }
        private List<MyParamForComboBox> _valComboBox = null;
        private string _valueStr = null;
        public string Value 
        {
            get 
            {
                string rez="";
                if (Typ == "Calendar")
                {
                    rez = Format;

                    rez = rez.Replace("DD", (_valdate.Day.ToString().ToString().Length == 1 ? "0" + _valdate.Day.ToString() : _valdate.Day.ToString()));
                    rez = rez.Replace("MM", (_valdate.Month.ToString().ToString().Length == 1 ? "0" + _valdate.Month.ToString() : _valdate.Month.ToString()));
                    rez = rez.Replace("YYYY", _valdate.Year.ToString());

                }
                if (Typ == "ComboBox")
                {
                    rez = this._valueStr == null ? this.DefaultRezStr : this._valueStr;
                }
                if (Typ == "ListBox")
                {
                    try
                    {
                        // Пользователь выбрал все объекты
                        if (this.ValAll)
                        {
                            rez = ShablonAll;
                        }
                        else   // Пользователь выбрал опеделённый список объектов
                        {
                            int start = ShablonSelected.IndexOf('{');
                            int end = ShablonSelected.IndexOf('}');
                            string element = ShablonSelected.Substring(start + 1, end - start - 1);
                            string elementtmp = element;
                            string elements = "";
                            for (int i = 0; i < this.ValListPriv.Count; i++)
                            {
                                elementtmp = element;
                                elements = elements + elementtmp.Replace("@id", this.ValListPriv[i]) + (i < (this.ValListPriv.Count - 1) ? "," : "");
                            }
                            rez = ShablonSelected.Substring(0, start) + elements + ShablonSelected.Substring(end + 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Не смогли обработать ListBox" + ex.Message);
                    }
                }
                return rez; 
            }
            private set { }
        }
        private bool? ValAllPriv;         // Когда пользователь выбрал все элементы
        public bool ValAll 
        {
            get 
            {
                // Если есть значние All по умолчанию
                if (ValAllPriv==null && this.DefaultRezLst != null && this.DefaultRezLst.Count == 1)
                {
                    if (this.DefaultRezLst[0] == this.ShablonAll) return true;
                }
                return (this.ValAllPriv == null ? false : (this.ValAllPriv == true?true:false));
            }
            private set { this.ValAllPriv = value; }
        }
        private List<string> ValListPriv;      // Когда параметр имеет несколько значений
        public List<string> ValList 
        {
            get
            {
                // Если есть значние All по умолчанию
                if (this.DefaultRezLst != null)
                {
                    if (this.DefaultRezLst[0] != this.ShablonAll)
                    {
                        return DefaultRezLst;
                    }
                }
                return ValListPriv;
            }
            private set { this.ValListPriv = value; } 
        }

        public bool HechEmpty
        {
            get 
            {
                if (this.Typ == "Calendar" && this._nullvaldate == _valdate && this.DefaultRezDt==null) return true;
                if (this.Typ == "ComboBox" && (this._valueStr == null || this._valueStr.Trim() == string.Empty) 
                    && (this.DefaultRezStr == null || this.DefaultRezStr.Trim() == string.Empty)) return true;
                if (this.Typ == "ListBox" && (!ValAll && this.ValListPriv.Count == 0)
                    && (this.DefaultRezLst != null && this.DefaultRezLst.Count==0)) return true;
                return false; 
            }
            private set { }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Name">Имя параметра</param>
        /// <param name="Caption">Заголовок параметра</param>
        /// <param name="Typ">Тип параметра</param>
        /// <param name="Format">Формат параметра при выводе в строку</param>
        public MyParam(string Name, string Caption, string Typ, string Format, string ShablonSelected, string ShablonAll, string InnerText, string Default)
        {
            this.Name = Name;
            this.Caption = Caption;
            this.Typ = Typ;
            this.Format = Format;
            this.ShablonSelected = ShablonSelected;
            this.ShablonAll = ShablonAll;
            this.InnerText = InnerText;
            this.ValListPriv = new List<string>();
            this.Default = Default;

            // отчистка параметра, чтобы при смене задания не сохранялись текущие параметры
            Clear();
        }

        /// <summary>
        /// Инициализация списка задач в задании
        /// </summary>
        /// <param name="MyCom">Наш основной объект</param>
        /// <param name="Wokflow">НашСписолЗадач</param>
        public void InicializationForSprWorkflow(Com.Com_SprParam SprParam, Com _MyCom)
        {
            this._MyCom = _MyCom;
            this.SprParam = SprParam;
            this.Index = _MyCom.SprWorkflow.Count;

            // Подписфываемся на событие добавления объекта в справочник
            //SprWorkflow.onAddWorkflow += new EventHandler<Com.onWokflowIteratorArg>(SprWorkflow_onAddWorkflow);
        }

        /// <summary>
        /// Получение списка значений. Делаем кеш для того чтобы не обращаться много раз к базе за списком
        /// </summary>
        /// <returns></returns>
        public List<MyParamForComboBox> getMyParamList()
        {
            // Если мы неделали обращения к базе за получением нужных элементов списка, то делаем это
            if (this._valComboBox != null && this._valComboBox.Count==0)
            {
                this._valComboBox = this._MyCom.Provider.GetListForComboBox(this.InnerText);
            }

            return this._valComboBox;
        }

        /// <summary>
        /// Получаем элемент списка по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MyParamForComboBox getMyParamList(int index)
        {
            if (this._valComboBox != null && this._valComboBox.Count == 0) getMyParamList();
            return this._valComboBox[index];
        }

        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="dt">Дата время которую устанавливаем</param>
        public void SetValue(DateTime dt)
        {
            this._valdate = dt;
        }
        public void SetValue(string setValStr)
        {
            this._valueStr = setValStr;
        }
        public void SetValAll(bool value)
        {
            this.ValAll = value;
            // Сбрасываем значение по умолчанию, чтобы не выводилось
            this.DefaultRezLst = null;
        }
        public void SetValList(List<string> value)
        {
            this.ValListPriv = value;
        }

        // Какой индекс у элемента в списке значений если его ид
        public int getIndexFromValList(string id)
        {
            int rez = -1;

            for (int i = 0; i < this._valComboBox.Count; i++)
            {
                if(this._valComboBox[i].ID.Equals(id)) return i;
            }

            return rez;
        }


        /// <summary>
        /// Клонирование параметра
        /// </summary>
        /// <returns></returns>
        public MyParam Clone()
        {
            MyParam rez = new MyParam(this.Name, this.Caption, this.Typ, this.Format, this.ShablonSelected, this.ShablonAll, this.InnerText, this.Default);

            if(this._MyCom!=null || this.SprParam!=null) rez.InicializationForSprWorkflow(this.SprParam, this._MyCom);
            rez.SetValue(this._valdate);
            rez.SetValue(this.Value);
            rez.SetValAll(this.ValAll);
            rez.SetValList(this.ValList);

            return rez;
        }

        // отчистка параметра, чтобы при смене задания не сохранялись текущие параметры
        public void Clear()
        {
            this._valdate = _nullvaldate;
            this._valComboBox = new List<MyParamForComboBox>();
        }

        /// <summary>
        /// Установка значений по умолчанию на основе базы занных
        /// </summary>
        public void SetDefaultParamForBD()
        {
            // Если это календарь то значение по умолчанию сохраняем в 
            if (this.Typ == "Calendar") { DefaultRezDt = this._MyCom.Provider.GetDefaultRezDt(this.Default); }
            else
            {
                if (this.Typ == "ListBox") DefaultRezLst = this._MyCom.Provider.GetDefaultRezLst(this.Default);
                else DefaultRezStr = this._MyCom.Provider.GetDefaultRezStr(this.Default);
            }
        }
    }
}
