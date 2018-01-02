namespace UmbracoStart.Models
{
    public class LatestBlogPosts
    {
        public string Title { get; set; }
        public string Introduction { get; set; }

        public LatestBlogPosts (string title, string introduction)
        {
            Title = title;
            Introduction = introduction;
        }
    }
}