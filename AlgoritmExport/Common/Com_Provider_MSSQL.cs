using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgoritmExport.Lib;
using System.Data.SqlClient;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        public class Com_Provider_MSSQL : ProviderI
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
            public string server
            {
                get { try { return (new SqlConnectionStringBuilder(this._ConnectString)).DataSource; } catch { return null; } }
                private set { }
            }
            public bool integratedSecurity
            {
                get { try { return (new SqlConnectionStringBuilder(this._ConnectString)).IntegratedSecurity; } catch { return false; } }
                private set { }
            }
            public string user
            {
                get { try { return (integratedSecurity?null:(new SqlConnectionStringBuilder(this._ConnectString)).UserID); } catch { return null; } }
                private set { }
            }
            public string password
            {
                get { try { return (integratedSecurity?null:(new SqlConnectionStringBuilder(this._ConnectString)).Password); } catch { return null; } }
                private set { }
            }
            public string database
            {
                get { try { return (new SqlConnectionStringBuilder(this._ConnectString)).InitialCatalog; } catch { return null; } }
                private set { }
            }

            /// <summary>
            /// Сохранение строки подключения
            /// </summary>
            public event EventHandler<onComProviderArg> onSaveConnectStr;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="com">Основной объект</param>
            public Com_Provider_MSSQL(Com MyCom)
            {
                this._MyCom = MyCom;

                if (this._MyCom.SystemEvent("Загрузка провайдера: " + Provider_En.MSSQL.ToString(), MyEvent.Com_Provider))
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
                SqlConnectionStringBuilder sBildOra = new SqlConnectionStringBuilder(ConnectStr);

                if (sBildOra.DataSource == null || sBildOra.DataSource == string.Empty)
                    throw new ApplicationException("Не указан сервер");
                if (!sBildOra.IntegratedSecurity)
                {
                    if (sBildOra.UserID == null || sBildOra.UserID == string.Empty)
                        throw new ApplicationException("Не указан логин");
                    if (sBildOra.Password == null || sBildOra.Password == string.Empty)
                        throw new ApplicationException("Не указан пароль");
                }

                try
                {
                    using (SqlConnection conSql = new SqlConnection(ConnectStr))
                    {
                        using (SqlCommand comSql = new SqlCommand("Select 1 As A", conSql))
                        {
                            conSql.Open();

                            using (SqlDataReader readSql = comSql.ExecuteReader())
                            {
                                if (readSql.HasRows)
                                {
                                    return ConnectStr;
                                }
                            }
                            conSql.Close();
                        }
                    }
                }
                catch (Exception ex) { throw new ApplicationException(ex.Message); }

                return null;
            }
            /// <summary>
            /// Проверка строки подключения
            /// </summary>
            /// <param name="server">TNS</param>
            /// <param name="integratedSecurity">Доменная авторизация</param>
            /// <param name="user">Пользователь</param>
            /// <param name="password">Пароль</param>
            /// <returns>Успешная строка подключения</returns>
            public string TestCon(string server, bool integratedSecurity, string user, string password)
            {
                SqlConnectionStringBuilder sBildSql = new SqlConnectionStringBuilder();
                sBildSql.DataSource = server;
                sBildSql.IntegratedSecurity = integratedSecurity;
                if (!integratedSecurity)
                {
                    sBildSql.UserID = user;
                    sBildSql.Password = password;
                }

                return TestCon(sBildSql.ConnectionString);
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
            /// <param name="server">Сервер</param>
            /// <param name="integratedSecurity">Доменная авторизация</param>
            /// <param name="user">Пользователь</param>
            /// <param name="password">Пароль</param>
            /// <param name="password">Базаданных</param>
            /// <returns>Успех сохранения строки подключения</returns>
            public bool SaveConnectStr(string server, bool integratedSecurity, string user, string password, string database)
            {
                SqlConnectionStringBuilder sBildSql = new SqlConnectionStringBuilder();
                sBildSql.DataSource = server;
                sBildSql.IntegratedSecurity = integratedSecurity;
                if (!integratedSecurity)
                {
                    sBildSql.UserID = user;
                    sBildSql.Password = password;
                }
                if (database!=null && database.Trim() != string.Empty) sBildSql.InitialCatalog = database;

                return SaveConnectStr(sBildSql.ConnectionString);
            }

            /// <summary>
            /// Получаем список доступных баз данных
            /// </summary>
            /// <param name="server">Сервер</param>
            /// <param name="integratedSecurity">Доменная авторизация</param>
            /// <param name="user">Пользователь</param>
            /// <param name="password">Пароль</param>
            /// <returns></returns>
            public List<string> GetDatabases(string server, bool integratedSecurity, string user, string password)
            {
                List<string> rez = new List<string>();

                try
                {
                    SqlConnectionStringBuilder sBildSql = new SqlConnectionStringBuilder();
                    sBildSql.DataSource = server;
                    sBildSql.IntegratedSecurity = integratedSecurity;
                    if (!integratedSecurity)
                    {
                        sBildSql.UserID = user;
                        sBildSql.Password = password;
                    }

                    return GetDatabases(sBildSql.ConnectionString);
                }
                catch (Exception){}
                return rez;
            }

            /// <summary>
            /// Получаем список доступных баз данных
            /// </summary>
            /// <param name="ConString">Строка подключения если null то использует текущую</param>
            /// <returns></returns>
            public List<string> GetDatabases(string ConString)
            {
                List<string> rez = new List<string>();

                try
                {
                    using (SqlConnection con = new SqlConnection(ConString != null && ConString.Trim() != string.Empty?ConString:this.ConnectString()))
                    {
                        con.Open();

                        using (SqlCommand com = new SqlCommand(@"Select name From sys.databases Where database_id>4", con))
                        {
                            using (SqlDataReader read = com.ExecuteReader())
                            {
                                if (read.HasRows)
                                {
                                    while (read.Read())
                                    {
                                        // Получаем значение
                                        for (int i = 0; i < read.FieldCount; i++)
                                        {
                                            if (!read.IsDBNull(i)) rez.Add(read.GetString(i));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception) { }
                return rez;
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

                    using (SqlConnection con = new SqlConnection(this._ConnectString))
                    {
                        con.Open();

                        using (SqlCommand com = new SqlCommand(Sql, con))
                        {
                            using (SqlDataReader read = com.ExecuteReader())
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

                    using (SqlConnection con = new SqlConnection(this._ConnectString))
                    {
                        con.Open();

                        using (SqlCommand com = new SqlCommand(Sql, con))
                        {
                            using (SqlDataReader read = com.ExecuteReader())
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

                    using (SqlConnection con = new SqlConnection(this._ConnectString))
                    {
                        con.Open();

                        using (SqlCommand com = new SqlCommand(Sql, con))
                        {
                            using (SqlDataReader read = com.ExecuteReader())
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

                    using (SqlConnection con = new SqlConnection(this._ConnectString))
                    {
                        con.Open();

                        using (SqlCommand com = new SqlCommand(Sql, con))
                        {
                            using (SqlDataReader read = com.ExecuteReader())
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
            /// <param name="SourceEmailQuery">Запрос для получения настроек почтового сервиса</param>
            public void SetThreadCurrentWorkflow(Com_Thread Thr, string SorceQUERY, string SourceEmailQuery)
            {
                using (SqlConnection con = new SqlConnection(this._ConnectString))
                {
                    con.Open();

                    // Если есть запрос для получения информации по отправке писем то парсим этот запрос
                    if (!string.IsNullOrWhiteSpace(SourceEmailQuery))
                    {
                        using (SqlCommand com = new SqlCommand(SourceEmailQuery, con))
                        {
                            using (SqlDataReader read = com.ExecuteReader())
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

                    using (SqlCommand com = new SqlCommand(SorceQUERY, con))
                    {
                        using (SqlDataReader read = com.ExecuteReader())
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
                using (SqlConnection con = new SqlConnection(this._ConnectString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(Task.QUERY, con))
                    {
                        using (SqlDataReader read = com.ExecuteReader())
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
