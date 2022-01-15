using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoritmExport.Common
{
    /// <summary>
    /// Для события отправки письма
    /// </summary>
    public class EvenvSmtpEmail : EventArgs
    {
        public MySmtpEmail Email;
        public Com.MyEvent Status;
        public string Message;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Email">Сообщение которое отправилось</param>
        /// <param name="Status">Статус с которым отправилось</param>
        public EvenvSmtpEmail(MySmtpEmail Email, Com.MyEvent Status)
        {
            this.Email = Email;
            this.Status = Status;
        }
    }
}
