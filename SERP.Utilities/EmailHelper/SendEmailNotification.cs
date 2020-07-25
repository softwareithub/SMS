﻿using Microsoft.AspNetCore.Hosting;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Text;

namespace SERP.Utilities.EmailHelper
{
    public class SendEmailNotification
    {
        public List<Tuple<string, string>> SendOnlineTestEmail( List<Tuple<string, string>> studentIds,Tuple<string, string> fromEmail,
            string subject, IHostingEnvironment hostingEnviroment)
        {
            List<Tuple<string, string>> exceptionList = new List<Tuple<string, string>>();
            foreach (var item in studentIds)
            {
                try
                {
                  var response=  SendEmail(fromEmail, subject, hostingEnviroment, item.Item1, item.Item2);
                }
                catch(Exception ex)
                {
                    exceptionList.Add(new Tuple<string, string>(item.Item1, ex.Message));
                    continue;
                }
               
            }
            return exceptionList;
        }

        private bool SendEmail(Tuple<string, string> fromEmail, string subject, IHostingEnvironment hostingEnviroment, string stduentName, string studentEmail)
        {
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress(fromEmail.Item1, fromEmail.Item2);
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress(stduentName, studentEmail);
            message.To.Add(to);
            message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = GetMailBody(hostingEnviroment);
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("bhaweshdeepak@gmail.com", "vi@pra91");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();

            return true;
        }

        public string GetMailBody(IHostingEnvironment hostEnvironment)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(hostEnvironment.ContentRootPath + "/OnlineTestNotification.html"))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }
    }
}
