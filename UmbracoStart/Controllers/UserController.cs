using System.Collections.Generic;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using UmbracoStart.Models.User;
using Umbraco.Core;
using Umbraco.Core.Services;
using System.Linq;
using UmbracoStart.Models;

namespace UmbracoStart.Controllers
{
    public class UserController : SurfaceController
    {
        IContentService _contentService;

        public UserController()
        {
            _contentService = ApplicationContext.Current.Services.ContentService;
        }

        private string PartialViewPath(string name)
        {
            return $"~/Views/Partials/User/{name}.cshtml";
        }
        
        public ActionResult RenderUsers()
        {
            List<UserViewModel> model = new List<UserViewModel>();

            var users = _contentService.GetChildren(CurrentPage.Id).ToList();

            foreach (var user in users)
            {
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.GetValue<string>("firstName"),
                    LastName = user.GetValue<string>("lastName"),
                    EmailAddress = user.GetValue<string>("emailAddress")
                });
            }
            
            return PartialView(PartialViewPath("_Users"), model);
        }

        public ActionResult RenderForm()
        {
            return PartialView(PartialViewPath("_UserForm"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                SaveUser(model);
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }
        
        public ActionResult DeleteUser(int? id)
        {
            if(id != null)
            {
                var user = _contentService.GetById((int)id);
                if (user != null)
                {
                    _contentService.Delete(user);
                }
                
                
            }

            
            return Redirect("/user-management");
        }

        private void SaveUser(UserViewModel user)
        {
            var contentUser = _contentService.CreateContent(string.Format("{0} {1}", user.FirstName, user.LastName),
                                                            CurrentPage.Id, AliasName.USER);
            contentUser.SetValue("firstName", user.FirstName);
            contentUser.SetValue("lastName", user.LastName);
            contentUser.SetValue("emailAddress", user.EmailAddress);

            _contentService.SaveAndPublish(contentUser);
        }
    }
}