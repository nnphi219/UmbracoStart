using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbracoStart.Models;
using System.Collections.Generic;
using Umbraco.Web;
using Umbraco.Core.Models;
using System.Linq;
using Archetype.Models;

namespace UmbracoStart.Controllers
{
    public class HomeController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Home/";

        public ActionResult RenderIntro()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "_Intro.cshtml");
        }

        public ActionResult RenderFeatured()
        {
            List<FeaturedItem> model = new List<FeaturedItem>();
            IPublishedContent homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias == "home").FirstOrDefault();
            ArchetypeModel featuredItems = homePage.GetPropertyValue<ArchetypeModel>("featuredItems");

            foreach (ArchetypeFieldsetModel fieldset in featuredItems)
            {
                var mediaItem = fieldset.GetValue<IPublishedContent>("image");
                //int imageId = fieldset.GetValue<int>("image");
                //var mediaItem = Umbraco.Media(imageId);
                string imageUrl = mediaItem.Url;

                //int pageId = fieldset.GetValue<int>("page");
                //IPublishedContent linkedToPage = Umbraco.TypedContent(pageId);
                var linkedToPage = fieldset.GetValue<IPublishedContent>("page");
                string linkUrl = linkedToPage.Url;

                model.Add(new FeaturedItem(fieldset.GetValue<string>("name"), fieldset.GetValue<string>("category"), imageUrl, linkUrl));
            }

            return PartialView(PARTIAL_VIEW_FOLDER + "_Featured.cshtml", model);
        }

        public ActionResult RenderServices()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "_Services.cshtml");
        }

        public ActionResult RenderBlog()
        {
            IPublishedContent homepage = CurrentPage.AncestorOrSelf("home");
            string title = homepage.GetPropertyValue<string>("latestBlogPostsTitle");
            string introduction = homepage.GetPropertyValue("latestBlogPostsIntroduction").ToString();

            LatestBlogPosts model = new LatestBlogPosts(title, introduction);

            return PartialView(PARTIAL_VIEW_FOLDER + "_Blog.cshtml", model);
        } 

        public ActionResult RenderClients()
        {
            IPublishedContent homepage = CurrentPage.AncestorOrSelf("home");
            string title = homepage.GetPropertyValue<string>("testimonialsTitle");
            string introduction = homepage.GetPropertyValue("testimonialsIntroduction").ToString();

            TestimonialsModel model = new TestimonialsModel(title, introduction);

            return PartialView(PARTIAL_VIEW_FOLDER + "_Clients.cshtml", model);
        }
    }
}