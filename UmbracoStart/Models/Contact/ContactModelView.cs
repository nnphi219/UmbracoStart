
using System.ComponentModel.DataAnnotations;

namespace UmbracoStart.Models.Contact
{
    public class ContactModelView
    {
        public string Name { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Message { get; set; }
    }
}