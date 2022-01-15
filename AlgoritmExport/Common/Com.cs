using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgoritmExport.Lib;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        #region Properties
            public string CurrentDirectory { get; private set; }
            public string FileLog { get; private set; }
            public int IOCountPoput { get; private set; }      // Колличество попуток доступа к файлу, если он недоступен
            public int IOWhileInt { get; private set; }        // Колличество милесекунд между попытками, если файл недоступен 
            public string DefColSpan { get; private set; }     // Разделитель по умолчанию для колонок
            public string DefRowSpan { get; private set; }     // Разделитель по умолчанию для строк
            public Lic Lic;
            public DocXML MyConfig;

            /// <summary>
            /// Системные события
            /// </summary>
            public event EventHandler<onSysteming> onEventonSysteming;
            public event EventHandler<onSystem> onEventonSystem;

            // внутренние классы
            private MyEventConverter ConvertMyEvent = new MyEventConverter();
            public ProviderI Provider;
            public Com_SprWorkflow SprWorkflow;
            public Com_Thread Threader;
            public Com_SprParam SprParam;
        #endregion

        #region Public metod

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="CurrentDirectory">Текущая директория</param>
            /// <param name="IOCountPoput">Количество попыток доступа к файлам</param>
            /// <param name="IOWhileInt">Промежуток между доступами</param>
            public Com(string CurrentDirectory, int IOCountPoput, int IOWhileInt)
            {
                this.CurrentDirectory = CurrentDirectory;
                this.FileLog = this.CurrentDirectory + @"\Log.txt";
                this.IOCountPoput = IOCountPoput;
                this.IOWhileInt = IOWhileInt;
                this.DefColSpan = @"\t";
                this.DefRowSpan = @"\r\n";
                if (this.SystemEvent("Старт программы", MyEvent.Stаrting))
                {
                    try
                    {
                        // Загружаем объект провайдера
                        this.Provider = new Com_Provider(this);

                        // Объект который выполняет поставленную задачу
                        this.Threader = new Com_Thread(this);

                        // Создаём список наших заданий по которым мы потом сможем запускать процессы выгрузки
                        this.SprWorkflow = new Com_SprWorkflow(this);

                        // Создаём список наших параметров по которым мы потом сможем запускать процессы выгрузки
                        this.SprParam = new Com_SprParam(this);

                        // Инициализация модуля лицензирования
                        this.Lic = new Lic(this, "AlgoritmExport");

                        // Тестирование процесса шифрования и расшифровки
                        //String ss = this.Lic.EnCoding("AB-C");
                        //String sd = this.Lic.DeCoding(ss);


                        // Читаем файл конфигурации
                        MyConfig = new DocXML(this, "Config.xml");

                    }
                    catch (Exception ex)
                    {
                        EventSave("Ошибка при загрузке конструктора MyCom: " + ex.Message, MyEvent.SystemError);

                        SystemEvent(ex.Message, this.ConvertMyEvent.Convert(ex.Source));
                        Dispose();
                        throw ex;
                    }
                }
            }
            /// <summary>
            /// Коснтруктор
            /// </summary>
            /// <param name="CurrentDirectory">Текущая директория</param>
            public Com(string CurrentDirectory) : this(CurrentDirectory, 5, 400) { }


            /// <summary>
            /// Убиваем наш объект
            /// </summary>
            public void Dispose()
            {
                // Собственно выгрузка приложения
                if (SystemEvent("Закрытие программы", MyEvent.Stopping))
                {
                    Application.Exit();
                }
            }

        #endregion

        #region Private metod
            /// <summary>
            /// Для блокировки потока записывающего в файл лога StreamWriter, чтобы не работало асинхронно
            /// </summary>
            private object locSwFileLog = new object();
            /// <summary>
            /// Метод для записи информации в лог
            /// </summary>
            /// <param name="str"></param>
            /// <param name="evn"></param>
            /// <param name="IOCountPoput"></param>
            private void EventSave(string Message, MyEvent evn, int IOCountPoput)
            {
                // Сохраняем задание
                try
                {
                    lock (locSwFileLog)
                    {
                        using (StreamWriter SwFileLog = new StreamWriter(this.FileLog, true))   //,Encoding.Unicode
                        {
                            SwFileLog.WriteLine(DateTime.Now.ToString() + "\t" + evn.ToString() + "\t" + Message);
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (IOCountPoput > 0)
                    {
                        Thread.Sleep(this.IOWhileInt);
                        EventSave(Message, evn, IOCountPoput - 1);
                    }
                    else
                        throw ex;
                }
            }
            /// <summary>
            /// Событие клиента
            /// </summary>
            /// <param name="Message">Сообщение события</param>
            private void EventSave(string Message, MyEvent evn)
            {
                this.EventSave(Message, evn, this.IOCountPoput);
            }

            private void SetDefRolColSpan(string ColSpan, string RowSpan)
            {
                this.DefColSpan = ColSpan;
                this.DefRowSpan = RowSpan;
            }
        #endregion

        #region Event
            /// <summary>
            /// Произошло системное событие
            /// </summary>
            /// <param name="Message">Сообщение</param>
            /// <param name="evn">Тип события</param>
            public bool SystemEvent(String Message, MyEvent evn)
            {
                onSysteming MyArgsIng = new onSysteming (Message, evn);
                onSystem MyArgs = new onSystem (Message, evn);

                if (onEventonSysteming != null)
                {
                    onEventonSysteming.Invoke(this, MyArgsIng);
                }

                if (MyArgsIng.Action)
                {
                    EventSave(Message, evn, this.IOCountPoput);

                    if (onEventonSystem != null)
                    {
                        onEventonSystem.Invoke(this, MyArgs);
                    }
                    return true;
                }

                return false;
            }
        #endregion
    }
}
