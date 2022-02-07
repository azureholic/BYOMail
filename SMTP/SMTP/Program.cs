using System;
using System.Net;
using System.Net.Mail;

namespace SMTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = "<smtp hostname>";
            var port = 587; //SSL Port
            var username = "<username auth smtp server>";
            var password = "<password auth smtp server>";
            var toAddress = "<mail to>";

            SmtpClient cl = new SmtpClient();
            cl.Host = host;
            cl.Port = port;
            cl.EnableSsl = true;

            NetworkCredential cred = new NetworkCredential(username, password);
            cl.Credentials = cred;

            MailMessage msg = new MailMessage();
            msg.Subject = "test";
            msg.Body = "hello world";
            msg.From = new MailAddress(username);
            msg.To.Add(toAddress);
            cl.Send(msg);
            Console.WriteLine("Mail Send");
            Console.ReadLine();

        }
    }
}
