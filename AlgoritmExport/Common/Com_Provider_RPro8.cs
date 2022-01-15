using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgoritmExport.Lib;
using System.IO;


namespace AlgoritmExport.Common
{
    public partial class Com
    {
        public class Com_Provider_RPro8 : ProviderI
        {
            #region Private Param
                private Com _MyCom;
                private string _ConnectString;
                private object MyLock = new object();
            #endregion



            /// <summary>
            /// Тип провайдеоа
            /// </summary>
            /// <returns>Кастомизированный тип провайдера</returns>
            public Provider_En GetTyp()
            {
                return Provider_En.RPro8;
            }

            /// <summary>
            /// Текущая строка подключения
            /// </summary>
            /// <returns></returns>
            public string ConnectString()
            {
                return this._ConnectString;
            }

            /// <summary>
            /// Сохранение строки подключения
            /// </summary>
            public event EventHandler<onComProviderArg> onSaveConnectStr;      // Сохранение строки подключения

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="com">Основной объект</param>
            public Com_Provider_RPro8(Com MyCom)
            {
                this._MyCom = MyCom;

                if (this._MyCom.SystemEvent("Загрузка провайдера: " + Provider_En.RPro8.ToString(), MyEvent.Com_Provider))
                {
                    try
                    {
                    }
                    catch (Exception ex)
                    {
                        ex.Source += MyEvent.Com_Provider.ToString();
                        throw ex;
                    }
                }
            }


            /// <summary>
            /// Проверка строки подключения
            /// </summary>
            /// <param name="ConnectStr">Строка подключения</param>
            /// <returns>Успешная строка подключения</returns>
            public string TestCon(string ConnectStr)
            {
                if (string.IsNullOrWhiteSpace(ConnectStr))
                    throw new ApplicationException("Не указана папка Rpro в которой хранится база данных");

                try
                {
                    string[] fileSource;
                    fileSource = Directory.GetFiles(ConnectStr, @"Client.Dat");
                    if (fileSource == null || fileSource.Length == 0) throw new ApplicationException(string.Format("Нет файлов отвечающих условию: {0}", @"Client.Dat"));
                    fileSource = Directory.GetFiles(ConnectStr, @"INVN.DAT");
                    if (fileSource == null || fileSource.Length == 0) throw new ApplicationException(string.Format("Нет файлов отвечающих условию: {0}", @"INVN.DAT"));
                }
                catch (Exception ex) { throw new ApplicationException(ex.Message); }

                return ConnectStr;
            }


            /// <summary>
            /// Сохранение строки подключения
            /// </summary>
            /// <param name="ConnectStr">Строка подключения</param>
            /// <returns>Успех сохранения строки подключения</returns>
            public bool SaveConnectStr(string ConnectStr)
            {
                try
                {
                    // Если нет ошибки в подключениии к источнику
                    if (TestCon(ConnectStr) != null)
                    {
                        if (this._MyCom.SystemEvent("Строка подключения корректна", MyEvent.Com_Provider))
                        {
                            this._ConnectString = ConnectStr;
                            onComProviderArg myArg = new onComProviderArg(this);

                            if (onSaveConnectStr != null)
                            {
                                onSaveConnectStr.Invoke(this, myArg);
                            }

                            return true;
                        }
                    }
                    else this._MyCom.SystemEvent("Строка подключения оказалась не корректна", MyEvent.Com_Provider);

                    return false;
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, MyEvent.ERROR);
                    throw ex;
                }
            }

            /// <summary>
            /// Получаем список для комбобокса
            /// </summary>
            /// <param name="Sql">Запрос к базе</param>
            /// <returns>результат</returns>
            public List<MyParamForComboBox> GetListForComboBox(string Sql)
            {
                try
                {
                    List<MyParamForComboBox> rez = new List<MyParamForComboBox>();

                    /*using (OracleConnection con = new OracleConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OracleCommand com = new OracleCommand(Sql, con))
                        {
                            using (OracleDataReader read = com.ExecuteReader())
                            {
                                if (read.HasRows)
                                {
                                    while (read.Read())
                                    {
                                        string ID = null;
                                        string TXT = null;

                                        // Получаем значение
                                        for (int i = 0; i < read.FieldCount; i++)
                                        {
                                            if (read.GetName(i) == @"ID" && !read.IsDBNull(i)) ID = read.GetString(i);
                                            if (read.GetName(i) == @"TXT" && !read.IsDBNull(i)) TXT = read.GetString(i);
                                        }

                                        // Проверка полученного значения
                                        if (ID == null || ID.Trim() == string.Empty) throw new ApplicationException("В списке поле ID не можеть быть пустым. Для TXT пустое поле разрешается");

                                        rez.Add(new MyParamForComboBox(ID, TXT));
                                    }
                                }
                            }
                        }
                    }*/

                    return rez;
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();
                    throw ex;
                }
            }

            /// <summary>
            /// Получаем значение по умолчанию для DefaultRezDt
            /// </summary>
            /// <param name="Sql">Запрос для получения значения по умолчанию</param>
            /// <returns>Значение по умолчанию</returns>
            public DateTime? GetDefaultRezDt(string Sql)
            {
                try
                {
                    DateTime? rez = null;

                    /*
                    if (Sql == null || Sql.Trim() == String.Empty) return rez;

                    using (OracleConnection con = new OracleConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OracleCommand com = new OracleCommand(Sql, con))
                        {
                            using (OracleDataReader read = com.ExecuteReader())
                            {
                                if (read.HasRows)
                                {
                                    while (read.Read())
                                    {
                                        // Получаем значение
                                        for (int i = 0; i < read.FieldCount; i++)
                                        {
                                            if (read.GetName(i) == @"ID" && !read.IsDBNull(i)) rez = read.GetDateTime(i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    */
                    return rez;
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();
                    throw ex;
                }
            }

            /// <summary>
            /// Получаем значение по умолчанию для DefaultRezStr
            /// </summary>
            /// <param name="Sql">Запрос для получения значения по умолчанию</param>
            /// <returns>Значение по умолчанию</returns>
            public string GetDefaultRezStr(string Sql)
            {
                try
                {
                    string rez = null;
                    /*
                    if (Sql == null || Sql.Trim() == String.Empty) return rez;

                    using (OracleConnection con = new OracleConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OracleCommand com = new OracleCommand(Sql, con))
                        {
                            using (OracleDataReader read = com.ExecuteReader())
                            {
                                if (read.HasRows)
                                {
                                    while (read.Read())
                                    {
                                        // Получаем значение
                                        for (int i = 0; i < read.FieldCount; i++)
                                        {
                                            if (read.GetName(i) == @"ID" && !read.IsDBNull(i)) rez = read.GetString(i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    */
                    return rez;
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();
                    throw ex;
                }
            }

            /// <summary>
            /// Получаем значение по умолчанию для DefaultRezLst
            /// </summary>
            /// <param name="Sql">Запрос для получения значения по умолчанию</param>
            /// <returns>Значение по умолчанию</returns>
            public List<string> GetDefaultRezLst(string Sql)
            {
                try
                {
                    List<string> rez = new List<string>();
                    /*
                    if (Sql == null || Sql.Trim() == String.Empty) return rez;

                    using (OracleConnection con = new OracleConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OracleCommand com = new OracleCommand(Sql, con))
                        {
                            using (OracleDataReader read = com.ExecuteReader())
                            {
                                if (read.HasRows)
                                {
                                    while (read.Read())
                                    {
                                        // Получаем значение
                                        for (int i = 0; i < read.FieldCount; i++)
                                        {
                                            if (read.GetName(i) == @"ID" && !read.IsDBNull(i)) rez.Add(read.GetString(i));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    */
                    return rez;
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();
                    throw ex;
                }
            }


            /// <summary>
            /// Установка задяния в переданном потоке
            /// </summary>
            /// <param name="Thr">Асинхронный поток</param>
            /// <param name="SorceQUERY">Запрос для получения списка заданий</param>
            /// <param name="SourceEmailQuery">Запрос для получения настроек почтового сервиса</param>
            public void SetThreadCurrentWorkflow(Com_Thread Thr, string SorceQUERY, string SourceEmailQuery)
            {
                if (SorceQUERY == null || SorceQUERY.Trim() == string.Empty) throw new ApplicationException(@"В задании не указан запрос.");

                // Получаем список запросов
                string[] Tasks = SorceQUERY.Split('^');
                foreach (string iTask in Tasks)
                {
                    string QUERY = null;
                    string FILE_PATH = null;
                    string COL_SPAN = null;
                    string ROW_SPAN = null;
                    string LIST_NAME = null;
                    string SpecificFormat = null;
                    string SpecialColumnName = null;
                    bool Append = false;

                    // Список колонок
                    string[] Columns = iTask.Split('!');
                    foreach (var iColumn in Columns)
                    {
                        string[] Values = iColumn.Split('|');
                        if (Values.Length!=2) throw new ApplicationException(@"В запросе задачи (" + Thr.CurrentWorkflow.LongName + ") не правильно указаны параметры запроса.");

                        switch (Values[0])
                        {
                            case "QUERY":
                                QUERY = Values[1];
                                break;
                            case "FILE_PATH":
                                FILE_PATH = Values[1];
                                break;
                            case "COL_SPAN":
                                COL_SPAN = Values[1];
                                break;
                            case "ROW_SPAN":
                                ROW_SPAN = Values[1];
                                break;
                            case "LIST_NAME":
                                LIST_NAME = Values[1];
                                break;
                            case "SpecificFormat":
                                SpecificFormat = Values[1];
                                break;
                            case "SpecialColumnName":
                                SpecialColumnName = Values[1];
                                break;
                            case "Append":
                                try {Append = bool.Parse(Values[1].ToString());}
                                catch (Exception) { }
                                break;
                            default:
                                break;
                        }
                    }


                    if (QUERY == null || QUERY.Trim() == string.Empty)
                        throw new ApplicationException(@"В запросе задачи (" + Thr.CurrentWorkflow.LongName + ") не указан запрос для объекта который является обязательным параметром.");
                    if ((FILE_PATH == null || FILE_PATH.Trim() == string.Empty) && Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL != null && Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL.Trim() == string.Empty)
                        throw new ApplicationException(@"В запросе задачи (" + Thr.CurrentWorkflow.LongName + ") не указан путь к файлу для объекта который является обязательным параметром.");
                    if ((LIST_NAME == null || LIST_NAME.Trim() == string.Empty) && (Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL != null && Thr.CurrentWorkflow.FILE_PATH_FOR_EXEL.Trim() != string.Empty))
                        throw new ApplicationException(@"В запросе задачи (" + Thr.CurrentWorkflow.LongName + ") не указано имя листа под которым результат будет сохранён в ехел файле.");


                    // Добавляем объекты нашей задачи
                    MyTask newTask;
                    newTask = new MyTask(_MyCom, QUERY, FILE_PATH, LIST_NAME, SpecificFormat, (COL_SPAN != null && COL_SPAN.Trim() != string.Empty ? COL_SPAN : this._MyCom.DefColSpan), (ROW_SPAN != null && ROW_SPAN.Trim() != string.Empty ? ROW_SPAN : this._MyCom.DefRowSpan), SpecialColumnName, Append);

                    Thr.CurrentWorkflow.Add(newTask);
                }
            }

            /// <summary>
            /// Установка задяния в переданном потоке
            /// </summary>
            /// <param name="Thr">Асинхронный поток</param>
            public void RunCurrentTask(Com_Thread Thr, MyTask Task, bool HachNotColName)
            {
                try
                {
                    lock (MyLock)
                    {
                        switch (Task.QUERY)
                        {
                            case "Client.Dat":
                                // Получаем массив байт для каждой строки
                                foreach (Byte[] item in GetRowsFromFile(Task.QUERY, 1008))
                                {

                                    // Если это первая строка и указано, что нужно сохранть имена колонок, то делаем это
                                    List<string> ColumnName = null;
                                    if (Task.RowAffweted == 0 &&
                                            (Thr.CurrentWorkflow.HachColName || (Task.SpecificFormat != null && Task.SpecificFormat.Trim() != string.Empty)) &&
                                            !HachNotColName
                                       )
                                    {
                                        ColumnName = new List<string>()
                                        {"FirstName","LastName","Address1","Phone1","EmailAddr","MaxDiscPerc","CustId","CustSid","LastSaleDate"};
                                        // Сохраняем и сбрасываем строку
                                        Thr.SetRowRez(Task, ColumnName, true, ColumnName);
                                    }

                                    string FirstName = GetStringForByte(item, 46);
                                    string LastName = GetStringForByte(item, 77);
                                    string Address1 = GetStringForByte(item, 108);
                                    string Phone1 = GetStringForByte(item, 215);
                                    string EmailAddr = GetStringForByte(item, 789);
                                    decimal? MaxDiscPerc = GetDecimalForByte2Delit2(item, 869);
                                    long? CustId = GetLongForByte8(item, 900);
                                    long? CustSid = GetLongForByte8(item, 908);
                                    if (CustSid == null) throw new ApplicationException(string.Format("Не смогли получить ключь клиента при парсинге из файла"));
                                    //DateTime? FstSaleDate = GetDateTimeForByte8(item, 932);
                                    DateTime? LastSaleDate = GetDateTimeForByte8(item, 932);

                                    if (CustSid != 0)
                                    {
                                        List<string> rez = new List<string>()
                                        {
                                            FirstName, LastName, Address1, Phone1, EmailAddr, MaxDiscPerc.ToString(), CustId.ToString(), CustSid.ToString(), LastSaleDate.ToString()
                                        };

                                        // Сохраняем строку
                                        Thr.SetRowRez(Task, rez, false, ColumnName);
                                    }
                                }

                                break;
                            case @"INVN.DAT":
                                // Получаем массив байт для каждой строки
                                foreach (Byte[] item in GetRowsFromFile(Task.QUERY, 224))
                                {
                                    string OKS = GetStringForByte(item, 4);
                                    string Articl = GetStringForByte(item, 52);
                                    string Atriib = GetStringForByte(item, 83);
                                    string Size = GetStringForByte(item, 99, 2);
                                    DateTime? dt = GetDateTimeForByte4(item, 191);
                                    int Nalog = GetIntForByte2(item,194);
                                    DateTime? dt1 = GetDateTimeForByte8(item, 211);
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
            }


            #region Специальные функции парсинга
            
            /// <summary>
            /// Чтение строк из файла
            /// </summary>
            /// <param name="FileName">Шаблон файла</param>
            /// <param name="TopLen">Длина заголовка</param>
            /// <param name="RowLen">Длина строки</param>
            /// <returns>Массив байт</returns>
            private IEnumerable<byte[]> GetRowsFromFile(string FileName, int TopLen, int RowLen)
            {
                // Иногда длина строки разная в зависимотсти от данных которые там хранятся
                int tmpRowLen = RowLen;
                //lock (this.MyLock)
                //{
                    string[] fileSource;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(this._ConnectString)) throw new ApplicationException(string.Format("Нет подключения к базе данных."));
                        if (string.IsNullOrWhiteSpace(FileName)) throw new ApplicationException(string.Format("Не указан файл откуда читать строки."));
                        fileSource = Directory.GetFiles(this._ConnectString, FileName);

                        if (fileSource == null || fileSource.Length == 0) throw new ApplicationException(string.Format("Нет файлов отвечающих условию: {0}", FileName));
                    }
                    catch (IOException ex)
                    {
                        ex.Source += MyEvent.Com_Provider.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при чтени данных из файла {0}. {1}", FileName, ex.Message), MyEvent.ERROR);
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        ex.Source += MyEvent.Com_Provider.ToString();

                        this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при чтени данных из файла {0}. {1}", FileName, ex.Message), MyEvent.ERROR);
                        throw ex;
                    }

                    // Пробегаем по списку файлов
                    foreach (string item in fileSource)
                    {
                        // прочитали весь файл
                        byte[] fileB = File.ReadAllBytes(item);
                        bool flagfirst = true;

                        if (fileB.Length <= TopLen)
                        {
                            // Проверка заголовка
                            if (flagfirst) { CheckFirstReadPage(fileB, TopLen, true); flagfirst = false; }
                            else yield return fileB;
                        }
                        else
                        {
                            int startPos = 0;
                            // Бегаем по строкам
                            while (fileB.Length > startPos)
                            {
                                byte[] rez = null;

                                // Переменные для работы с чеками
                                int DocType = 0;  // Байт по которому мы определяем что это возврат или продажа 72 - Возврат | 122 - Продажа 
                                int rowcount = 0; // Кол-во строк одна строка 216 байт

                                // длина массива которую нужно вернуть
                                if (flagfirst)
                                {
                                    if (fileB.Length - startPos >= TopLen) rez = new byte[TopLen];
                                    else rez = new byte[fileB.Length - TopLen];
                                }
                                else
                                {
                                    // Есть файлы где длина строки может быть разной
                                    if (FileName == @"SA??????.Dat")   // Если это чеки
                                    {
                                        /*
                                        216 байт заголовок строк
                                        648 (216*3) кол-во строк какая то инфа по строкам
                                        864(216*4) что-то от заголовка при возврате 1080 (216*5)
                                        648 (216*3) кол-во строк какая то инфа по строкам
                                        */
                                        DocType = fileB[startPos + 372];  // Байт по которому мы определяем что это возврат или продажа 72 - Возврат | 122 - Продажа 
                                        rowcount = BitConverter.ToInt32(fileB, startPos + 210); // Кол-во строк одна строка 216 байт

                                        if (DocType == 72) tmpRowLen = 216 + (rowcount * 216 * 2) + (216 * 5);  //72 - Возврат
                                        else tmpRowLen = 216 + (rowcount * 216 * 2) + (216 * 4);                //122 - Продажа 
                                    }

                                    if (fileB.Length - startPos >= tmpRowLen) rez = new byte[tmpRowLen];
                                    else rez = new byte[fileB.Length - tmpRowLen];

                                }

                                // Обработка чеков требует специального подхода
                                if (!flagfirst && FileName == @"SA??????.Dat")   // Если это чеки
                                {
                                    // Создаём промежуточный буфер чтобы возвращать построчно
                                    byte[] rezChk = null;
                                    if (DocType == 72) rezChk = new byte[216 + (216 * 2) + (216 * 5)]; //72 - Возврат
                                    else rezChk = new byte[216 + (216 * 2) + (216 * 4)]; //72 - Продажа

                                    // Парсим документ на строки
                                    for (int i = 0; i < rowcount; i++)
                                    {
                                        // Заполняем массив заголовка первые 216 байт
                                        for (int t1 = 0; t1 < 216; t1++)
                                        {
                                            rezChk[t1] = fileB[startPos + t1];
                                        }
                                        // Заполняем массив заголовка вторые 864(216*4) для продаж 1080(216*5) для возвратов байт
                                        for (int t2 = 0; t2 < (DocType == 72 ? 1080 : 864); t2++)
                                        {
                                            rezChk[216 + t2] = fileB[startPos + 216 + (rowcount * 216) + t2];
                                        }
                                        // Заполняем массив первыми данными по выбранной строке
                                        for (int r1 = 0; r1 < 216; r1++)
                                        {
                                            rezChk[216 + (DocType == 72 ? 1080 : 864) + r1] = fileB[startPos + 216 + (216 * i) + r1];
                                        }
                                        // Заполняем массив вторыми данными по выбранной строке
                                        for (int r2 = 0; r2 < 216; r2++)
                                        {
                                            int ttt = startPos + 216 + (216 * rowcount) + (DocType == 72 ? 1080 : 864) + (216 * i) + r2;
                                            rezChk[216 + (DocType == 72 ? 1080 : 864) + 216 + r2] = fileB[startPos + 216 + (216 * rowcount) + (DocType == 72 ? 1080 : 864) + (216 * i) + r2];
                                        }

                                        yield return rezChk;
                                    }
                                }
                                else
                                {
                                    // Заполняем массив
                                    for (int i = 0; i < rez.Length; i++)
                                    {
                                        rez[i] = fileB[startPos + i];
                                    }
                                }

                                // передвигаем указатель
                                if (flagfirst) startPos = startPos + TopLen;
                                else startPos = startPos + tmpRowLen;

                                // Проверка заголовка
                                if (flagfirst) { CheckFirstReadPage(rez, TopLen, true); flagfirst = false; }
                                else
                                {
                                    if (FileName != @"SA??????.Dat")   // Если это  не чеки
                                    {
                                        yield return rez;
                                    }
                                }
                            }
                        }
                    }
                    yield break;
                //}
            }
            /// <summary>
            /// Чтение строк из файла
            /// </summary>
            /// <param name="FileName">Шаблон файла</param>
            /// <param name="TopLen">Длина заголовка</param>
            /// <returns>Массив байт</returns>
            private IEnumerable<byte[]> GetRowsFromFile(string FileName, int TopLen)
            {
                // Если длина строк не указана значит она ровна заголовку
                return GetRowsFromFile(FileName, TopLen, TopLen);
            }
            /// <summary>
            /// Проверка заголовка страниц
            /// </summary>
            /// <param name="b">массив первых байт</param>
            /// <param name="RowLen">Длина строки которую нужно проверить в заголовке</param>
            /// <param name="HashException">выводить ошибки</param>
            /// <returns>Результат проверки</returns>
            private bool CheckFirstReadPage(byte[] b, int RowLen, bool HashException)
            {
                bool rez = false;
                try
                {
                    if (b == null) throw new ApplicationException("Нет массива данных в котором нужно сделать проверку заголовка.");
                    if (b.Length < 14) throw new ApplicationException("Массив коткий не сможем сделать проверку.");
                    if (b[0] != 255 || b[1] != 255 || b[2] != 255 || b[3] != 255) throw new ApplicationException("Скорее всего файл не того формата.");
                    if ((b[13] * 256) + b[12] != RowLen) throw new ApplicationException(string.Format("В заголовке файла указана длина строки {0} байт, а вы парсите по {1} байт.", (b[13] * 256) + b[12], RowLen));
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получаем строку из байт
            /// </summary>
            /// <param name="RowB">Массив байт</param>
            /// <param name="StartPozition">Начальная позиция строки</param>
            /// <returns>Строка которая получается в текущей кодировке из массива байт</returns>
            private string GetStringForByte(byte[] RowB, int StartPozition)
            {
                string rez = null;
                try
                {
                    int len = RowB[StartPozition];
                    byte[] tmpb = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i + 1];
                    }
                    rez = Encoding.Default.GetString(tmpb);
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получаем строку из байт
            /// </summary>
            /// <param name="RowB">Массив байт</param>
            /// <param name="StartPozition">Начальная позиция строки</param>
            /// <param name="Len">Длитна строки</param>
            /// <returns>Строка которая получается в текущей кодировке из массива байт</returns>
            private string GetStringForByte(byte[] RowB, int StartPozition, int Len)    
            {
                string rez = null;
                try
                {
                    byte[] tmpb = new byte[Len];
                    for (int i = 0; i < Len; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }
                    rez = Encoding.Default.GetString(tmpb);
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }


            /// <summary>
            /// Получаем long обработав 8 байт от начала которое мы указали
            /// </summary>
            /// <param name="RowB">Массив байт</param
            /// <param name="StartPozition">Начальная позиция</param>
            /// <returns></returns>
            private long? GetLongForByte8(byte[] RowB, int StartPozition)
            {
                long? rez = null;
                try
                {
                    byte[] tmpb = new byte[8];
                    for (int i = 0; i < tmpb.Length; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }

                    Int64 ii = BitConverter.ToInt64(tmpb, 0);
                    rez = (long?)ii;

                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получаем число из 4 байт
            /// </summary>
            /// <param name="RowB">Массив байт</param
            /// <param name="StartPozition">Начальная позиция</param>
            /// <returns></returns>
            private int GetIntForByte4(byte[] RowB, int StartPozition)
            {
                int rez;
                try
                {
                    byte[] tmpb = new byte[4];
                    for (int i = 0; i < tmpb.Length; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }

                    rez = BitConverter.ToInt32(tmpb, 0);
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получаем число из 4 байт
            /// </summary>
            /// <param name="RowB">Массив байт</param
            /// <param name="StartPozition">Начальная позиция</param>
            /// <returns></returns>
            private int GetIntForByte2(byte[] RowB, int StartPozition)
            {
                int rez;
                try
                {
                    byte[] tmpb = new byte[2];
                    for (int i = 0; i < tmpb.Length; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }

                    rez = BitConverter.ToInt16(tmpb, 0);
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получить десимал из массива из 2х байт, где 2 знака в кконце фиксированы для разделителя
            /// </summary>
            /// <param name="RowB">Массив байт</param>
            /// <param name="StartPozition">Начальная позиция</param>
            /// <returns>Возвращает значение полученное из байтов</returns>
            private decimal? GetDecimalForByte2Delit2(byte[] RowB, int StartPozition)
            {
                decimal? rez = null;
                try
                {
                    byte[] tmpb = new byte[2];
                    for (int i = 0; i < tmpb.Length; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }

                    Int16 ii = BitConverter.ToInt16(tmpb, 0);
                    rez = ((decimal)ii) / 100;
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получить десимал из массива из 2х байт, где 2 знака в кконце фиксированы для разделителя
            /// </summary>
            /// <param name="RowB">Массив байт</param>
            /// <param name="StartPozition">Начальная позиция</param>
            /// <returns>Возвращает значение полученное из байтов</returns>
            private decimal? GetDecimalForByte4Delit3(byte[] RowB, int StartPozition)
            {
                decimal? rez = null;
                try
                {
                    byte[] tmpb = new byte[4];
                    for (int i = 0; i < tmpb.Length; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }

                    Int32 ii = BitConverter.ToInt32(tmpb, 0);
                    rez = ((decimal)ii) / 1000;
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получаем дату время из 8 батов
            /// </summary>
            /// <param name="RowB">Массив байт</param>
            /// <param name="StartPozition">Начальная позиция</param>
            /// <returns>Возвращает значение полученное из байтов</returns>
            private DateTime? GetDateTimeForByte8(byte[] RowB, int StartPozition)
            {
                DateTime? rez = null;
                try
                {
                    byte[] tmpb = new byte[2];
                    for (int i = 0; i < tmpb.Length; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }
                    Int16 yyyy = BitConverter.ToInt16(tmpb, 0);
                    Int16 mm = RowB[StartPozition + 2];
                    Int16 dd = RowB[StartPozition + 3];
                    Int16 h = RowB[StartPozition + 4];
                    Int16 mi = RowB[StartPozition + 5];
                    Int16 ss = RowB[StartPozition + 6];

                    if (yyyy != 0 && mm != 0 && dd != 0)
                    {
                        string tmp = string.Format("{0}.{1}.{2} {3}:{4}:{5}", dd.ToString("D2"), mm.ToString("D2"), yyyy.ToString("D4"), h.ToString(), mi.ToString("D2"), ss.ToString("D2"));
                        try
                        {
                            rez = DateTime.Parse(tmp);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            /// <summary>
            /// Получаем дату время из 8 батов
            /// </summary>
            /// <param name="RowB">Массив байт</param>
            /// <param name="StartPozition">Начальная позиция</param>
            /// <returns>Возвращает значение полученное из байтов</returns>
            private DateTime? GetDateTimeForByte4(byte[] RowB, int StartPozition)
            {
                DateTime? rez = null;
                try
                {
                    byte[] tmpb = new byte[2];
                    for (int i = 0; i < tmpb.Length; i++)
                    {
                        tmpb[i] = RowB[StartPozition + i];
                    }
                    Int16 yyyy = BitConverter.ToInt16(tmpb, 0);
                    Int16 mm = RowB[StartPozition + 2];
                    Int16 dd = RowB[StartPozition + 3];

                    if (yyyy != 0 && mm != 0 && dd != 0)
                    {
                        string tmp = string.Format("{0}.{1}.{2} {3}:{4}:{5}", dd.ToString("D2"), mm.ToString("D2"), yyyy.ToString("D4"), "00", "00", "00");
                        try
                        {
                            rez = DateTime.Parse(tmp);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + string.Format("Произожла ошибка при получении данных с источника. {0}", ex.Message), MyEvent.ERROR);
                    throw ex;
                }
                return rez;
            }

            #endregion
        }
    }
}
