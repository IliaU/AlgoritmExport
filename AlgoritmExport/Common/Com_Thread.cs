using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgoritmExport.Lib;
using System.Threading;
using Oracle.DataAccess.Client;
using System.IO;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        /// <summary>
        /// Клас который собственно выполняет задание которое мы запускаем на выполнение
        /// </summary>
        public class Com_Thread
        {
            #region Properties
                private Com _MyCom;
                private MyStatus _Status;
                private Thread Thr;
                private List<MyParam> Params = new List<MyParam>();
                /// <summary>
                /// Список значений параметров которые передану через консоль
                /// </summary>
                private List<string> my_params = new List<string>();

                public string Status { get { return this._Status.ToString(); } private set { } }
                public MyWorkflow CurrentWorkflow;

                public int TimeLimitForEventAffectedRows = 5;        // Количество секунд, через которое будут возникать события AffectedRows

                /// <summary>
                /// Системные события
                /// </summary>
                public event EventHandler<onComThreadrArg> onChengStatus;                   // Событие изменения статуса объекта выполняющего задания
                public event EventHandler<onComThreadrRowAffwetedArg> onRowAffweted;        // Событие возникающие при изменениии RowAffweted
                /// <summary>
                /// Событие отпраки сообщения
                /// </summary>
                public event EventHandler<EvenvSmtpEmail> onEvenvSendMail;

            #endregion

            #region Public metod
                /// <summary>
                /// Конструктор
                /// </summary>
                /// <param name="MyCom"></param>
                public Com_Thread(Com MyCom)
                {
                    this._MyCom = MyCom;
                    this._Status = MyStatus.NotActiv;
                }

                /// <summary>
                /// Запуск задания на выполнение
                /// </summary>
                /// <param name="Workflow">Задание которое следует запустить</param>
                public void StartWorkflow(MyWorkflow Workflow)
                {
                    try
                    {
                        this.CurrentWorkflow = Workflow.CloneClear();
                        this.GetParam();
                        this.ChengStatus(MyStatus.Starting);

                        // Асинхронный запуск нашего процесса
                        this.Thr = new Thread(Run);
                        this.Thr.Name = "AE_Thr_" + this.CurrentWorkflow.ShortName;
                        this.Thr.IsBackground = true;
                        this.Thr.Start();

                    }
                    catch (Exception ex)
                    {
                        ex.Source += MyEvent.Com_Thread.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, MyEvent.ERROR);
                        throw ex;
                    }
                }
                /// <summary>
                /// Запуск задания на выполнение
                /// </summary>
                /// <param name="IndexWorkflow">Индекс задания, которое нужно запустить на выполнение</param>
                public void StartWorkflow(int IndexWorkflow)
                {
                    try
                    {
                        this.StartWorkflow(this._MyCom.SprWorkflow[IndexWorkflow]);
                    }
                    catch (Exception ex)
                    {
                        ex.Source += MyEvent.Com_Thread.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, MyEvent.ERROR);
                        throw ex;
                    }
                }
                /// <summary>
                /// Запуск задания на выполнение
                /// </summary>
                /// <param name="ShortName">Индекс задания, которое нужно запустить на выполнение</param>
                /// <param name="my_params">Список параметров который содержит в одном элементе формат @ParamName=Value</param>
                public void StartWorkflow(string ShortName, List<string> my_params)
                {
                    try
                    {
                        this.my_params = my_params;
                        this.StartWorkflow(this._MyCom.SprWorkflow[ShortName]);
                    }
                    catch (Exception ex)
                    {
                        ex.Source += MyEvent.Com_Thread.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, MyEvent.ERROR);
                        throw ex;
                    }
                }

            #endregion

            #region Private metod
                /// <summary>
                /// Изменение статуса выполяемого процесса
                /// </summary>
                /// <param name="Stat">Статус, который нужно выставить</param>
                /// <param name="ErMessage">Сообщение, например об ошибке, если оно есть</param>
                private void ChengStatus(MyStatus Stat, string ErMessage)
                {
                    this._Status = Stat;

                    if (this._MyCom.SystemEvent("Изменился статус обработчика заданий на: " + Stat.ToString(), MyEvent.Com_Thread))
                    {
                        onComThreadrArg myArg = new onComThreadrArg(this, ErMessage);

                        if (onChengStatus != null)
                        {
                            onChengStatus.Invoke(this, myArg);
                        }
                    }
                }
                /// <summary>
                /// Изменение статуса выполяемого процесса
                /// </summary>
                /// <param name="Stat">Статус, который нужно выставить</param>
                private void ChengStatus(MyStatus Stat) { ChengStatus(Stat, null); }

                /// <summary>
                /// Метод запускаемый асинхронно, который собственно выполняет выгрузку
                /// </summary>
                private void Run()
                {
                    this.ChengStatus(MyStatus.CreateObj);
                    this.CurrentWorkflow.Clear();

                    // Получаем инфу по заданным емейлам на которые нужно отправить сообщения
                    string EmailQuery = this.CurrentWorkflow.EmailQuery;
                    string SorceQUERY = this.CurrentWorkflow.QUERY;
                    try 
	                {
                        // пробегаем по параметрам переданным через консоль
                        foreach (var item in this.my_params)
	                    {
                            string[] t = item.Split('=');
                            if (t.Count() == 2 && t[0].IndexOf('@') > -1)
                            {
                                SorceQUERY = SorceQUERY.Replace(@"@" + t[0].Replace(@"@",""), t[1]);
                            }
	                    }
                        

                        foreach (MyParam item in this.Params)
                        {
                            SorceQUERY = SorceQUERY.Replace(@"@" + item.Name, item.Value);
                        }

                        this._MyCom.Provider.SetThreadCurrentWorkflow(this, SorceQUERY, EmailQuery);

                        // Меняем статус на выполнение заливку последовательно по таблицам
                        this.ChengStatus(MyStatus.Running);

                        // Проверка целевого пути
                        this.ChechFolder(this.CurrentWorkflow.FILE_PATH_FOR_EXEL, this._MyCom.IOCountPoput, false);
                        
                        foreach (MyTask item in CurrentWorkflow)
                        {
                            // Проверка целевого пути если есть вугрузка в файл
                            bool HachColName = this.ChechFolder(item, this._MyCom.IOCountPoput);
                            
                            // Запуск начала обработки задания
                            item.BeginCreate(this, item);
                            
                            // Инициаизация переменных
                            item.RowAffweted = 0;
                            this.CurrentWorkflow.LastEvent = DateTime.Now;  // Последний запуск события изменения количества перекаченных строк
                            SorceQUERY = item.QUERY;

                            // Запускаем обработку задания
                            this._MyCom.Provider.RunCurrentTask(this, item, HachColName);

                            // срабатывает событие AffectedRows по завершению заливки в этот объект
                            AffectedRowFromTask(item, item.RowAffweted, MyStatus.Completed.ToString());

                            // расширяем колонки в книги если выгрузка в excel а если специальный формат, то обрабатываем окончание задачи
                            item.EndCreate(this, false);
                        }

                        // Закрываем и сохраняем книгу
                        this.CurrentWorkflow.EndCreateExcel(this, true);

                        // Смотрим есть кому вообще отправлять письма
                        if (this.CurrentWorkflow.ListSmtpServer.Count > 0)
                        {
                            // Получаем список файлов который нужно отправить в письме
                            List<string> Context = new List<string>();
                            if (!string.IsNullOrWhiteSpace(this.CurrentWorkflow.FILE_PATH_FOR_EXEL)) Context.Add(this.CurrentWorkflow.FILE_PATH_FOR_EXEL);
                            else
                            {
                                foreach (MyTask item in CurrentWorkflow)
                                {
                                    Context.Add(item.FILE_PATH);
                                }
                            }

                            // Пробегаем по списку адресантов которым нужно отправить письма
                            foreach (MySmtpEmail item in this.CurrentWorkflow.ListSmtpServer)
                            {
                                item.onEvenvSendMail += new EventHandler<EvenvSmtpEmail>(item_onEvenvSendMail); 
                                item.SendEmail(Context);
                            }
                        }

                        // меняем статус на завершонный
                        this.ChengStatus(MyStatus.Completed);
	                }
	                catch (Exception ex)
	                {
                        ex.Source += Com.MyEvent.Com_Thread.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message + 
                            (SorceQUERY!=null ? Environment.NewLine+"<<<<<<<<<<---------- Dump Sql ---------------------" 
                                              + Environment.NewLine + SorceQUERY 
                                              + Environment.NewLine+"-------------------- Dump Sql ----------->>>>>>>>>>":"")
                            , Com.MyEvent.ERROR);

                        this.ChengStatus(MyStatus.ERROR, ex.Message);
	                }


                }

                // Обработчик события отправки Сообщения
                void item_onEvenvSendMail(object sender, EvenvSmtpEmail e)
                {
                    // Обработка события
                    if (onEvenvSendMail != null)
                    {
                        onEvenvSendMail.Invoke(this, e);
                    } 
                }

                /// <summary>
                /// Установка результата
                /// </summary>
                /// <param name="Row">Строка с данными</param>
                /// <param name="Row">Строка является заголовком</param>
                public void SetRowRez(MyTask Task, List<string> Row, bool isHeder, List<string> ColumnName)
                {
                    if (this.CurrentWorkflow.FILE_PATH_FOR_EXEL != null && this.CurrentWorkflow.FILE_PATH_FOR_EXEL.Trim() != string.Empty)
                    {
                        Task.SetRowForExcel(this, Row, isHeder);
                    }
                    else
                    {
                        string str = null;
                        if (Task.SpecificFormat != null && Task.SpecificFormat.Trim() != string.Empty)
                        {
                            if (!isHeder)
                            {
                                string[] strSPF = Task.SpecificFormat.Split(';');
                                switch (strSPF[0])
                                {
                                    case "XML":
                                        str = "\t" + @"<" + strSPF[3] + @">" + "\r\n";
                                        for (int i = 0; i < Row.Count; i++)
                                        {
                                            if (Row[i] == null) str += "\t\t" + @"<" + (Task.SpecialColumnName!=null && Task.SpecialColumnName.Length> i ? Task.SpecialColumnName[i] : ColumnName[i]) + @" />" + "\r\n";     // <Lic />
                                            else str += "\t\t" + @"<" + (Task.SpecialColumnName != null && Task.SpecialColumnName.Length > i ? Task.SpecialColumnName[i] : ColumnName[i]) + @">" + "\r\n\t\t\t" + Row[i] + "\r\n\t\t" + @"</" + (Task.SpecialColumnName != null && Task.SpecialColumnName.Length > i ? Task.SpecialColumnName[i] : ColumnName[i]) + @">" + "\r\n";
                                        }
                                        str += "\t" + @"</" + strSPF[3] + @">";
                                        break;
                                    case "XML2":
                                        str = "\t" + @"<" + strSPF[3];
                                        for (int i = 0; i < Row.Count; i++)
                                        {
                                            if (Row[i] != null) str += @" " + (Task.SpecialColumnName != null && Task.SpecialColumnName.Length > i ? Task.SpecialColumnName[i] : ColumnName[i]) + @"=""" + Row[i] + @"""";
                                        }
                                        str += @"/>";
                                        break;
                                    default:
                                        break;
                                }

                                // Сохраняем строку. 
                                SaveNewRow(Task, str, this._MyCom.IOCountPoput);
                            }
                            else
                            {
                                // Проверяем если мы попали в специальный формат, то скорее всего не нужно обрабатывать строчку с заголовком, в этом случае крутим счётчик назад на еденицу
                                string[] strSPF = Task.SpecificFormat.Split(';');
                                switch (strSPF[0])
                                {
                                    case "XML":
                                    case "XML2":
                                            Task.RowAffweted = Task.RowAffweted - 1;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            foreach (string item in Row)
                            {
                                if (str == null) str = (item==null?string.Empty:item);
                                else str = str + Task.COL_SPAN.Replace(@"\t", "\t") + (item == null ? string.Empty : item);
                            }
                            // Сохраняем строку. 
                            SaveNewRow(Task, str, this._MyCom.IOCountPoput);
                        }
                    }
                    // Накручиваем счётчик
                    Task.RowAffweted = Task.RowAffweted + 1;

                    // Срабатывание события AffectedRows по истечению времени 
                    if (this.CurrentWorkflow.LastEvent.AddSeconds(TimeLimitForEventAffectedRows) < DateTime.Now)
                    {
                        AffectedRowFromTask(Task, Task.RowAffweted, MyStatus.Running.ToString());
                        this.CurrentWorkflow.LastEvent = DateTime.Now;
                    }
                    
                }

                /// <summary>
                /// Проверка на наличие данного пути, если его нет, то необходимо его создать, а если в каталоге уже существует целевой файл, то нужно его удалить в зависимости от флага
                /// </summary>
                /// <param name="Task">Задание в рамках которого это делается</param>
                /// <param name="IOCountPoput">Количество повторов, если файл недоступен</param>
                /// <returns>Возвращаем True если файл существовол или существуетю. Нужно для озаписи, чтобы не повторялся заголовок</returns>
                public bool ChechFolder(MyTask Task, int IOCountPoput)
                {
                    if (Task.FILE_PATH != null) return ChechFolder(Task.FILE_PATH, IOCountPoput, Task.Append);
                    else return false;
                }
                
                /// <summary>
                /// Проверка на наличие данного пути, если его нет, то необходимо его создать, а если в каталоге уже существует целевой файл, то нужно его удалить в зависимости от флага
                /// </summary>
                /// <param name="Folder">Задание в рамках которого это делается</param>
                /// <param name="IOCountPoput">Количество повторов, если файл недоступен</param>
                /// <param name="Append">Флаг добавления в файл, С ним файл удаляться не будет и вернётся True если не нужно писать заголовок в файл.</param>
                /// <returns>Возвращаем True если файл существовол или существуетю. Нужно для озаписи, чтобы не повторялся заголовок</returns>
                public bool ChechFolder(string Folder, int IOCountPoput, bool Append)
                {
                    bool rez = false;
                    try
                    {
                        if (Folder != null && Folder.Trim() != string.Empty)
                        {
                            string _Folder = Path.GetDirectoryName(Folder);
                            string[] Folders = _Folder.Split('\\');
                            string NewFolder = null;
                            foreach (string item in Folders)
                            {
                                if (item.IndexOf(@":") == -1)
                                {
                                    NewFolder = NewFolder + @"\" + item;

                                    // Если директории не существует
                                    if (!Directory.Exists(NewFolder)) Directory.CreateDirectory(NewFolder);
                                }
                                else NewFolder = item;
                            }

                            // теперь проверяем наличие файла
                            if (File.Exists(Folder))
                            {
                                if (Append) rez = true;
                                else File.Delete(Folder);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IOCountPoput > 0)
                        {
                            Thread.Sleep(this._MyCom.IOWhileInt);
                            ChechFolder(Folder, IOCountPoput - 1, Append);
                        }
                        else
                            throw ex;
                    }
                    return rez;
                }

                // Сохранение строки в файл
                public void SaveNewRow(MyTask Task, string Row, int IOCountPoput)
                {
                    if (Task.FILE_PATH != null)
                    {
                        try
                        {
                            using (StreamWriter SwFileLog = new StreamWriter(Task.FILE_PATH, true, this.CurrentWorkflow.EnCHCP))   //,Encoding.Unicode   Encoding.GetEncoding(866)
                            {
                                if (Task.ROW_SPAN == @"\r\n" || Task.ROW_SPAN == "\r\n")
                                {
                                    SwFileLog.WriteLine(Row);
                                }
                                else SwFileLog.Write(Task.ROW_SPAN + Row);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (IOCountPoput > 0)
                            {
                                Thread.Sleep(this._MyCom.IOWhileInt);
                                SaveNewRow(Task, Row, IOCountPoput - 1);
                            }
                            else
                                throw ex;
                        }
                    }
                }

                // Срабатывание собятия AffectedRow
                public void AffectedRowFromTask(MyTask Task, Int64 RowAffweted, string Stat)
                {
                    if (this._MyCom.SystemEvent("Статус выгрузки по объекту " + Task.FILE_PATH + @": " + RowAffweted.ToString() + @" AffectedRows", MyEvent.Com_Thread))
                    {
                        onComThreadrRowAffwetedArg myArg = new onComThreadrRowAffwetedArg(Task, RowAffweted, Stat);

                        if (onRowAffweted != null)
                        {
                            onRowAffweted.Invoke(this, myArg);
                        }
                    }
                }

                /// <summary>
                /// Инкапсуляция параметров в нашем задании
                /// </summary>
                private void GetParam()
                {
                    Params.Clear();
                    foreach (MyParam item in this._MyCom.SprParam)
                    {
                        if (this.CurrentWorkflow.QUERY.IndexOf(@"@"+item.Name) > 0)
                        {
                            this.Params.Add(item.Clone());
                        }
                    }
                }
            #endregion

            /// <summary>
            /// Список возможных статусов нашего процесса
            /// </summary>
            public enum MyStatus
            {
                NotActiv, Starting, CreateObj, Running, Completed, ERROR
            }
        }
    }
}
