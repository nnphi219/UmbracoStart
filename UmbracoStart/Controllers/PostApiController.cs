using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace UmbracoStart.Controllers
{
    public class PostApiController : UmbracoApiController
    {
        [HttpGet]
        public List<string> Test()
        {
            return new List<string> { "Test", "Umbraco", "API" };
        }
    }
}