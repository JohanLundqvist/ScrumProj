using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ScrumProj.Models;
using System.Threading.Tasks;
using System.Threading;

namespace ScrumProj.Controllers
{
    public class MailController : Controller
    {
        
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }
        public async Task SendEmail(EmailFormModel model, List<string> ToMail)
        {
            var body = "<p>Email From: {0} </p><p>{2}</p><p>{1}</p>";
            var message = new MailMessage();
            foreach (var m in ToMail)
            {              
                message.To.Add(new MailAddress(m));
            }
            if (message.To == null)
                return;
            message.From = new MailAddress("scrumcgrupptvanelson@outlook.com");
            message.Subject = "Du har en ny notis i Nelson Administration";
            message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "scrumcgrupptvanelson@outlook.com",
                    Password = "hej12345"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
        
    }
}