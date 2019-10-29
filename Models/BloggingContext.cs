using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NLog;
namespace BlogsConsole.Models
{
    public class BloggingContext : DbContext
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public BloggingContext() : base("name=BlogContext") { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public void DisplayBlog()
        {
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.Name);

            Console.WriteLine($"{query.Count()} Blogs returned");
            foreach (var item in query)
            {
                Console.WriteLine(item.Name);
            }
        }
        public void AddBlog()
        {
            Console.Write("Enter new blog");
            var blog = new Blog { Name = Console.ReadLine() };
            var db = new BloggingContext();

            logger.Info("Blog added - {name}", blog.Name);
        }
        public void CreatePost()
        {
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.BlogId);

            Console.WriteLine("Select blog");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.BlogId}) {item.Name}");
                Console.WriteLine("Enter blog ID here");
                int userBlogChosen = int.Parse(Console.ReadLine());

                Post match = new Post();

                if (userBlogChosen == item.BlogId)
                {
                    //add post
                }
            }
        }
        public void DisplayPost()
        {
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.BlogId);
            Console.WriteLine("Enter Blog ID to display");
            Console.WriteLine("(D)isplay all blogs");
            var userSelect = Console.ReadLine().ToUpper();

            foreach (var item in query)
            {
                Console.WriteLine($"{item.BlogId}) Posts from {item.Name}");
            }
            List<Post> LPost;
            if (userSelect == "D")
            {
                //display all post
            }
        }
        public void DeletePost()
        {
            //delete post

        }
        public void EditPost()
        {
            //edit post

        }
    }
}
