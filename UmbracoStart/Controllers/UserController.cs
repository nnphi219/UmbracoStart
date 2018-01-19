using System.Collections.Generic;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using UmbracoStart.Models.User;
using Umbraco.Core;
using Umbraco.Core.Services;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using UmbracoStart.Global;
using UmbracoStart.PetaPoco.Entity;
using Umbraco.Core.Persistence;

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

            var users = _contentService.GetChildren(CurrentPage.Id).Where(x => x.ContentType.Alias == AliasName.USER).ToList();

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

            IPublishedContent homePage = CurrentPage.AncestorOrSelf("home");
            IPublishedContent userManagement = homePage.Children.Where(x => x.DocumentTypeAlias == AliasName.USER_MANAGEMENT).FirstOrDefault();
            var users2 = userManagement.Children.ToList();

            //foreach (var user in users2)
            //{
            //    model.Add(new UserViewModel
            //    {
            //        Id = user.Id,
            //        FirstName = user.GetPropertyValue<string>("firstName"),
            //        LastName = user.GetPropertyValue<string>("lastName"),
            //        EmailAddress = user.GetPropertyValue<string>("emailAddress")
            //    });
            //}

            var db = ApplicationContext.DatabaseContext.Database;
            //var ppUser = new PPUserEntity
            //{
            //    FirstName = "Hung",
            //    LastName = "Pham",
            //    EmailAddress = "hungpham@gmail.com"
            //};
            
            //db.Insert(ppUser);
            var query = new Sql().From("PetaPocoUser");
            var ppUsers = db.Fetch<PPUserEntity>(query);

            //foreach (var user in ppUsers)
            //{
            //    model.Add(new UserViewModel
            //    {
            //        Id = user.Id,
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //        EmailAddress = user.EmailAddress
            //    });
            //}

            return PartialView(PartialViewPath("_Users"), model);
        }

        public ActionResult RenderForm()
        {
            return PartialView(PartialViewPath("_UserForm"));
        }
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userContent = _contentService.GetById(model.Id);
                if(userContent == null)
                {
                    userContent = _contentService.CreateContent(string.Format("{0} {1}", model.FirstName, model.LastName),
                                                            CurrentPage.Id, AliasName.USER);
                }
                SaveUser(userContent, model);

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
                    _contentService.UnPublish(user);
                    _contentService.Delete(user);

                }
            }
            
            return Redirect("/user-management");
        }

        private void SaveUser(IContent userContent, UserViewModel user)
        {
            userContent.SetValue("firstName", user.FirstName);
            userContent.SetValue("lastName", user.LastName);
            userContent.SetValue("emailAddress", user.EmailAddress);

            _contentService.SaveAndPublish(userContent);
        }

        private void CreateUser(UserViewModel user)
        {
            var userContent = _contentService.CreateContent(string.Format("{0} {1}", user.FirstName, user.LastName),
                                                            CurrentPage.Id, AliasName.USER);
            SaveUser(userContent, user);
        }

        private void UpdateUser(UserViewModel user)
        {
            var userContent = _contentService.GetById(user.Id);
            if (userContent != null)
            {
                SaveUser(userContent, user);
            }
        }
    }
}