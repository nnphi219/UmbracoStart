using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbracoStart.Models;
using System.Collections.Generic;
using Umbraco.Web;
using Umbraco.Core.Models;
using Umbraco.Core;
using System.Linq;
using Archetype.Models;

namespace UmbracoStart.Controllers
{
    public class HomeController : SurfaceController
    {
        private const int MAXIMUM_TESTIMONIAL = 4;

        private string PartialViewPath(string name)
        {
            return $"~/Views/Partials/Home/{name}.cshtml";
        }

        public ActionResult RenderIntro()
        {
            var test = ApplicationContext.Current.Services.ContentService.Count("");
            return PartialView(PartialViewPath("_Intro"));
        }

        public ActionResult RenderFeatured()
        {
            List<FeaturedItem> model = new List<FeaturedItem>();
            IPublishedContent homePage = CurrentPage.AncestorOrSelf("home");
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

            return PartialView(PartialViewPath("_Featured"), model);
        }

        public ActionResult RenderServices()
        {
            return PartialView(PartialViewPath("_Services"));
        }

        public ActionResult RenderBlog()
        {
            IPublishedContent homepage = CurrentPage.AncestorOrSelf("home");
            string title = homepage.GetPropertyValue<string>("latestBlogPostsTitle");
            string introduction = homepage.GetPropertyValue("latestBlogPostsIntroduction").ToString();

            LatestBlogPosts model = new LatestBlogPosts(title, introduction);

            return PartialView(PartialViewPath("_Blog"), model);
        } 

        public ActionResult RenderTestimonials()
        {
            IPublishedContent homepage = CurrentPage.AncestorOrSelf("home");
            string title = homepage.GetPropertyValue<string>("testimonialsTitle");
            string introduction = homepage.GetPropertyValue("testimonialsIntroduction").ToString();

            List<TestimonialModel> testimonials = new List<TestimonialModel>();

            ArchetypeModel testimonialList = homepage.GetPropertyValue<ArchetypeModel>("testimonialList");
            if(testimonialList != null)
            {
                foreach (ArchetypeFieldsetModel testimonial in testimonialList.Take(MAXIMUM_TESTIMONIAL))
                {
                    string name = testimonial.GetValue<string>("name");
                    string quote = testimonial.GetValue<string>("quote");
                    testimonials.Add(new TestimonialModel(quote, name));
                }
            }

            TestimonialsModel model = new TestimonialsModel(title, introduction, testimonials);
            return PartialView(PartialViewPath("_Testimonials"), model);
        }

        private void CreateUser()
        {
            //var userType = Services.UserService.CreateUserWithIdentity()
            var test = ApplicationContext.Current.Services;
   
        }
    }
}