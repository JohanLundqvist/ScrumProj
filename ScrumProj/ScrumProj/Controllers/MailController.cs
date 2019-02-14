using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }
        //public void SendEmail(string recipientmail, string frommail, string subject, string message)
        //{
        //    var body = "<p>Email From: {0} </p><p>Message:</p><p>{1}</p>";
        //    var message = new MailMessage();
        //    message.To.Add(new MailAddress(recipient));
        //    message.From = new MailAddress(frommail);
        //    message.Subject = subject;
        //    message.Body = string.Format(body, frommail, message);
        //    message.IsBodyHtml = true;

        //    using (var smtp = new SmtpClient())
        //    {
        //        var credential = new NetworkCredential
        //        {
        //            UserName = "user@outlook.com",  // replace with valid value
        //            Password = "password"  // replace with valid value
        //        };
        //        smtp.Credentials = credential;
        //        smtp.Host = "smtp-mail.outlook.com";
        //        smtp.Port = 587;
        //        smtp.EnableSsl = true;
        //        await smtp.SendMailAsync(message);
        //    }
        //}
    }
}