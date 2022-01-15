using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AlgoritmExport.Lib;
using System.Data.Odbc;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        public class Com_Provider_ODBC : ProviderI
        {
            private Com _MyCom;
            private string _ConnectString;

            /// <summary>
            /// Тип провайдеоа
            /// </summary>
            /// <returns>Кастомизированный тип провайдера</returns>
            public Provider_En GetTyp()
            {
                return Provider_En.MSSQL;
            }

            /// <summary>
            /// Текущая строка подключения
            /// </summary>
            /// <returns></returns>
            public string ConnectString()
            {
                return this._ConnectString;
            }

            // Текущие параметры подключения
            public string ODBC_DSN
            {
                get { try { return (new OdbcConnectionStringBuilder(this._ConnectString)).Dsn; } catch { return null; } }
                private set { }
            }
            public string ODBC_User
            {
                get
                {
                    try
                    {
                        object rez;
                        (new OdbcConnectionStringBuilder(this._ConnectString)).TryGetValue("Uid", out rez);
                        return rez.ToString();
                    }
                    catch { return null; }
                }
                private set { }
            }
            public string ODBC_Password
            {
                get
                {
                    try
                    {
                        object rez;
                        (new OdbcConnectionStringBuilder(this._ConnectString)).TryGetValue("Pwd", out rez);
                        return rez.ToString();
                    }
                    catch { return null; }
                }
                private set { }
            }

            /// <summary>
            /// Сохранение строки подключения
            /// </summary>
            public event EventHandler<onComProviderArg> onSaveConnectStr;      // Сохранение строки подключения

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="com">Основной объект</param>
            public Com_Provider_ODBC(Com MyCom)
            {
                this._MyCom = MyCom;

                if (this._MyCom.SystemEvent("Загрузка провайдера: " + Provider_En.ODBC.ToString(), MyEvent.Com_Provider))
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
                OdbcConnectionStringBuilder sBildOdbc = new OdbcConnectionStringBuilder(ConnectStr);

                if (!string.IsNullOrWhiteSpace(sBildOdbc.Dsn)) throw new ApplicationException("Не указан DSN");
                object rez;
                if (!string.IsNullOrWhiteSpace((new OdbcConnectionStringBuilder(this._ConnectString)).TryGetValue("Uid", out rez).ToString()))  throw new ApplicationException("Не указан логин");
                if (!string.IsNullOrWhiteSpace((new OdbcConnectionStringBuilder(this._ConnectString)).TryGetValue("Pwd", out rez).ToString()))  throw new ApplicationException("Не указан пароль");

                try
                {
                    using (OdbcConnection conOdbc = new OdbcConnection(ConnectStr))
                    {
                        string ver = conOdbc.ServerVersion;

                        if (!string.IsNullOrWhiteSpace(ver)) return ConnectStr;
                    }
                }
                catch (Exception ex) { throw new ApplicationException(ex.Message); }

                return null;
            }
            /// <summary>
            /// Проверка строки подключения
            /// </summary>
            /// <param name="DSN">DSN</param>
            /// <param name="User">Пользователь</param>
            /// <param name="Password">Пароль</param>
            /// <returns>Успешная строка подключения</returns>
            public string TestCon(string DSN, string User, string Password)
            {
                OdbcConnectionStringBuilder sBildOdbc = new OdbcConnectionStringBuilder();
                sBildOdbc.Dsn = DSN;
                sBildOdbc.Add("Uid", User);
                sBildOdbc.Add("Pwd", Password);

                return TestCon(sBildOdbc.ConnectionString);
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
            /// Сохранение строки подключения
            /// </summary>
            /// <param name="DSN">DSN</param>
            /// <param name="User">Пользователь</param>
            /// <param name="Password">Пароль</param>
            /// <returns>Успех сохранения строки подключения</returns>
            public bool SaveConnectStr(string DSN, string User, string Password)
            {
                OdbcConnectionStringBuilder sBildOdbc = new OdbcConnectionStringBuilder();
                sBildOdbc.Dsn = DSN;
                sBildOdbc.Add("Uid", User);
                sBildOdbc.Add("Pwd", Password);

                return SaveConnectStr(sBildOdbc.ConnectionString);
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

                    using (OdbcConnection con = new OdbcConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OdbcCommand com = new OdbcCommand(Sql, con))
                        {
                            using (OdbcDataReader read = com.ExecuteReader())
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
                    }

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

                    if (Sql == null || Sql.Trim() == String.Empty) return rez;

                    using (OdbcConnection con = new OdbcConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OdbcCommand com = new OdbcCommand(Sql, con))
                        {
                            using (OdbcDataReader read = com.ExecuteReader())
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

                    if (Sql == null || Sql.Trim() == String.Empty) return rez;

                    using (OdbcConnection con = new OdbcConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OdbcCommand com = new OdbcCommand(Sql, con))
                        {
                            using (OdbcDataReader read = com.ExecuteReader())
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

                    if (Sql == null || Sql.Trim() == String.Empty) return rez;

                    using (OdbcConnection con = new OdbcConnection(this._ConnectString))
                    {
                        con.Open();

                        using (OdbcCommand com = new OdbcCommand(Sql, con))
                        {
                            using (OdbcDataReader read = com.ExecuteReader())
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
            /// <param name="SorceQUERY">Запрос для получения настроек почтового сервиса</param>
            public void SetThreadCurrentWorkflow(Com_Thread Thr, string SorceQUERY, string SourceEmailQuery)
            {
                using (OdbcConnection con = new OdbcConnection(this._ConnectString))
                {
                    con.Open();

                    // Если есть запрос для получения информации по отправке писем то парсим этот запрос
                    if (!string.IsNullOrWhiteSpace(SourceEmailQuery))
                    {
                        using (OdbcCommand com = new OdbcCommand(SourceEmailQuery, con))
                        {
                            using (OdbcDataReader read = com.ExecuteReader())
                            {
                                if (read.HasRows)
                                {
                                    while (read.Read())
                                    {
                                        string SmtpServer = null;
                                        int SmtpPort = 0;
                                        string SmtpUser = null;
                                        string SmtpPassword = null;
                                        string To = null;
                                        string From = null;
                                        string Subject = null;
                                        string Body = null;
                                        string CHCP = null;
                                        bool SSL = false;

                                        for (int i = 0; i < read.FieldCount; i++)
                                        {
                                            if (read.GetName(i).ToUpper() == @"SMTPSERVER" && !read.IsDBNull(i)) SmtpServer = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"SMTPPORT" && !read.IsDBNull(i))
                                            {
                                                try { SmtpPort = int.Parse(read.GetValue(i).ToString()); }
                                                catch (Exception) { }
                                            }
                                            if (read.GetName(i).ToUpper() == @"SMTPUSER" && !read.IsDBNull(i)) SmtpUser = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"SMTPPASSWORD" && !read.IsDBNull(i)) SmtpPassword = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"TO" && !read.IsDBNull(i)) To = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"FROM" && !read.IsDBNull(i)) From = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"SUBJECT" && !read.IsDBNull(i)) Subject = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"BODY" && !read.IsDBNull(i)) Body = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"CHCP" && !read.IsDBNull(i)) CHCP = read.GetString(i);
                                            if (read.GetName(i).ToUpper() == @"SSL" && !read.IsDBNull(i))
                                            {
                                                try { if (int.Parse(read.GetValue(i).ToString()) != 0) SSL = true; }
                                                catch (Exception) { }
                                            }
                                        }

                                        if (SmtpServer == null || SmtpServer.Trim() == string.Empty)
                                            throw new ApplicationException(@"В запросе задачи (" + Thr.CurrentWorkflow.LongName + ") не указан сервер SMTP отправить письмо будет невозможно.");
                                        if ((To == null || To.Trim() == string.Empty))
                                            throw new ApplicationException(@"В запросе задачи (" + Thr.CurrentWorkflow.LongName + ") не указано кому отпралять письмо.");



                                        // Добавляем объекты нашей задачи
                                        MySmtpEmail newSmtpEmail;
                                        newSmtpEmail = new MySmtpEmail(_MyCom, SmtpServer, SmtpPort, SmtpUser, SmtpPassword, To, From, Subject, Body, CHCP, SSL);

                                        Thr.CurrentWorkflow.ListSmtpServer.Add(newSmtpEmail);
                                    }
                                }
                            }
                        }
                    }

                    // Получаем инфу по заданиям
                    using (OdbcCommand com = new OdbcCommand(SorceQUERY, con))
                    {
                        using (OdbcDataReader read = com.ExecuteReader())
                        {
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    string QUERY = null;
                                    string FILE_PATH = null;
                                    string COL_SPAN = null;
                                    string ROW_SPAN = null;
                                    string LIST_NAME = null;
                                    string SpecificFormat = null;
                                    string SpecialColumnName = null;
                                    bool Append = false;

                                    for (int i = 0; i < read.FieldCount; i++)
                                    {
                                        if (read.GetName(i).ToUpper() == @"QUERY" && !read.IsDBNull(i)) QUERY = read.GetString(i);
                                        if (read.GetName(i).ToUpper() == @"FILE_PATH" && !read.IsDBNull(i)) FILE_PATH = read.GetString(i);
                                        if (read.GetName(i).ToUpper() == @"COL_SPAN" && !read.IsDBNull(i)) COL_SPAN = read.GetString(i);
                                        if (read.GetName(i).ToUpper() == @"ROW_SPAN" && !read.IsDBNull(i)) ROW_SPAN = read.GetString(i);
                                        if (read.GetName(i).ToUpper() == @"LIST_NAME" && !read.IsDBNull(i)) LIST_NAME = read.GetString(i);
                                        if (read.GetName(i).ToUpper() == @"SPECIFICFORMAT" && !read.IsDBNull(i)) SpecificFormat = read.GetString(i);
                                        if (read.GetName(i).ToUpper() == @"SPECIALCOLUMNNAME" && !read.IsDBNull(i)) SpecialColumnName = read.GetString(i);
                                        if (read.GetName(i).ToUpper() == @"APPEND" && !read.IsDBNull(i))
                                        {
                                            try { Append = bool.Parse(read.GetString(i)); }
                                            catch (Exception) { }
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
                        }
                    }
                    con.Close();
                }
            }

            /// <summary>
            /// Установка задяния в переданном потоке
            /// </summary>
            /// <param name="Thr">Асинхронный поток</param>
            public void RunCurrentTask(Com_Thread Thr, MyTask Task, bool HachNotColName)
            {
                using (OdbcConnection con = new OdbcConnection(this._ConnectString))
                {
                    con.Open();

                    using (OdbcCommand com = new OdbcCommand(Task.QUERY, con))
                    {
                        using (OdbcDataReader read = com.ExecuteReader())
                        {
                            List<string> ColumnName = null;
                            // Если это первая строка и указано, что нужно сохранть имена колонок, то делаем это
                            if (Task.RowAffweted == 0 &&
                                    (Thr.CurrentWorkflow.HachColName || (Task.SpecificFormat != null && Task.SpecificFormat.Trim() != string.Empty)) &&
                                    !HachNotColName
                               )
                            {
                                ColumnName = new List<string>();
                                for (int i = 0; i < read.FieldCount; i++)
                                {
                                    ColumnName.Add(read.GetName(i));
                                }
                                // Сохраняем и сбрасываем строку
                                Thr.SetRowRez(Task, ColumnName, true, ColumnName);
                            }

                            List<string> rez = null;
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    rez = new List<string>();

                                    for (int i = 0; i < read.FieldCount; i++)
                                    {
                                        if (!read.IsDBNull(i)) rez.Add(read.GetValue(i).ToString());
                                        else rez.Add(null);
                                    }

                                    // Сохраняем строку
                                    Thr.SetRowRez(Task, rez, false, ColumnName);
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
        }
    }
}
