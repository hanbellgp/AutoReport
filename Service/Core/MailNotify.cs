using System;
using System.Net.Mail;
using System.Net;

namespace Hanbell.AutoReport.Core
{
    public class MailNotify : Notify
    {

        protected string mailserver;
        protected int port;
        protected MailAddress from;
        protected MailMessage mail;

        public Notification content { get; set; }

        public MailNotify()
        {
            mailserver = Base.GetMailServerAddress();
            port = int.Parse(Base.GetMailServerPort());
            from = new MailAddress(Base.GetMailFrom());
        }

        protected override void Init()
        {

            mail = new MailMessage();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Subject = this.content.subject;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Body = this.content.content;
            mail.From = from;

            foreach (var item in this.content.to.Values)
            {
                mail.To.Add(item.ToString());
            }

            foreach (var item in this.content.cc.Values)
            {
                mail.CC.Add(item.ToString());
            }

            foreach (var item in this.content.bcc.Values)
            {
                mail.Bcc.Add(item.ToString());
            }

            foreach (var item in this.content.att.Values)
            {
                Attachment data = new Attachment(item.ToString());
                mail.Attachments.Add(data);
            }

        }

        protected override void Send()
        {
            try
            {
                SmtpClient client = new SmtpClient(mailserver, port);
                NetworkCredential myNC = new NetworkCredential(Base.GetMailAccount(), Base.GetMailAccountPassword());
                CredentialCache myCC = new CredentialCache();
                myCC.Add(mailserver, port, "NTLM", myNC);
                if (Base.GetMailAccount() != "" && Base.GetMailAccountPassword() !="")
                {
                    client.Credentials = myCC.GetCredential(mailserver, port, "NTLM");
                }
                else
                {
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void SendInfo(Notification content)
        {
            this.content = content;
            Init();
            Send();
        }

    }
}
