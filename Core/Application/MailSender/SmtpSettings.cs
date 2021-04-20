using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.MailSender
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }


     /* 
     
          "SmtpSettings": {
            "Server": "mail.xx.com",
            "Port": 587,
            "SenderName": "Apparel Port",
            "SenderEmail": "no-reply@apparelport.com",
            "Username": "no-reply@apparelport.com",
            "Password": "xxxx"
          },
     
     
     */
}
