using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System.Data.SqlClient;
using System.Web.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace CustomerSupport.Class
{
    public class Utilities
    {
        public bool SendMail(string strfromAddres,string strtoAddress, string strBody, string strSubject,  string strfromPassword, string strhost,int intPort) 
        {
            try
            {
                var message = new MimeMessage();
                string[] subsstrnameFrom = strfromAddres.Split('@');
                string strnameFrom = "";

                if (subsstrnameFrom.Length > 0)
                    strnameFrom = subsstrnameFrom[0];

                string[] subsstrtoAddress = strtoAddress.Split('@');
                string strnamestrtoAddress = "";

                if (subsstrtoAddress.Length > 0)
                    strnamestrtoAddress = subsstrtoAddress[0];

                message.From.Add(new MailboxAddress(strnameFrom, strfromAddres));
                message.To.Add(new MailboxAddress(strnamestrtoAddress, strtoAddress));

                message.Subject = strSubject;

                message.Body = new TextPart("html") { Text = strBody };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.CheckCertificateRevocation = false;
                    client.Connect(strhost, intPort, false);
                    client.Authenticate(strfromAddres, strfromPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}