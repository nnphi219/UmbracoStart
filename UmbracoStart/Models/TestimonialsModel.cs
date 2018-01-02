namespace UmbracoStart.Models
{
    public class TestimonialsModel
    {
        public string Title { get; set; }
        public string Introduction { get; set; }

        public TestimonialsModel(string title, string introduction)
        {
            Title = title;
            Introduction = introduction;
        }
    }
}