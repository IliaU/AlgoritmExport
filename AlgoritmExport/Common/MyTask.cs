using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace AlgoritmExport.Common
{
    public class MyTask
    {
        private Com _MyCom;
        private MyWorkflow Wokflow;
        public int Index { get; private set; }
        public string QUERY { get; private set; }
        private string _FILE_PATH;
        public string FILE_PATH 
        {
            get
            {
                if (this._MyCom == null) return this._FILE_PATH;
                if (this._FILE_PATH == null) return null;// this._FILE_PATH = @"@CurrentFolder\Expoprt\" + this.Index.ToString() + @".txt";
                return this._FILE_PATH.Replace(@"@CurrentFolder", this._MyCom.CurrentDirectory);
            }
            private set { }
        }
        //private string L_FILE_PATH() { return FILE_PATH; } 
        private string _COL_SPAN = null;
        public string COL_SPAN
        {
            get
            {
                if (this._MyCom != null && this._COL_SPAN == null) return this._MyCom.DefColSpan;
                return this._COL_SPAN;
            }
            private set { }
        }
        private string _ROW_SPAN = null;
        public string ROW_SPAN
        {
            get
            {
                if (this._MyCom != null && this._ROW_SPAN == null) return this._MyCom.DefRowSpan;
                return this._ROW_SPAN;
            }
            private set { }
        }
        public string LIST_NAME { get; private set; }
        public string SpecificFormat { get; private set; }
        public string[] SpecialColumnName { get; private set; }
        public bool Append { get; private set; }

        /// <summary>
        /// Счётчик с колличеством залитых строк
        /// </summary>
        public Int64 RowAffweted = 0;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="QUERY">Запрос из которого нужно выкачать данные</param>
        /// <param name="FILE_PATH">Путь в месте с именем файла куда сохранить данные</param>
        /// <param name="LIST_NAME">Имя листа при выгрузке в ексель</param>
        /// <param name="COL_SPAN">Разделитель который использовать для колонок, если он отличается от заданных по умолчанию</param>
        /// <param name="ROW_SPAN">Разделитель который использовать для строк, если он отличается от заданных по умолчанию</param>
        /// <param name="SpecialColumnName">Массив специальных имён к колонкам</param>
        /// <param name="Append">Признак добавления добавления а не перетирания исходного файла</param>
        public MyTask(Com _MyCom, string QUERY, string FILE_PATH, string LIST_NAME, string SpecificFormat, string COL_SPAN, string ROW_SPAN, string[] SpecialColumnName, bool Append)
        {
            this.Index = -1;
            this._MyCom = _MyCom;
            if (QUERY == null || QUERY.Trim() == string.Empty) throw new ApplicationException("В заданиях запрос является обязательным полем");
            this.QUERY = QUERY;
            this.SpecialColumnName = SpecialColumnName;
            this.Append = Append;

            if (FILE_PATH != null && FILE_PATH.Trim() != string.Empty) this._FILE_PATH = FILE_PATH;
            else this._FILE_PATH = null;

            if (LIST_NAME != null && LIST_NAME.Trim() != string.Empty) this.LIST_NAME = LIST_NAME;
            else this.LIST_NAME = null;

            if (COL_SPAN != null && COL_SPAN.Trim() != string.Empty) this._COL_SPAN = COL_SPAN;
            if (ROW_SPAN != null && ROW_SPAN.Trim() != string.Empty) this._ROW_SPAN = ROW_SPAN;

            // Проверка пути к папке с файлом, если папки нет то создаём её
            string[] patch = this.FILE_PATH.Split('\\');
            string patchTmp = patch[0];
            for (int i = 1; i < patch.Count() - 1; i++)
            {
                patchTmp = patchTmp + @"\" + patch[i];

                // Если папки нет, то создаём её
                if (!Directory.Exists(patchTmp)) Directory.CreateDirectory(patchTmp);
            }

            if (SpecificFormat != null && SpecificFormat.Trim() != string.Empty) this.SpecificFormat = SpecificFormat;
            else this.SpecificFormat = null;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="QUERY">Запрос из которого нужно выкачать данные</param>
        /// <param name="FILE_PATH">Путь в месте с именем файла куда сохранить данные</param>
        /// <param name="LIST_NAME">Имя листа при выгрузке в ексель</param>
        /// <param name="Append">Признак добавления добавления а не перетирания исходного файла</param>
        /// <param name="Append">Признак добавления добавления а не перетирания исходного файла</param>
        public MyTask(Com _MyCom, string QUERY, string FILE_PATH, string LIST_NAME, string SpecificFormat, bool Append) : this(_MyCom, QUERY, FILE_PATH, LIST_NAME, SpecificFormat, null, null, (string)null, Append) { }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="QUERY">Запрос из которого нужно выкачать данные</param>
        /// <param name="FILE_PATH">Путь в месте с именем файла куда сохранить данные</param>
        /// <param name="LIST_NAME">Имя листа при выгрузке в ексель</param>
        /// <param name="COL_SPAN">Разделитель который использовать для колонок, если он отличается от заданных по умолчанию</param>
        /// <param name="ROW_SPAN">Разделитель который использовать для строк, если он отличается от заданных по умолчанию</param>
        /// <param name="SpecialColumnName">Строка где разделителем является запятая</param>
        /// <param name="Append">Признак добавления добавления а не перетирания исходного файла</param>
        public MyTask(Com _MyCom, string QUERY, string FILE_PATH, string LIST_NAME, string SpecificFormat, string COL_SPAN, string ROW_SPAN, string SpecialColumnName, bool Append) : this(_MyCom, QUERY, FILE_PATH, LIST_NAME, SpecificFormat, COL_SPAN, ROW_SPAN, (SpecialColumnName != null ? SpecialColumnName.Split(',') : null), Append) { }

        /// <summary>
        /// Инициализация списка задач в задании
        /// </summary>
        /// <param name="MyCom">Наш основной объект</param>
        /// <param name="Wokflow">НашСписолЗадач</param>
        public void InicializationForTaskIteratir(MyWorkflow Wokflow, Com _MyCom)
        {
            this._MyCom = _MyCom;
            this.Wokflow = Wokflow;
            this.Index = Wokflow.Count;

            // Подписываемся на событие добавления объекта в список задач, во время этого происходит монопольная блокировака поэтому количество задач в списке является индексом для этого объекта
            //this.Wokflow.onAddTask += new EventHandler<Com.onTaskIteratorArg>(_TaskIterator_onAddTask);
        }

        /// <summary>
        /// Инициализация задачи перед началом обработки
        /// </summary>
        public void BeginCreate(Common.Com.Com_Thread Thr, MyTask Task)
        {
            try
            {
                this.RowAffweted = 0;
                if (Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL != null && Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL.Trim() != string.Empty)
                {
                    Thr.CurrentWorkflow.BeginCreateExcel(Thr, Task);
                }
                else
                {
                    // Если нужно обработать в спецефичным формате
                    if (Task.SpecificFormat != null && Task.SpecificFormat.Trim() != string.Empty)
                    {
                        string[] str = Task.SpecificFormat.Split(';');
                        switch (str[0])
                        {
                            case "XML":
                                Thr.SaveNewRow(Task, str[1], this._MyCom.IOCountPoput);
                                Thr.SaveNewRow(Task, @"<"+str[2]+@">", this._MyCom.IOCountPoput);
                                break;
                            case "XML2":
                                Thr.SaveNewRow(Task, str[1], this._MyCom.IOCountPoput);
                                Thr.SaveNewRow(Task, str[2], this._MyCom.IOCountPoput);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source += "BeginCreate";

                this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.ERROR);
                throw ex;
            }

        }

        /// <summary>
        /// Обработка строки для EXCEL
        /// </summary>
        /// <param name="Thr"></param>
        /// <param name="Row"></param>
        /// <param name="isHeder"></param>
        public void SetRowForExcel(Common.Com.Com_Thread Thr, List<string> Row, bool isHeder)
        {
            if (Row != null && Thr.CurrentWorkflow.excelsheets != null)
            {
                Thr.CurrentWorkflow.ColumnCount = Row.Count;
                

                // Если есть выгрузка в ексель
                if (Thr.CurrentWorkflow.excelworksheet != null)
                {
                    if (RowAffweted == 0 && Thr.CurrentWorkflow.HachColName)
                    {
                        for (int i = 0; i < Thr.CurrentWorkflow.ColumnCount; i++)
                        {
                            // Получаем ссулку на ячейку с которой работаем
                            Thr.CurrentWorkflow.excelcells = (Excel.Range)Thr.CurrentWorkflow.excelworksheet.Cells[1, i + 1];

                            // Присваиваем значение и форматируем ячейку
                            Thr.CurrentWorkflow.excelcells.Value2 = Row[i];
                            Thr.CurrentWorkflow.excelcells.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            Thr.CurrentWorkflow.excelcells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            //excelcells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Thr.CurrentWorkflow.ColumnCount; i++)
                        {
                            // Получаем ссулку на ячейку с которой работаем
                            Thr.CurrentWorkflow.excelcells = (Excel.Range)Thr.CurrentWorkflow.excelworksheet.Cells[RowAffweted + (Thr.CurrentWorkflow.HachColName ? 1 : 0), i + 1];

                            // Присваиваем значение и форматируем ячейку
                            Thr.CurrentWorkflow.excelcells.Value2 = Row[i];
                            Thr.CurrentWorkflow.excelcells.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            Thr.CurrentWorkflow.excelcells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                    }
                }      
            }
        }

        /// <summary>
        /// Окончание обработки задачи
        /// </summary>
        /// <param name="Thr"></param>
        public void EndCreate(Common.Com.Com_Thread Thr, bool Save)
        {
            try
            {
                if (Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL != null && Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL.Trim() != string.Empty)
                {
                    Thr.CurrentWorkflow.EndCreateExcel(Thr, Save);
                }
                else
                {
                    // Если нужно обработать в спецефичным формате
                    if (this.SpecificFormat != null && this.SpecificFormat.Trim() != string.Empty)
                    {
                        string[] str = this.SpecificFormat.Split(';');
                        switch (str[0])
                        {
                            case "XML":
                                Thr.SaveNewRow(this, @"</" + str[2] + @">", this._MyCom.IOCountPoput);
                                break;
                            case "XML2":
                                Thr.SaveNewRow(this, str[4], this._MyCom.IOCountPoput);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source += "EndCreate";

                this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.ERROR);
                throw ex;
            }
        }


        /// <summary>
        /// Событие добавления объектов в списке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void _TaskIterator_onAddTask(object sender, Com.onTaskIteratorArg e)
        //{
        //    if(this.Index==-1)
        //        this.Index=((MyWorkflow)sender).Count-1;
        //}
    }
}
