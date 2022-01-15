using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgoritmExport.Lib;
using Oracle.DataAccess.Client;

namespace AlgoritmExport.Common 
{
    public partial class Com
    {
        public class Com_Provider : ProviderI
        {
            private Com _MyCom;
            private ProviderI _Provider;

            /// <summary>
            /// Кастомизированный провайдер
            /// </summary>
            public ProviderI Provider
            {
                get {return this._Provider;}
                private set {}
            }

            /// <summary>
            /// Тип провайдеоа
            /// </summary>
            /// <returns>Кастомизированный тип провайдера</returns>
            public Provider_En GetTyp()
            {
                try
                {
                    return this._Provider.GetTyp();
                }
                catch (Exception)
                {
                    return Provider_En.Empty;
                }        
            }

            /// <summary>
            /// Текущая строка подключения
            /// </summary>
            /// <returns></returns>
            public string ConnectString()
            {
                if (this._Provider != null && this.GetTyp() != Provider_En.Empty) return this._Provider.ConnectString();
                else return null;
            } 

            /// <summary>
            /// Системные события провайдера
            /// </summary>
            public event EventHandler<onComProviderArg> onSaveConnectStr;      // Сохранение строки подключения

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="com">Основной объект</param>
            /// <param name="com">Провайдер с которым работаем</param>
            public Com_Provider(Com MyCom)
            {
                this._MyCom = MyCom;

                if (this._MyCom.SystemEvent("Загрузка провайдера", MyEvent.Com_Provider))
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
            /// Устанавливаем свой провайдер
            /// </summary>
            /// <param name="Provider">Любой провайдер наследующий интерфейс Com_ProviderI</param>
            public void SetProvider(ProviderI Provider)
            {
                this._Provider = Provider;
            }

            /// <summary>
            /// Проверка строки подключения
            /// </summary>
            /// <param name="ConnectStr">Строка подключения</param>
            /// <returns>Успешная строка подключения</returns>
            public string TestCon(string ConnectStr)
            {
                return this._Provider.TestCon(ConnectStr);
            }

            /// <summary>
            /// Сохранение строки подключения
            /// </summary>
            /// <param name="ConnectStr">Строка подключения</param>
            /// <returns>Успех сохранения строки подключения</returns>
            public bool SaveConnectStr(string ConnectStr)
            {
                bool rez;

                rez=this._Provider.SaveConnectStr(ConnectStr);

                onComProviderArg myArg = new onComProviderArg(this);

                if (onSaveConnectStr != null)
                {
                    onSaveConnectStr.Invoke(this, myArg);
                }

                return rez;
            }

            /// <summary>
            /// Получаем значение по умолчанию для DefaultRezDt
            /// </summary>
            /// <param name="Sql">Запрос для получения значения по умолчанию</param>
            /// <returns>Значение по умолчанию</returns>
            public List<MyParamForComboBox> GetListForComboBox(string Sql)
            {
                return this._Provider.GetListForComboBox(Sql);  
            }

            /// <summary>
            /// Получаем значение по умолчанию для DefaultRezDt
            /// </summary>
            /// <param name="Sql">Запрос для получения значения по умолчанию</param>
            /// <returns>Значение по умолчанию</returns>
            public DateTime? GetDefaultRezDt(string Sql)
            {
                return this._Provider.GetDefaultRezDt(Sql);
            }

            /// <summary>
            /// Получаем значение по умолчанию для DefaultRezStr
            /// </summary>
            /// <param name="Sql">Запрос для получения значения по умолчанию</param>
            /// <returns>Значение по умолчанию</returns>
            public string GetDefaultRezStr(string Sql)
            {
                return this._Provider.GetDefaultRezStr(Sql);
            }

            /// <summary>
            /// Получаем значение по умолчанию для DefaultRezLst
            /// </summary>
            /// <param name="Sql">Запрос для получения значения по умолчанию</param>
            /// <returns>Значение по умолчанию</returns>
            public List<string> GetDefaultRezLst(string Sql)
            {
                return this._Provider.GetDefaultRezLst(Sql);
            }


            /// <summary>
            /// Установка задяния в переданном потоке
            /// </summary>
            /// <param name="Thr">Асинхронный поток</param>
            /// <param name="SorceQUERY">Запрос для получения списка заданий</param>
            /// <param name="SorceQUERY">Запрос для получения настроек почтового сервиса</param>
            public void SetThreadCurrentWorkflow(Com_Thread Thr, string SorceQUERY, string SourceEmailQuery)
            {
                this._Provider.SetThreadCurrentWorkflow(Thr, SorceQUERY, SourceEmailQuery);
            }

            /// <summary>
            /// Установка задяния в переданном потоке
            /// </summary>
            /// <param name="Thr">Асинхронный поток</param>
            /// <param name="Task">Запрос задания</param>
            /// <param name="HachNotColName">Признак о том что бодавлять заголовок не нужно он уже существует</param>
            public void RunCurrentTask(Com_Thread Thr, MyTask Task, bool HachNotColName)
            {
                this._Provider.RunCurrentTask(Thr, Task, HachNotColName);
            }
        }
    }
}
