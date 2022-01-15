using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Mail;
using System.Net.Mime;

namespace AlgoritmExport.Common
{
    /// <summary>
    /// Класс для предоставления информации для отправки на почту нужного нам сообщения
    /// </summary>
    public class MySmtpEmail
    {
        private Com _MyCom;
        /// <summary>
        /// Почтовый клиент
        /// </summary>
        private SmtpClient smtp = null;

        public string SmtpServer { get; private set; }
        public int SmtpPort = 25;
        public string SmtpUser { get; private set; }
        public string SmtpPassword { get; private set; }
        public string To { get; private set; }
        public string From { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string CHCP { get; private set; }
        public Encoding EnCHCP
        {
            get
            {
                Encoding rez = Encoding.Default;
                try
                {
                    rez = Encoding.GetEncoding(int.Parse(this.CHCP));
                }
                catch
                {
                    try
                    {
                        rez = Encoding.GetEncoding(this.CHCP);
                    }
                    catch
                    {
                        rez = Encoding.Default;
                    }
                }

                return rez;
            }
            private set { }
        }
        public bool SSL = false;

        /// <summary>
        /// Событие отпраки сообщения
        /// </summary>
        public event EventHandler<EvenvSmtpEmail> onEvenvSendMail;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="MyCom"></param>
        /// <param name="SmtpServer"></param>
        /// <param name="SmtpPort"></param>
        /// <param name="SmtpUser"></param>
        /// <param name="SmtpPassword"></param>
        /// <param name="To"></param>
        /// <param name="From"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        public MySmtpEmail(Com MyCom, string SmtpServer, int SmtpPort, string SmtpUser, string SmtpPassword, string To, string From, string Subject, string Body, string CHCP, bool SSL)
        {
            this._MyCom = MyCom;
            this.SmtpServer = SmtpServer;
            if (SmtpPort > 0) this.SmtpPort = SmtpPort;
            this.SmtpUser = SmtpUser;
            this.SmtpPassword = SmtpPassword;
            this.To = To;
            this.From = From;
            this.Subject = Subject;
            this.Body = Body;
            this.CHCP = CHCP;
            this.SSL = SSL;

            // Создаём клиента для отправки сообщений
            smtp = new SmtpClient(this.SmtpServer, this.SmtpPort);
            if (!string.IsNullOrWhiteSpace(this.SmtpUser) && !string.IsNullOrWhiteSpace(this.SmtpPassword))
            {
                smtp.Credentials = new System.Net.NetworkCredential(SmtpUser, SmtpPassword);
            }
            smtp.EnableSsl = this.SSL;

            /*
            switch (DeliveryMethod)
            {
                case "Network":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    break;
                case "PickupDirectoryFromIis":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                    break;
                case "SpecifiedPickupDirectory":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    break;
                default:
                    break;
            }
            */
        }

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="ContextPath">Пути к файлам которые нужно вложить в сообщения в качестве контекста</param>
        public void SendEmail(List<string> ContextPath)
        {
            try
            {
                // Создаём сообщение
                MailMessage mail = new MailMessage();

                // Создаём вложения в письмо
                foreach (string item in ContextPath)
                {
                    Attachment attachData = new Attachment(item);
                    mail.Attachments.Add(attachData);
                }
                /*
                //Пробегаем по вложенным файлам
                string htmlBody = null;
                foreach (string item in ContextPath)
                {
                    // Создаём ресурс
                    LinkedResource inline = null;
                    inline = new LinkedResource(item, "xlsx");
                    if (inline != null) inline.ContentId = Guid.NewGuid().ToString();


                    // Оборачиваем в теги
                    if (htmlBody == null) htmlBody = @"<html><body>" + (inline == null ? this.Body : this.Body.Replace("@ContMessage", inline.ContentId + " @ContMessage")) + @"</body></html>";
                    else htmlBody = (inline == null ? htmlBody : htmlBody.Replace("@ContMessage", inline.ContentId + " @ContMessage"));

                    // Создаём представление
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                    if (inline != null) avHtml.LinkedResources.Add(inline);

                    // Добавляем представление в сообщение
                    mail.AlternateViews.Add(avHtml);
                }
                */

                // Заполняем реквизитами письмо
                //
                if (!string.IsNullOrWhiteSpace(this.CHCP)) mail.HeadersEncoding = this.EnCHCP;
                mail.From = new MailAddress(ConvertEncoding(Encoding.Default, this.EnCHCP, this.From));
                //
                foreach (string item in this.To.Split(','))
                {
                    mail.To.Add(new MailAddress(ConvertEncoding(Encoding.Default, this.EnCHCP, item.Trim())));
                }
                //
                if (!string.IsNullOrWhiteSpace(this.CHCP)) mail.SubjectEncoding = this.EnCHCP;
                mail.Subject = ConvertEncoding(Encoding.Default, this.EnCHCP, this.Subject);
                //
                if (!string.IsNullOrWhiteSpace(this.CHCP)) mail.BodyEncoding = this.EnCHCP;
                mail.Body = ConvertEncoding(Encoding.Default, this.EnCHCP, this.Body);


                // Oтправляем Письмо
                smtp.Send(mail);

                // Собственно обработка события
                EvenvSmtpEmail myArg = new EvenvSmtpEmail (this, Com.MyEvent.Success);
                if (onEvenvSendMail != null)
                {
                    onEvenvSendMail.Invoke(this, myArg);
                }

                // Логируем в лог факт отправки письма
                this._MyCom.SystemEvent(string.Format(@"Сообщение успешно отправлено на адресс {0} с темой сообщения: {1} и текстом: {2}", this.To, this.Subject, this.Body), myArg.Status);
            }
            catch (SmtpException ex)
            {
                ex.Source += this.ToString();

                // Обработка события
                EvenvSmtpEmail myArg = new EvenvSmtpEmail(this, Com.MyEvent.ERROR);
                myArg.Message = string.Format("{0} ({1})", ex.Message, (ex.InnerException!=null ? ex.InnerException.Message:null));
                if (onEvenvSendMail != null)
                {
                    onEvenvSendMail.Invoke(this, myArg);
                } 

                // Логируем в лог ошибку
                this._MyCom.SystemEvent(ex.Source + @": " + myArg.Message, myArg.Status);
            }
            catch (Exception ex)
            {
                ex.Source += this.ToString();

                // Обработка события
                EvenvSmtpEmail myArg = new EvenvSmtpEmail(this, Com.MyEvent.ERROR);
                myArg.Message = ex.Message;
                if (onEvenvSendMail != null)
                {
                    onEvenvSendMail.Invoke(this, myArg);
                }

                // Логируем в лог ошибку
                this._MyCom.SystemEvent(ex.Source + @": " + myArg.Message, myArg.Status);
            }
        }



        /// <summary>
        /// Конвертация в нужную кодировку
        /// </summary>
        /// <param name="EnSource">Кодировка источника</param>
        /// <param name="enTarget">Кодировка приёмника</param>
        /// <param name="Data">Данные которые нужно перекодировать</param>
        /// <returns>Перекодированные данные</returns>
        private string ConvertEncoding(Encoding EnSource, Encoding enTarget, string Data)
        {
            // если преобразовывать не нужно то просто возвращаем результат
            if (EnSource == enTarget) return Data;

            byte[] tmpSource = EnSource.GetBytes(Data);
            byte[] tmpTarget = Encoding.Convert(EnSource, enTarget, tmpSource);
            return enTarget.GetString(tmpTarget);
        }
    }
}
