using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;

namespace Group14_BevoBooks.Utilities
{
    public static class EmailMessaging
    {
        public static void SendEmail(String toEmailAddress, String emailSubject, String emailBody)
        {
            //Create an email client to send the emails
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                //This is the SENDING emailing address and password
                Credentials = new NetworkCredential("team14bevobooks@gmail.com", "Password123!!"),
                EnableSsl = true
            };
            //Add anything that you need to the body of the message
            String finalMessage = emailBody + "\n\n This is a disclaimer that will be on all messages.";

            //Create an email address object for the sender address
            MailAddress senderEmail = new MailAddress("team14bevobooks@gmail.com", "Team14_BevoBook");
            MailMessage mm = new MailMessage();
            mm.Subject = "Team 14 - " + emailSubject;
            mm.Sender = senderEmail;
            mm.From = senderEmail;
            mm.To.Add(new MailAddress(toEmailAddress));
            mm.Body = finalMessage;
            client.Send(mm);
        }
    }
}

//so we have this method (Send Email) -- do we create new methods for specific functions?
//and then call them in the specfic controller (Account, Order, etc?)
//Also do we install any packages like using System.Net.Mime;
//are we supposed to have a model class and this is our controller?