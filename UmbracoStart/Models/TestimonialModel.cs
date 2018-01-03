using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbracoStart.Models
{
    public class TestimonialModel
    {
        public string Quote { get; set; }
        public string Name { get; set; }

        public TestimonialModel(string quote, string name)
        {
            Quote = quote;
            Name = name;
        }
    }
}