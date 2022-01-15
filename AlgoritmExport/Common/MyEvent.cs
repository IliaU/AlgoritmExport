using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgoritmExport.Lib;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        public enum MyEvent
        {
            Stаrting, Stopping, ERROR, WARNING, SystemError, DocXmlConfigError, DocXmlConfig, Com_Provider, UserControl, Wokflow, SprWorkflow, Com_Thread, Param, Success
        }

        /// <summary>
        /// Класс для конвертации моих собственных событий
        /// </summary>
        public class MyEventConverter
        {
            private MyEvent _MyEvent;

            /// <summary>
            /// Конструктор
            /// </summary>
            public MyEventConverter() { }


            /// <summary>
            /// Получаем список событий в текстовом виде с указанным разделитеолем
            /// </summary>
            /// <param name="chr">Текстовый вариант нашего события</param>
            public string Convert(Char chr)
            {
                string MyEventSpisok = null;

                foreach (MyEvent item in Enum.GetValues(typeof(MyEvent)))
                {
                    if (MyEventSpisok != null) MyEventSpisok = MyEventSpisok + chr.ToString() + item.ToString();
                    else MyEventSpisok = item.ToString();
                }
                return MyEventSpisok;
            }

            /// <summary>
            /// Конвертация в объект MyEvent
            /// </summary>
            /// <param name="Event">Текстовый вариант нашего события</param>
            /// <returns>Возврашаем костомизированное сопытие из перечисления</returns>
            public MyEvent Convert(string Event) //:this ()
            {
                if (Event != null && Event.Trim() != string.Empty)
                {
                    foreach (MyEvent item in Enum.GetValues(typeof(MyEvent)))
                    {
                        if (item.ToString() == Event.Trim()) return item;
                    }
                }
                return this._MyEvent;
            }

        }


        /// <summary>
        /// Аргументы при сробатывании системных событий
        /// </summary>
        public class onSysteming : EventArgs
        {
            public Boolean Action;
            public String Message { get; private set; }
            public MyEvent Evn { get; private set; }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="Message">Сообщение</param>
            public onSysteming(String Message, MyEvent evn)
            {
                this.Evn = evn;
                this.Action = true;
            }
        }
        public class onSystem : EventArgs
        {
            public String Message { get; private set; }
            public MyEvent Evn { get; private set; }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="Message">Сообщение</param>
            public onSystem(String Message, MyEvent evn)
            {
                this.Evn = evn;
            }
        }
        public class onComProviderArg : EventArgs
        {
            public ProviderI Provider;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// /// <param name="Provider">Провайдер</param>
            public onComProviderArg(ProviderI Provider)
            {
                this.Provider = Provider;
            }
        }
        public class onTaskIteratorArg : EventArgs
        {
            public MyTask Task;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="MyTask">Объект, который добавляется</param>
            public onTaskIteratorArg(MyTask Task)
            {
                this.Task = Task;
            }
        }
        public class onWokflowIteratorArg : EventArgs
        {
            public MyWorkflow Wokflow;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// /// <param name="Provider">Провайдер</param>
            public onWokflowIteratorArg(MyWorkflow Wokflow)
            {
                this.Wokflow = Wokflow;
            }
        }
        public class onComThreadrArg : EventArgs
        {
            public Com.Com_Thread Thread;
            public string ErMessage;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// /// <param name="Provider">Провайдер</param>
            public onComThreadrArg(Com.Com_Thread Thread, string ErMessage)
            {
                this.Thread = Thread;
                this.ErMessage = ErMessage;
            }
        }

        public class onComThreadrRowAffwetedArg : EventArgs
        {
            public MyTask Task;
            public Int64 RowAffweted;
            public string Stat;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="MyTask">Объект, который добавляется</param>
            public onComThreadrRowAffwetedArg(MyTask Task, Int64 RowAffweted, string Stat)
            {
                this.Task = Task;
                this.RowAffweted = RowAffweted;
                this.Stat = Stat;
            }
        }
    }
}
