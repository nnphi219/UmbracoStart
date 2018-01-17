using Umbraco.Web.Mvc;
using System.Web.Mvc;
using UmbracoStart.Models.Contact;
using System.Net.Mail;
using Umbraco.Core;
using Umbraco.Core.Services;
using System;
using System.Linq;

namespace UmbracoStart.Controllers
{
    public class ContactController : SurfaceController
    {
        private string PartialViewPath(string name)
        {
            return $"~/Views/Partials/Contact/{name}.cshtml";
        }
        public ActionResult RenderForm()
        {
            return PartialView(PartialViewPath("_Contact"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ContactModelView model)
        {
            if (ModelState.IsValid)
            {
                //SendEmail(model);
                GetContact();
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        private void SendEmail(ContactModelView model)
        {
            MailMessage message = new MailMessage(model.EmailAddress, "51302922@hcmut.edu.vn");
            message.Subject = string.Format("Enquiry from {0} - {1}", model.Name, model.EmailAddress);
            message.Body = model.Message;
            SmtpClient client = new SmtpClient("127.0.0.1", 25);
            client.Send(message);
        }

        private void AddData(ContactModelView model)
        {
            var contentServiceContact = ApplicationContext.Current.Services.ContentService.CreateContent(
                    DateTime.Now.ToString("dd/MM/yyyy"), CurrentPage.Id, "contact"
                );
        }

        private void GetContact()
        {
            var contentServiceContact = ApplicationContext.Current.Services.ContentService.GetById(1054);
            var contentServiceContact2 = ApplicationContext.Current.Services.ContentService.GetChildren(1054);
            var list = contentServiceContact2.ToList();
        }
    }
}