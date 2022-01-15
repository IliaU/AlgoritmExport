using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;

namespace AlgoritmExport.Common
{
    // Класс представляющий задание
    public class MyWorkflow : IEnumerable
    {
        #region Properties
            private Com _MyCom;
            private Com.Com_SprWorkflow SprWorkflow;
            private List<MyTask> _Tasks;
            public int Index { get; private set; }
            public int Count { get { return _Tasks!=null?_Tasks.Count:0; } private set { } }
            public string LongName = null;        // Длинное имя задания
            public string ShortName = null;       // Короткое имя задания
            public string QUERY; //{ get; private set; }
            public string CHCP { get; private set; }             // Кодировка, которую будем использовать, по умодчанию будет использоваться текущая
            public Encoding EnCHCP
            {
                get 
                {
                    Encoding rez = Encoding.Default;
                    try 
                    {
                        rez = Encoding.GetEncoding(int.Parse(this.CHCP));
                    }
                    catch
                    {
                        try
                        {
                            rez = Encoding.GetEncoding(this.CHCP);
                        }
                        catch 
                        {
                            rez = Encoding.Default;
                        }
                    }

                    return rez;
                }
                private set { }
            }                   // сконвертировали в объект Encoding
            public int MaxLenFilePath
            {
                get 
                {
                    if (_Tasks == null || _Tasks.Count == 0) return 0;

                    int rez = 0;
                    foreach (MyTask item in this._Tasks)
                    {
                        if (item.FILE_PATH != null && item.FILE_PATH.Length > rez) rez = item.FILE_PATH.Length;
                        if (item.LIST_NAME != null && item.LIST_NAME.Length > rez) rez = item.LIST_NAME.Length;
                    }
                    return rez;
                }
                private set { }
            }

            private Excel.Application excelapp = null;              // Создаём приложение
            //private Excel.Workbooks excelappworkbooks=null;       // Список книг в приложении
            private Excel.Workbook excelappworkbook = null;         // Книга с которой мы работаем
            public Excel.Sheets excelsheets = null;                // Массив листов с которыми мы потом будем работать
            public Excel.Worksheet excelworksheet = null;          // Текущий лист в приложении ексель
            public Excel.Range excelcells = null;                  // Ссылка на ячейку с которой мы сейчас работаем
            public int ColumnCount = 0;


            private string _FILE_PATH_FOR_EXEL;
            public string FILE_PATH_FOR_EXEL
            {
                get
                {
                    if (this._MyCom == null) return this._FILE_PATH_FOR_EXEL;
                    if (this._FILE_PATH_FOR_EXEL == null) return null;
                    return this._FILE_PATH_FOR_EXEL.Replace(@"@CurrentFolder", this._MyCom.CurrentDirectory);
                }
                private set { }
            }

            private string _EmailQuery;
            public string EmailQuery
            {
                get
                {
                    return this._EmailQuery;
                }
                private set { }
            }
            /// <summary>
            /// Объект представляющий из себя список почтовых серверов и адресатов на которые необходимо отправить сообщения
            /// </summary>
            public List<MySmtpEmail> ListSmtpServer = new List<MySmtpEmail>();

            public bool HachColName { get; private set; }
            /// <summary>
            /// Последний запуск события изменения количества перекаченных строк
            /// </summary>
            public DateTime LastEvent = DateTime.Now;

            /// <summary>
            /// Событие добавление объекта в список задачи
            /// </summary>
            public event EventHandler<Com.onTaskIteratorArg> onAddTask;             // Сохранение строки подключения
            public event EventHandler<Com.onWokflowIteratorArg> onClearAllTask;     // Отчистка всех объектов в задаче

            /// <remarks>Индексатор</remarks>
            public MyTask this[int i]
            {
                get { return this._Tasks[i]; }
                set { this._Tasks[i] = value; }
            }

            /// <summary>
            /// Нинциализация при создании EXCEL
            /// </summary>
            public void BeginCreateExcel(Common.Com.Com_Thread Thr, MyTask Task)
            {
                // Создаём глобальные объекты для работы с екселем
                //this.excelapp = null;              // Создаём приложение
                //this.excelappworkbooks=null;       // Список книг в приложении
                //this.excelappworkbook = null;         // Книга с которой мы работаем
                //this.excelsheets = null;                // Массив листов с которыми мы потом будем работать
                if (this.FILE_PATH_FOR_EXEL != null && this.FILE_PATH_FOR_EXEL.Trim() != string.Empty && Count > 0)
                {

                    if (this.excelsheets == null)
                    {
                        // Загрузка приложения и получение текущей книги
                        this.excelapp = new Excel.Application();                                     // Загрузка приложения
                        //this.excelapp.Visible = true;                                              // Включаем видимость его для пользователя
                        this.excelapp.SheetsInNewWorkbook = Count;                   // Указываем количество листов в новой книге
                        this.excelapp.Workbooks.Add(Type.Missing);                                   // указываем, что загружать шаблон не нужно
                        this.excelappworkbook = (this.excelapp.Workbooks)[this.excelapp.Workbooks.Count];      // Получаем ссылоку на последнюю рабочую книгу в приложении

                        // получаем объект со списокм листов с которым мы потом будем работать
                        this.excelsheets = this.excelappworkbook.Worksheets;

                        this.excelworksheet = null;             // Текущий лист в приложении ексель
                        this.excelcells = null;                 // Ссылка на ячейку с которой мы сейчас работаем
                    }
                   
                    if (this.excelsheets != null)
                    {
                        this.excelworksheet = (Excel.Worksheet)this.excelsheets.get_Item(Task.Index + 1);     // Получаем ссылку на лист с которым будем работать
                        if (Task.LIST_NAME != null) this.excelworksheet.Name = Task.LIST_NAME;               // Присваиваем имя нашему листу
                    }
                }
            }

            /// <summary>
            /// Сохранение книги экселя
            /// </summary>
            /// <param name="Thr"></param>
            public void EndCreateExcel(Common.Com.Com_Thread Thr, bool Save)
            {
                // Закрываем и сохраняем книгу
                if (excelappworkbook != null)
                {
                    // Пробегаем по колонкам и делаем автоподбор ширины
                    for (int i = 1; i <= this.ColumnCount; i++)
                    {
                        ((Excel.Range)excelworksheet.Columns[i, Type.Missing]).EntireColumn.AutoFit();
                    }

                    if (Save)
                    {
                        excelappworkbook.SaveAs(Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL);        // Сохраняем файл
                        excelappworkbook.Close();
                        //excelapp.DefaultSaveFormat=Excel.XlFileFormat.xlExcel9795;        // Устанавливаем формат
                        excelapp.Quit();                                                    // Выгружаем приложение
                    }
                }
            }

        #endregion

        #region Public metod
            /// <remarks>Конструктор</remarks>
            public MyWorkflow(string QUERY, string LongName, string ShortName, string FILE_PATH_FOR_EXEL, string EmailQuery, bool HachColName, string CHCP)
            {
                this.Index = -1;
                this.QUERY = QUERY;
                this.LongName = LongName;
                this.ShortName = ShortName;
                this._FILE_PATH_FOR_EXEL = FILE_PATH_FOR_EXEL;
                this._EmailQuery = EmailQuery;
                this.CHCP = CHCP;

                if (HachColName != null) this.HachColName = HachColName;
                else this.HachColName = false;

                this._Tasks = new List<MyTask>();
            }

            /// <remarks>Передаём перечислитель чтобы можно было использовать в коде forech </remarks>
            public IEnumerator GetEnumerator()
            {
                return this._Tasks.GetEnumerator();
            }

            /// <summary>
            /// Создание новой чистой копии данного объекта
            /// </summary>
            /// <returns>новая чистая копия</returns>
            public MyWorkflow CloneClear()
            {
                MyWorkflow temp = new MyWorkflow(this.QUERY, this.LongName, this.ShortName, this._FILE_PATH_FOR_EXEL, this.EmailQuery, this.HachColName, this.CHCP);
                temp.InicializationForMyWorkflow(this._MyCom);
                return temp;
            }

            /// <summary>
            /// Инициализация списка задач в задании
            /// </summary>
            /// <param name="MyCom">Наш основной объект</param>
            /// <param name="Wokflow">НашСписолЗадач</param>
            public void InicializationForSprWorkflow(Com.Com_SprWorkflow SprWorkflow, Com _MyCom)
            {
                this._MyCom = _MyCom;
                this.SprWorkflow = SprWorkflow;
                this.Index = _MyCom.SprWorkflow.Count;

                // Подписфываемся на событие добавления объекта в справочник
                //SprWorkflow.onAddWorkflow += new EventHandler<Com.onWokflowIteratorArg>(SprWorkflow_onAddWorkflow);
            }

            // Пользователь добавляет задание
            public bool Add(MyTask Task)
            {
                bool rez = false;
                lock (this._Tasks)
                {
                    try
                    {
                        if (this._MyCom.SystemEvent("Добавление объекта " + Task.FILE_PATH + @" в задание " + this.ShortName, Com.MyEvent.Wokflow))
                        {
                            Task.InicializationForTaskIteratir(this, this._MyCom);

                            // Собственно обработка событие добовление задачи в задание
                            Com.onTaskIteratorArg myArg = new Com.onTaskIteratorArg(Task);
                            this._Tasks.Add(Task);
                            if (onAddTask != null)
                            {
                                onAddTask.Invoke(this, myArg);
                            }

                            rez = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Source += Com.MyEvent.Wokflow.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.ERROR);
                        throw ex;
                    }
                }
                return rez;
            }

            // Пользователь отчищает список заданий
            public bool Clear()
            {
                bool rez = false;
                lock (this._Tasks)
                {
                    try
                    {
                        if (this._MyCom.SystemEvent("Отчистка списка объектов в задании " + this.ShortName, Com.MyEvent.Wokflow))
                        {

                            // Собственно обработка событие добовление задачи в задание
                            Com.onWokflowIteratorArg myArg = new Com.onWokflowIteratorArg(this);
                            if (onClearAllTask != null)
                            {
                                this._Tasks.Clear();
                                onClearAllTask.Invoke(this, myArg);
                            }

                            rez = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Source += Com.MyEvent.Wokflow.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.ERROR);
                        throw ex;
                    }
                }
                return rez;
            }

            // Получаем список используемых параметров
            public List<MyParam> GetParam()
            {
                List<MyParam> rez = new List<MyParam>();

                foreach (MyParam item in this._MyCom.SprParam)
                {
                    if (QUERY.IndexOf(@"@" + item.Name) > 0)
                    {
                        item.SetDefaultParamForBD();
                        rez.Add(item);
                    }
                }

                return rez;
            }

            // Возвращаем если хоть один из параметров не определён
            public bool HashEmptyPar()
            {
                bool rez = false;

                foreach (MyParam item in GetParam())
                {
                    if (item.HechEmpty) return true;
                }

                return rez;
            }

        #endregion

        #region Private metod

            // Инициализация объекта при копировании этого объекта
            private void InicializationForMyWorkflow(Com _MyCom)
            {
                this._MyCom = _MyCom;
            }

            /// <summary>
            /// Событие добавления объекта в основной справочник, при этом событии можно получить индекс в этом справочнике
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            //private void SprWorkflow_onAddWorkflow(object sender, Com.onWokflowIteratorArg e)
            //{
            //    if (this.Index==-1)
            //        this.Index = ((Com.Com_SprWorkflow)sender).Count-1;
            //}

        #endregion
    }



}
