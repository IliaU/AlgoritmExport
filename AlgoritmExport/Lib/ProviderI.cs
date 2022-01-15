using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgoritmExport.Common;

namespace AlgoritmExport.Lib
{
    public interface ProviderI
    {
        /// <summary>
        /// Тип провайдеоа
        /// </summary>
        /// <returns>Кастомизированный тип провайдера</returns>
        Provider_En GetTyp();

        /// <summary>
        /// Текущая строка подключения
        /// </summary>
        /// <returns>Подключение в виде строки</returns>
        string ConnectString();

        /// <summary>
        /// Сохранение строки подключения
        /// </summary>
        event EventHandler<Common.Com.onComProviderArg> onSaveConnectStr;

        /// <summary>
        /// Проверка строки подключения
        /// </summary>
        /// <param name="ConnectStr">Строка подключения</param>
        /// <returns>Успешная строка подключения</returns>
        string TestCon(string ConnectStr);

        /// <summary>
        /// Сохранение строки подключения
        /// </summary>
        /// <param name="ConnectStr">Строка подключения</param>
        /// <returns>Успех сохранения строки подключения</returns>
        bool SaveConnectStr(string ConnectStr);

        /// <summary>
        /// Получаем список для комбобокса
        /// </summary>
        /// <param name="Sql">Запрос к базе</param>
        /// <returns>результат</returns>
        List<MyParamForComboBox> GetListForComboBox(string Sql);

        /// <summary>
        /// Получаем значение по умолчанию для DefaultRezDt
        /// </summary>
        /// <param name="Sql">Запрос для получения значения по умолчанию</param>
        /// <returns>Значение по умолчанию</returns>
        DateTime? GetDefaultRezDt(string Sql);


        /// <summary>
        /// Получаем значение по умолчанию для DefaultRezStr
        /// </summary>
        /// <param name="Sql">Запрос для получения значения по умолчанию</param>
        /// <returns>Значение по умолчанию</returns>
        string GetDefaultRezStr(string Sql);

        /// <summary>
        /// Получаем значение по умолчанию для DefaultRezLst
        /// </summary>
        /// <param name="Sql">Запрос для получения значения по умолчанию</param>
        /// <returns>Значение по умолчанию</returns>
        List<string> GetDefaultRezLst(string Sql);

        /// <summary>
        /// Установка задяния в переданном потоке
        /// </summary>
        /// <param name="Thr">Асинхронный поток</param>
        /// <param name="SorceQUERY">Запрос для получения списка заданий</param>
        /// <param name="SourceEmailQuery">Запрос для получения настроек почтового сервиса</param>
        void SetThreadCurrentWorkflow(Com.Com_Thread Thr, string SorceQUERY, string SourceEmailQuery);


        /// <summary>
        /// Установка задяния в переданном потоке
        /// </summary>
        /// <param name="Thr">Асинхронный поток</param>
        /// <param name="Task">Запрос задания</param>
        /// <param name="HachNotColName">Признак о том что бодавлять заголовок не нужно он уже существует</param>
        void RunCurrentTask(Com.Com_Thread Thr, MyTask Task, bool HachNotColName);
    }
}
