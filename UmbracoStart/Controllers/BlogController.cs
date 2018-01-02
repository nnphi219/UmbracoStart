using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using UmbracoStart.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Linq;

namespace UmbracoStart.Controllers
{
    public class BlogController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Blog/";

        public ActionResult RenderPostList(int numberOfItems)
        {
            List<BlogPreview> model = new List<BlogPreview>();
            IPublishedContent blogPage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias == "blog").FirstOrDefault();

            foreach (IPublishedContent page in blogPage.Children.OrderByDescending(x => x.UpdateDate).Take(numberOfItems))
            {
                var mediaItem = page.GetPropertyValue<IPublishedContent>("articleImage");/* page.GetPropertyValue<int>("image");*/
                string imageUrl = mediaItem.Url;
                model.Add(new BlogPreview(page.Name, page.GetPropertyValue<string>("articleIntro"), imageUrl, page.Url));
            }

            return PartialView(PARTIAL_VIEW_FOLDER + "_PostList.cshtml", model);
        }
    }
}