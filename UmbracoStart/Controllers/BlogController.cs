﻿using System;
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
        private string PartialViewPath(string name)
        {
            return $"~/Views/Partials/Blog/{name}.cshtml";
        }

        public ActionResult RenderPostList(int numberOfItems)
        {
            List<BlogPreview> model = new List<BlogPreview>();
            IPublishedContent homepage = CurrentPage.AncestorOrSelf("home");
            IPublishedContent blogPage = homepage.Children.Where(x => x.DocumentTypeAlias == "blog").FirstOrDefault();
            //blogPage = homepage.Descendant("blog"); // solution 2

            var blogPosts = blogPage.Children.OrderByDescending(x => x.UpdateDate).Take(numberOfItems);
            //blogPosts = blogPage.Descendants("blogPost").OrderByDescending(x => x.UpdateDate).Take(numberOfItems); // solution 2

            foreach (IPublishedContent page in blogPosts)
            {
                var mediaItem = page.GetPropertyValue<IPublishedContent>("articleImage");/* page.GetPropertyValue<int>("image");*/
                string imageUrl = mediaItem.Url;
                model.Add(new BlogPreview(page.Name, page.GetPropertyValue<string>("articleIntro"), imageUrl, page.Url));
            }

            return PartialView(PartialViewPath("_PostList"), model);
        }
    }
}