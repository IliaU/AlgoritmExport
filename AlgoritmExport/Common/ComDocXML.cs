using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.IO;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        // Класс для работы с файлами XML
        public class DocXML
        {
            private Com _MyCom;
            private int _Version = 3;
            public string DocName { get; private set; }

            /// <remarks>Версия XML файла</remarks>
            public int Version { get { return this._Version; } private set { } }
            /// <remarks>Путь к файлу конфигурации</remarks>
            public string file { get; private set; }
            /// <remarks>Объект XML</remarks>
            private XmlDocument Document;

            /// <summary>
            /// Список подгруженный из нашего файла
            /// </summary>
            private List<Lic.LicEventKey> myKeys = new List<Lic.LicEventKey>();

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="com">Основной объект</param>
            /// <param name="DocName"></param>
            public DocXML(Com MyCom, string DocName)
            {
                this._MyCom = MyCom;
                this.DocName = DocName;
                this.file = this._MyCom.CurrentDirectory + @"\" + DocName;
                this.Document = new XmlDocument();
                this.Version = 2;

                if (this._MyCom.SystemEvent("Обработка конфигурационного файла", MyEvent.DocXmlConfig))
                {
                    ApplicationException app = new ApplicationException("Не известен файл в котором нужно искать настройки");
                    try
                    {
                        // Проверка входного параметра
                        if (file == null || file == string.Empty) throw app;

                        // Читаем файл или создаём
                        if (File.Exists(file)) { Load(); }
                        else { Create(); }

                        // Создаём костомизированный объект
                        GetDate();

                        // Подписываемся на события
                        this._MyCom.Provider.onSaveConnectStr += new EventHandler<onComProviderArg>(Provider_onSaveConnectStr);
                    }
                    catch (Exception ex)
                    {
                        ex.Source += MyEvent.DocXmlConfigError.ToString();
                        throw ex;
                    }
                }
            }

            /// <summary>
            /// Запись новой лицензии в настроечный файл
            /// </summary>
            /// <param name="?"></param>
            public void SetLic(Lic.LicEventKey LicEventKey)
            {

                XmlElement root = this.Document.DocumentElement;

                bool IsHashLic = false;
                bool IsRepid = false;
                foreach (XmlElement item in root.ChildNodes)
                {
                    if (item.Name == "Lic")
                    {
                        IsHashLic = true;

                        // Собственно сами элементы внутри нашего источника объектов для MS SQL
                        XmlElement xmlKey = this.Document.CreateElement("Key");
                        xmlKey.SetAttribute("MachineName", LicEventKey.MachineName);
                        xmlKey.SetAttribute("UserName", LicEventKey.UserName);
                        xmlKey.SetAttribute("ValidToYYYYMMDD", LicEventKey.ValidToYYYYMMDD.ToString());
                        xmlKey.SetAttribute("LicKey", LicEventKey.LicKey);
                        if (LicEventKey.ActivNumber != null) { xmlKey.SetAttribute("ActivNumber", LicEventKey.ActivNumber); }
                        if (LicEventKey.Info != null) { xmlKey.SetAttribute("Info", LicEventKey.Info); }
                        xmlKey.SetAttribute("HashUserOS", LicEventKey.HashUserOS.ToString());
                        item.AppendChild(xmlKey);
                    }
                }

                if (!IsHashLic)
                {
                    // Считываем элемент лицензии
                    XmlElement xmlLic = this.Document.CreateElement("Lic");
                    root.AppendChild(xmlLic);
                    IsRepid = true;
                }

                Save();

                // если нет контейнера для хранения лицензий, то создаём его и пробуем сохранить повторно
                if (IsRepid) SetLic(LicEventKey);
            }

            /// <summary>
            /// Установка параметров провайдера
            /// </summary>
            /// <param name="Provider"></param>
            public void SetProvider(Lib.ProviderI Provider)
            {
                XmlElement root = this.Document.DocumentElement;
                root.SetAttribute("ProviderTyp", Enum.GetName(typeof(Lib.Provider_En), Provider.GetTyp()));
                root.SetAttribute("ConnectionString", Provider.ConnectString());
                this.Save();
            }
            void Provider_onSaveConnectStr(object sender, onComProviderArg e)
            {
                SetProvider(e.Provider);
            }

            /// <summary>
            /// Чтение в ОЗУ
            /// </summary>
            private void Load()
            {
                try
                {
                    this.Document.Load(this.file);
                }
                catch (Exception ex) { throw ex; }
            }

            /// <remarks>Создание объекта</remarks>
            private void Create()
            {
                try
                {
                    string TekYear = DateTime.Now.Year.ToString();

                    // Создаём строку инициализации
                    XmlElement wbRoot = this.Document.DocumentElement;
                    XmlDeclaration wbxmldecl = this.Document.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    this.Document.InsertBefore(wbxmldecl, wbRoot);

                    // Создаём начальное тело в кторое будем потом всё вставлять
                    XmlElement xmlMain = this.Document.CreateElement("Algoritm_Export");
                    xmlMain.SetAttribute("Version", this.Version.ToString());
                    xmlMain.SetAttribute("DefColSpan", this._MyCom.DefColSpan);
                    xmlMain.SetAttribute("DefRowSpan", this._MyCom.DefRowSpan);
                    xmlMain.SetAttribute("ProviderTyp", Enum.GetName(typeof(Lib.Provider_En), Lib.Provider_En.Oracle));
                    xmlMain.SetAttribute("ConnectionString", "USER ID=monitoring;PASSWORD=ware22mon;DATA SOURCE=DWMON");
                    this.Document.AppendChild(xmlMain);

                    // Создаём элемент лицензии
                    XmlElement xmlLic = this.Document.CreateElement("Lic");
                    xmlMain.AppendChild(xmlLic);

                    // Создаём список доступных параметров
                    XmlElement xmlParams = this.Document.CreateElement("Params");
                    xmlMain.AppendChild(xmlParams);


                    XmlElement xmlParam1 = this.Document.CreateElement("Param");
                    xmlParam1.SetAttribute("Name", "From");
                    xmlParam1.SetAttribute("Caption", "C");
                    xmlParam1.SetAttribute("Typ", @"Calendar");
                    xmlParam1.SetAttribute("Format", "DD.MM.YYYY");
                    xmlParam1.SetAttribute("Default", "Select Sysdate As ID from dual");
                    //xmlParam1.InnerText = "";
                    xmlParams.AppendChild(xmlParam1);

                    XmlElement xmlParam2 = this.Document.CreateElement("Param");
                    xmlParam2.SetAttribute("Name", "To");
                    xmlParam2.SetAttribute("Caption", "По");
                    xmlParam2.SetAttribute("Typ", @"Calendar");
                    xmlParam2.SetAttribute("Format", "DD.MM.YYYY");
                    xmlParam2.SetAttribute("Default", "Select Sysdate As ID from dual");
                    //xmlParam1.InnerText = "";
                    xmlParams.AppendChild(xmlParam2);

                    XmlElement xmlParam3 = this.Document.CreateElement("Param");
                    xmlParam3.SetAttribute("Name", "Product");
                    xmlParam3.SetAttribute("Caption", "Продукт");
                    xmlParam3.SetAttribute("Typ", @"ComboBox");
                    xmlParam3.SetAttribute("Default", "Select '2' As ID from dual");
                    xmlParam3.InnerText = @"Select '-1' ID, 'Все модели' As TXT From dual union Select '1', 'Ford Galaxy' From dual union Select '2', 'Ford Fusion' From dual union Select '3', 'Dewo Matiz' From dual union Select '4', 'Иж Ода' From dual";
                    xmlParams.AppendChild(xmlParam3);

                    XmlElement xmlParam4 = this.Document.CreateElement("Param");
                    xmlParam4.SetAttribute("Name", "Month");
                    xmlParam4.SetAttribute("Caption", "Месяц");
                    xmlParam4.SetAttribute("ShablonSelected", "in ({''@id''})");
                    xmlParam4.SetAttribute("ShablonAll", "= To_Char(D,''DD.MM.YYYY'')");
                    xmlParam4.SetAttribute("Typ", @"ListBox");
                    xmlParam4.SetAttribute("Typ", @"Select '= To_Char(D,''''DD.MM.YYYY'''')' As ID from dual");
                    xmlParam4.InnerText = @"Select '01.01."+TekYear+@"' AS ID, 'Январь' AS TXT from dual
union Select '01.02."+TekYear+@"' AS ID, 'Февраль' AS TXT from dual
union Select '01.03."+TekYear+@"' AS ID, 'Март' AS TXT from dual";
                    xmlParams.AppendChild(xmlParam4);

                    // Создаём список наших заданий
                    XmlElement xmlWorkflows = this.Document.CreateElement("Workflows");
                    xmlMain.AppendChild(xmlWorkflows);

                    XmlElement xmlWorkflow1 = this.Document.CreateElement("Workflow");
                    xmlWorkflow1.SetAttribute("LongName", "Тестовая выгрузка с применением фильтра");
                    xmlWorkflow1.SetAttribute("ShortName", "tst");
                    xmlWorkflow1.SetAttribute("HachColName", true.ToString());
                    xmlWorkflow1.SetAttribute("CHCP", "866");
                    xmlWorkflow1.InnerText = @"Select 'with T As (Select 1 ID, To_Date(''01.01." + TekYear + @"'',''DD.MM.YYYY'') D, 4 As MODEL_ID, ''Иж Ода'' As MODEL_TXT From dual union Select 2, To_Date(''01.02." + TekYear + @"'',''DD.MM.YYYY''), 2,  ''Ford Fusion'' From dual union Select 3, To_Date(''01.03." + TekYear + @"'',''DD.MM.YYYY''), 1, ''Ford Galaxy'' From dual union Select 4, To_Date(''01.03." + TekYear + @"'',''DD.MM.YYYY''), 3, ''Dewo Matiz'' From dual)
Select *
From T
Where D between To_Date(''@From'',''DD.MM.YYYY'') and To_Date(''@To'',''DD.MM.YYYY'')
    and MODEL_ID=Case When @Product=''-1'' Then MODEL_ID Else @Product end' As QUERY, '@CurrentFolder\Export\My_Prod_chcp866.txt' As FILE_PATH, '\t' As COL_SPAN, '\r\n' As ROW_SPAN
From dual";
                    xmlWorkflows.AppendChild(xmlWorkflow1);


                    XmlElement xmlWorkflow2 = this.Document.CreateElement("Workflow");
                    xmlWorkflow2.SetAttribute("LongName", "Выгружаем всё");
                    xmlWorkflow2.SetAttribute("ShortName", "tst2");
                    xmlWorkflow2.SetAttribute("FILE_PATH_FOR_EXEL", @"@CurrentFolder\Export\ttt.xlsx");      // Пока не заказывали, но уже сделано
                    xmlWorkflow2.SetAttribute("HachColName", true.ToString());
                    xmlWorkflow2.InnerText = @"Select 'Select 1 ID, ''Ford Galaxy'' As MODEL From dual union Select 2, ''Ford Fusion'' From dual union Select 3, ''Dewo Matiz'' From dual union Select 4, ''Иж Ода'' From dual' As QUERY, 'Product' As LIST_NAME,  '@CurrentFolder\Export\Product.txt' As FILE_PATH, 'True' As ""APPEND"" From Dual
union 
Select 'Select 1 ID, To_Date(''01.01." + TekYear + @"'',''DD.MM.YYYY'') D, 4 As MODEL From dual union Select 2, To_Date(''01.02." + TekYear + @"'',''DD.MM.YYYY''), 2 From dual union Select 3, To_Date(''01.03." + TekYear + @"'',''DD.MM.YYYY''), 1 From dual union Select 4, To_Date(''01.03." + TekYear + @"'',''DD.MM.YYYY''), 3 From dual', 'Journal' As LIST_NAME,  '@CurrentFolder\Export\Journal.txt', 'False' As ""APPEND"" From Dual";
                    xmlWorkflows.AppendChild(xmlWorkflow2);

                    // Тест специального формата XML в MS SQL
                    XmlElement xmlWorkflow3 = this.Document.CreateElement("Workflow");
                    xmlWorkflow3.SetAttribute("LongName", "Выгружаем всё в специальном формате");
                    xmlWorkflow3.SetAttribute("ShortName", "tst3");
                    xmlWorkflow3.SetAttribute("HachColName", true.ToString());
                    xmlWorkflow3.InnerText = @"Select 'Select 1 ID, ''Ford Galaxy'' As MODEL union Select 2, ''Ford Fusion'' union Select 3, ''Dewo Matiz'' union Select 4, ''Иж Ода'' union Select 5, null ' As QUERY, 'Product' As LIST_NAME,  '@CurrentFolder\Export\Product.txt' As FILE_PATH, 'XML;<?xml version=""1.0"" encoding=""windows-1251""?>;Товары;Элемент' As SpecificFormat From Dual
union 
Select 'Select 1 ID, convert(date,''01.01.'' + Convert(varchar,DATEPART(Year,GetDate())), 104) D, 4 As MODEL union Select 2,convert(date,''01.02.'' + Convert(varchar,DATEPART(Year,GetDate())), 104), 2 union Select 3, convert(date,''01.03.'' + Convert(varchar,DATEPART(Year,GetDate())), 104), 1 union Select 4, convert(date,''01.04.'' + Convert(varchar,DATEPART(Year,GetDate())), 104), 3 ', 'Journal' As LIST_NAME,  '@CurrentFolder\Export\Journal.txt', null From Dual";
                    xmlWorkflows.AppendChild(xmlWorkflow3);


                    XmlElement xmlWorkflow4 = this.Document.CreateElement("Workflow");
                    xmlWorkflow4.SetAttribute("LongName", "Выгружаем всё в специальном формате2");
                    xmlWorkflow4.SetAttribute("ShortName", "tst4");
                    xmlWorkflow4.SetAttribute("HachColName", true.ToString());
                    xmlWorkflow4.SetAttribute("CHCP", "UTF-8");
                    xmlWorkflow4.InnerText = @"With T As (Select  '@CurrentFolder\Export\Specific2.xml' As FILE_PATH, 'XML2;<?xml version=""1.0"" encoding=""UTF-8""?>;<Data>
    <EcrConfigStoreHdr_T FileVerFormat=""256"" FileDataType=""8"" CodePageStr=""Windows-1251""/>
    <box Name=""PriceCodes"">;PrItemPriceCode_T;    </box>
</Data>' As SpecificFormat From Dual)
Select 'Select 1 ID, ''Ford Galaxy'' As MODEL From dual union Select 2, ''Ford Fusion'' From dual union Select 3, ''Dewo Matiz'' From dual union Select 4, ''Иж Ода'' From dual' As QUERY, 'Product' As LIST_NAME,  T.FILE_PATH, T.SpecificFormat, 'Nam1,Nam2' As SpecialColumnName From Dual, T";
                    xmlWorkflows.AppendChild(xmlWorkflow4);

                    XmlElement xmlWorkflow5 = this.Document.CreateElement("Workflow");
                    xmlWorkflow5.SetAttribute("LongName", "Выгружаем из RPro8 всех клиентов");
                    xmlWorkflow5.SetAttribute("ShortName", "tst5");
                    xmlWorkflow5.SetAttribute("HachColName", true.ToString());
                    xmlWorkflow5.SetAttribute("CHCP", "UTF-8");
                    xmlWorkflow5.InnerText = @"QUERY|Client.Dat!FILE_PATH|@CurrentFolder\Export\Client.txt!COL_SPAN|\t!ROW_SPAN|\r\n!LIST_NAME|Client^QUERY|INVN.DAT!FILE_PATH|@CurrentFolder\Export\Client2.txt!COL_SPAN|\t!ROW_SPAN|\r\n!LIST_NAME|Client2";
                    xmlWorkflows.AppendChild(xmlWorkflow5);

                    XmlElement xmlWorkflow6 = this.Document.CreateElement("Workflow");
                    xmlWorkflow6.SetAttribute("LongName", "Выгружаем всё и отправляем на мыло");
                    xmlWorkflow6.SetAttribute("ShortName", "tst2");
                    xmlWorkflow6.SetAttribute("FILE_PATH_FOR_EXEL", @"@CurrentFolder\Export\ttt.xlsx");      // Пока не заказывали, но уже сделано
                    xmlWorkflow6.SetAttribute("EmailQuery", @"Select 'smtp.server.ru' As SmtpServer, 0 As SmtpPort, null As SmtpUser, null As SmtpPassword, 'ilia82@mail.ru' As ""To"", 'ilia82@mail.ru' As ""From"", 'Тема' As Subject, 'Сообщение' As Body, null As CHCP, 0 As SSL From Dual");      // Запрос для отправки на мыло
                    xmlWorkflow6.SetAttribute("HachColName", true.ToString());
                    xmlWorkflow6.InnerText = @"Select 'Select 1 ID, ''Ford Galaxy'' As MODEL From dual union Select 2, ''Ford Fusion'' From dual union Select 3, ''Dewo Matiz'' From dual union Select 4, ''Иж Ода'' From dual' As QUERY, 'Product' As LIST_NAME,  '@CurrentFolder\Export\Product.txt' As FILE_PATH, 'True' As ""APPEND"" From Dual
union 
Select 'Select 1 ID, To_Date(''01.01." + TekYear + @"'',''DD.MM.YYYY'') D, 4 As MODEL From dual union Select 2, To_Date(''01.02." + TekYear + @"'',''DD.MM.YYYY''), 2 From dual union Select 3, To_Date(''01.03." + TekYear + @"'',''DD.MM.YYYY''), 1 From dual union Select 4, To_Date(''01.03." + TekYear + @"'',''DD.MM.YYYY''), 3 From dual', 'Journal' As LIST_NAME,  '@CurrentFolder\Export\Journal.txt', 'False' As ""APPEND"" From Dual";
                    xmlWorkflows.AppendChild(xmlWorkflow6);


                    // Сохранение в файл
                    Save();
                }
                catch (Exception ex) { throw ex; }
            }

            /// <remarks>Сохранение в файл</remarks>
            private void Save()
            {
                try
                {
                    this.Document.Save(this.file);
                }
                catch (Exception ex) { throw ex; }
            }

            /// <remarks>Получение костомизированных данных</remarks>
            private void GetDate()
            {
                ApplicationException appM = new ApplicationException("Неправильный настроечный файл, скорее всего не от этой программы.");
                ApplicationException appV = new ApplicationException("Неправильная версия настроечного файла, требуется " + this.Version.ToString() + " версия.");
                try
                {
                    XmlElement root = this.Document.DocumentElement;

                    // Проверяем тип файла настройки по имени коренвого нода и версию
                    if (root.Name != "Algoritm_Export") throw appM;
                    if (this.Version < int.Parse(root.GetAttribute("Version"))) { throw appV; }
                    if (this.Version > int.Parse(root.GetAttribute("Version"))) UpdateVersionXml(root, int.Parse(root.GetAttribute("Version")));

                    // Получаем значения из заголовка
                    string DefColSpan = null;
                    string DefRowSpan = null;
                    string ProviderTyp = Lib.Provider_En.Oracle.ToString();
                    string ConnectionString = null;
                    for (int i = 0; i < root.Attributes.Count; i++)
                    {
                        if (root.Attributes[i].Name == "DefColSpan") DefColSpan = root.Attributes[i].Value.ToString();
                        if (root.Attributes[i].Name == "DefRowSpan") DefRowSpan = root.Attributes[i].Value.ToString();
                        if (root.Attributes[i].Name == "ProviderTyp") ProviderTyp = root.Attributes[i].Value.ToString();
                        if (root.Attributes[i].Name == "ConnectionString") ConnectionString = root.Attributes[i].Value.ToString();
                    }
                    if (DefColSpan != null && DefRowSpan != null) this._MyCom.SetDefRolColSpan(DefColSpan, DefRowSpan);

                    switch (Lib.EnConverter.Convert(ProviderTyp, Lib.Provider_En.Empty))
                    {
                        case AlgoritmExport.Lib.Provider_En.Oracle:
                            // Устанавливаем строку подключения в объекте провайдера
                            if (ConnectionString != null && ConnectionString.Trim() != string.Empty)
                            {
                                try
                                {
                                    Com_Provider_Ora conOra = new Com_Provider_Ora(this._MyCom);
                                    conOra.SaveConnectStr(ConnectionString);
                                    this._MyCom.Provider = conOra;
                                }
                                catch (Exception) { }
                            }
                         break;
                        case AlgoritmExport.Lib.Provider_En.ODBC:
                            // Устанавливаем строку подключения в объекте провайдера
                            if (ConnectionString != null && ConnectionString.Trim() != string.Empty)
                            {
                                try
                                {
                                    Com_Provider_ODBC conOdbc = new Com_Provider_ODBC(this._MyCom);
                                    conOdbc.SaveConnectStr(ConnectionString);
                                    this._MyCom.Provider = conOdbc;
                                }
                                catch (Exception) { }
                            }
                            break;
                        case AlgoritmExport.Lib.Provider_En.MSSQL:
                            // Устанавливаем строку подключения в объекте провайдера
                            if (ConnectionString != null && ConnectionString.Trim() != string.Empty)
                             {
                                 try
                                 {
                                     Com_Provider_MSSQL conSql = new Com_Provider_MSSQL(this._MyCom);
                                     conSql.SaveConnectStr(ConnectionString);
                                     this._MyCom.Provider = conSql;
                                 }
                                 catch (Exception) { }
                             }
                         break;
                        case AlgoritmExport.Lib.Provider_En.RPro8:
                         // Устанавливаем строку подключения в объекте провайдера
                         if (ConnectionString != null && ConnectionString.Trim() != string.Empty)
                         {
                             try
                             {
                                 Com_Provider_RPro8 conRPro8 = new Com_Provider_RPro8(this._MyCom);
                                 conRPro8.SaveConnectStr(ConnectionString);
                                 this._MyCom.Provider = conRPro8;
                             }
                             catch (Exception) { }
                         }
                         break;
                        default:
                            new ApplicationException("Нет реализации для работы с провайдером: " + ProviderTyp);
                         break;
                    }

                    // Получаем список объектов
                    foreach (XmlElement item in root.ChildNodes)
                    {
                        if (item.Name == "Lic")
                        {

                            foreach (XmlElement licKey in item.ChildNodes)
                            {
                                if (licKey.Name == "Key")
                                {
                                    try
                                    {
                                        string MachineName = null;
                                        string UserName = null;
                                        string ActivNumber = null;
                                        string LicKey = null;
                                        int ValidToYYYYMMDD = 0;
                                        string Info = null;
                                        bool HashUserOS = false;

                                        //Получаем данные по параметру из файла
                                        for (int i = 0; i < licKey.Attributes.Count; i++)
                                        {
                                            if (licKey.Attributes[i].Name == "MachineName") { MachineName = licKey.Attributes[i].Value; }
                                            if (licKey.Attributes[i].Name == "UserName") { UserName = licKey.Attributes[i].Value; }
                                            if (licKey.Attributes[i].Name == "ActivNumber") { ActivNumber = licKey.Attributes[i].Value; }
                                            if (licKey.Attributes[i].Name == "LicKey") { LicKey = licKey.Attributes[i].Value; }
                                            if (licKey.Attributes[i].Name == "ValidToYYYYMMDD") { try { ValidToYYYYMMDD = int.Parse(licKey.Attributes[i].Value); } catch { } }
                                            if (licKey.Attributes[i].Name == "Info") { Info = licKey.Attributes[i].Value; }
                                            try{if (licKey.Attributes[i].Name == "HashUserOS") { HashUserOS = bool.Parse(licKey.Attributes[i].Value); }}
                                            catch (Exception){}
                                            
                                        }

                                        // Проверяем валидность подгруженного ключа
                                        if (LicKey!=null && this._MyCom.Lic.IsValidLicKey(LicKey))
                                        {
                                            // Если ключь валидный то сохраняем его в списке ключей
                                            Lic.LicEventKey newKey = new Lic.LicEventKey(MachineName, UserName, ActivNumber, LicKey, ValidToYYYYMMDD, Info, HashUserOS);
                                            this.myKeys.Add(newKey);
                                        }
                                    }
                                    catch { } // Если ключь прочитать не удалось или он не подходит, то исключения выдавать не нужно
                                }
                            }
                            // После пробега по всему списку ключей, вибираем тот у которого дата будет позже, по логике в списке будут все валидные ключи
                            int indexMaxYYMMDD = -1;
                            int itmp = -1;
                            for (int i = 0; i < this.myKeys.Count; i++)
                            {
                                if (this.myKeys[i].ValidToYYYYMMDD > indexMaxYYMMDD)
                                {
                                    indexMaxYYMMDD = this.myKeys[i].ValidToYYYYMMDD;
                                    itmp = i;
                                }
                            }
                            // Теперь регистрируем ключь который имеет большую дату
                            if (itmp > -1) this._MyCom.Lic.RegNewKey(this.myKeys[itmp]);
                        }


                        if (item.Name == "Params")
                        {
                            foreach (XmlElement iParams in item.ChildNodes)
                            {
                                if (iParams.Name == "Param")
                                {
                                    string Name=null;
                                    string Caption = null;
                                    string Typ = null;
                                    string Format = null;
                                    string InnerText = null;
                                    string ShablonSelected = null;
                                    string ShablonAll = null;
                                    string Default = null;

                                    //Получаем данные по параметру из файла
                                    for (int i = 0; i < iParams.Attributes.Count; i++)
                                    {
                                        if (iParams.Attributes[i].Name == "Name") { Name = iParams.Attributes[i].Value; }
                                        if (iParams.Attributes[i].Name == "Caption") { Caption = iParams.Attributes[i].Value; }
                                        if (iParams.Attributes[i].Name == "Typ") { Typ = iParams.Attributes[i].Value; }
                                        if (iParams.Attributes[i].Name == "Format") { Format = iParams.Attributes[i].Value; }
                                        if (iParams.Attributes[i].Name == "ShablonSelected") { ShablonSelected = iParams.Attributes[i].Value; }
                                        if (iParams.Attributes[i].Name == "ShablonAll") { ShablonAll = iParams.Attributes[i].Value; }
                                        if (iParams.Attributes[i].Name == "Default") { Default = iParams.Attributes[i].Value; }
                                    }

                                    if (iParams.InnerText!=string.Empty)
                                        InnerText = iParams.InnerText;

                                    // Проверка на корректность (Общая)
                                    if (Name != null && Name.Trim() != string.Empty && Caption != null && Caption.Trim() != string.Empty && Typ != null && Typ.Trim() != string.Empty)
                                    {
                                        // Проверка если тип календарь, то должен быть указан формат
                                        if (Typ == "Calendar" && Format != null && Format.Trim() != string.Empty)
                                        {
                                            MyParam Param = new MyParam(Name, Caption, Typ, Format, null, null, InnerText, Default);
                                            this._MyCom.SprParam.Add(Param);
                                        }

                                        // Проверка на корректрость если типа ComboBox
                                        if (Typ == "ComboBox")
                                        {
                                            MyParam Param = new MyParam(Name, Caption, Typ, null, null, null, InnerText, Default);
                                            this._MyCom.SprParam.Add(Param);
                                        }

                                        // Проверка на корректрость если типа ListBox
                                        if (Typ == "ListBox")
                                        {
                                            MyParam Param = new MyParam(Name, Caption, Typ, null, ShablonSelected, ShablonAll, InnerText, Default);
                                            this._MyCom.SprParam.Add(Param);
                                        }
                                    }
                                }
                            }
                        }

                        if (item.Name == "Workflows")
                        {
                            foreach (XmlElement iWorkflows in item.ChildNodes)
                            {
                                if (iWorkflows.Name == "Workflow")
                                {
                                    string LongName = null;
                                    string ShortName = null;
                                    string SqlQuery= null;
                                    string FILE_PATH_FOR_EXEL = null;
                                    string EmailQuery = null;
                                    bool HachColName = false;
                                    string CHCP = null;

                                    for (int i = 0; i < iWorkflows.Attributes.Count; i++)
                                    {
                                        if (iWorkflows.Attributes[i].Name == "LongName") { LongName = iWorkflows.Attributes[i].Value; }
                                        if (iWorkflows.Attributes[i].Name == "ShortName") { ShortName = iWorkflows.Attributes[i].Value; }
                                        if (iWorkflows.Attributes[i].Name == "FILE_PATH_FOR_EXEL") { FILE_PATH_FOR_EXEL = iWorkflows.Attributes[i].Value; }
                                        if (iWorkflows.Attributes[i].Name == "EmailQuery") { EmailQuery = iWorkflows.Attributes[i].Value; }
                                        try
                                        {
                                            if (iWorkflows.Attributes[i].Name == "HachColName") { HachColName  = bool.Parse(iWorkflows.Attributes[i].Value); }
                                        }
                                        catch (Exception ex)
                                        {
                                            ex.Source += MyEvent.DocXmlConfigError.ToString();
             
                                            this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.WARNING);
                                        }
                                        if (iWorkflows.Attributes[i].Name == "CHCP") { CHCP = iWorkflows.Attributes[i].Value; }
                                    }
                                    try { SqlQuery = iWorkflows.InnerText; }
                                    catch (Exception) { }

                                    // Проверка на корректность
                                    if (LongName != null && LongName.Trim() != string.Empty && ShortName != null && ShortName.Trim() != string.Empty && SqlQuery != null && SqlQuery.Trim() != string.Empty)
                                    {
                                        MyWorkflow Workflow = new MyWorkflow(SqlQuery, LongName, ShortName, FILE_PATH_FOR_EXEL, EmailQuery, HachColName, CHCP);
                                        //new_TreyContecstItem.onClickChengStatusMenuItemSave += new EventHandler<onChengStatusMenuItem>(new_TreyContecstItem_onClickChengStatusMenuItemSave);
                                        this._MyCom.SprWorkflow.Add(Workflow);
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex) 
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, MyEvent.ERROR);
                    throw ex;
                }
            }

            /// <summary>
            /// Oбновляем до текущей версии
            /// </summary>
            /// <param name="root">Корневой элемент</param>
            /// <param name="oldVersion">Текущая версия элемента</param>
            private void UpdateVersionXml(XmlElement root, int oldVersion)
            {
                try
                {
                    if (oldVersion <= 2)
                    {
                        string OraTNS = null;
                        string OraUser = null;
                        string OraPassword = null;
                        for (int i = 0; i < root.Attributes.Count; i++)
                        {
                            if (root.Attributes[i].Name == "OraTNS") OraTNS = root.Attributes[i].Value.ToString();
                            if (root.Attributes[i].Name == "OraUser") OraUser = root.Attributes[i].Value.ToString();
                            if (root.Attributes[i].Name == "OraPassword") OraPassword = root.Attributes[i].Value.ToString();
                        }

                        // Устанавливаем строку подключения в объекте провайдера
                        if (OraTNS != null && OraTNS.Trim() != string.Empty && OraUser != null && OraUser.Trim() != string.Empty && OraPassword != null && OraPassword.Trim() != string.Empty)
                        {
                            Com_Provider_Ora conOra = new Com_Provider_Ora(this._MyCom);
                            try
                            {
                                if (!conOra.SaveConnectStr(OraTNS, OraUser, OraPassword) || conOra.ConnectString() == null || conOra.ConnectString().Trim() == string.Empty) new ApplicationException("Не можем обновить конфиг файл так как подключение к ораклу невалидно.");
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException("Не можем обновить конфиг файл так как подключение к ораклу невалидно. (" + ex.Message + ")");
                            }

                            root.SetAttribute("ProviderTyp", Enum.GetName(typeof(Lib.Provider_En), Lib.Provider_En.Oracle));
                            root.SetAttribute("ConnectionString", conOra.ConnectString());
                            root.RemoveAttribute("OraTNS");
                            root.RemoveAttribute("OraUser");
                            root.RemoveAttribute("OraPassword");
                        }

                    }

                    root.SetAttribute("Version", this._Version.ToString());
                    this.Save();
                }
                catch (Exception ex)
                {
                    ex.Source += MyEvent.Com_Provider.ToString();

                    this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, MyEvent.ERROR);
                    throw ex;
                }

            }

        }
    }
}
