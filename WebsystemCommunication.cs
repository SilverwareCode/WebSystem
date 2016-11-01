using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

/// <summary>
/// Summary description for WebsystemCommunication
/// </summary>
/// 
namespace WebSystem
{

    public class Communication
    {
        public Communication()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool sendEmail(string mailTo, string subject, string messageHTML, string senderAccount, string senderPassword, string smtpServer, string senderAddress)
        {

            bool retVal = false;

            //sender - napr."web@my.cuesystem.com"
            //senderPassword - "CUE1991"
            //smtpServer - "mail4.aspone.cz"
            //senderAddress - "training@cuesystem.com"

            try
            {
            SmtpClient mailKlient = new SmtpClient();
            //tvorime instanci SMTP klienta
            MailMessage myEmail = new MailMessage();
            mailKlient.UseDefaultCredentials = false;
            mailKlient.Credentials = new System.Net.NetworkCredential(senderAccount, senderPassword);
            mailKlient.Port = 25;
            mailKlient.EnableSsl = false;
            mailKlient.Host = smtpServer;
            myEmail = new MailMessage();
            myEmail.From = new MailAddress(senderAddress);
            myEmail.To.Add(mailTo);
            myEmail.Subject = subject;
            myEmail.IsBodyHtml = true;
            myEmail.Body = messageHTML;
            mailKlient.Send(myEmail);
            retVal = true;
            }
            catch (Exception error_t)
            {
                retVal = false;
            }
            return retVal;
        }

        ///validaceEmailu
        public static bool correctEmail(string emailaddress)
        {
            bool success = false;
            bool val1 = false;
            bool val2 = false;
            bool val3 = false;
            //obsahuje zavináč
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                val1 = true;
            }
            catch (FormatException)
            {
                val1 = false;
            }
            //obsahuje tečku
            if (emailaddress.Contains('.'))
            {
                val2 = true;
            }
            else
            {
                val2 = false;
            }
            //obsahuje part za tečkou
            string domena = null;
            try
            {
                domena = WebSystem.Strings.splitString(emailaddress, ".", 1);
            }
            catch { }
            if (domena != "")
            {
                val3 = true;
            }
            else
            {
                val3 = false;
            }
            //shrnutí podmínek
            if ((val1) && (val2) && (val3))
            {
                success = true;
            }

            return success;
        }

        //validace reCaptcha Google použití .Net JavaScriptSerializer
        public class ReCaptcha
        {
            public bool Success { get; set; }
            public List<string> ErrorCodes { get; set; }

            public static bool Validate(string encodedResponse)
            {
                if (string.IsNullOrEmpty(encodedResponse)) return false;

                var client = new System.Net.WebClient();
                var secret = ConfigurationManager.AppSettings["Google.ReCaptcha.Secret"];

                if (string.IsNullOrEmpty(secret)) return false;

                var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, encodedResponse));

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                var reCaptcha = serializer.Deserialize<ReCaptcha>(googleReply);

                return reCaptcha.Success;
            }
        }

    }
}