using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string userChoice;
                do
                {
                    Console.WriteLine("1) Display all blogs");
                    Console.WriteLine("2) Add Blog");
                    Console.WriteLine("3) Create Post");
                    Console.WriteLine("4) Display Posts");
                    Console.WriteLine("5) Delete Post");
                    Console.WriteLine("6) Edit Post");
                    Console.WriteLine("7) Display All Blogs and All Post");
                    Console.WriteLine("Enter x to exit program");
                    userChoice = Console.ReadLine();
                    Console.Clear();

                    if (userChoice == "1")
                    {
                        // Choice 1: Display Blogs
                        var database = new BloggingContext();
                        var query = database.Blogs.OrderBy(b => b.Name);

                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    else if (userChoice == "2")
                    {
                        // Choice 2: Add Blog
                        Console.Write("What would you like to name your new blog?");
                        var blog = new Blog { Name = Console.ReadLine() };

                        ValidationContext context = new ValidationContext(blog, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        //Validate userinput (Review Northwind example)
                        var isValid = Validator.TryValidateObject(blog, context, results, true);
                        if (isValid)
                        {
                            var database = new BloggingContext();
                            if (database.Blogs.Any(b => b.Name == blog.Name))
                            {
                                isValid = false;
                                results.Add(new ValidationResult("Blog name exists", new string[] { "Name" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                database.AddBlog(blog);
                                logger.Info("Blog added - {name}", blog.Name);
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (userChoice == "3")
                    {
                        // Choice 3: Create Post
                        var database = new BloggingContext();
                        var query = database.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("Please choose blog to add post to.");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}) {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            if (database.Blogs.Any(b => b.BlogId == BlogId))
                            {
                                Post post = InputPost(database);
                                if (post != null)
                                {
                                    post.BlogId = BlogId;
                                    database.AddPost(post);
                                    logger.Info("Post added - {title}", post.Title);
                                }
                            }
                            else
                            {
                                logger.Error("There are no Blogs saved with that Id");
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Blog Id");
                        }
                    }
                    else if (userChoice == "4")
                    {
                        // Choice 4: Display Posts
                        var database = new BloggingContext();
                        var query = database.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Which blog do you want to view post?");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}) Posts from {item.Name}");
                        }
                        IEnumerable<Post> Posts;

                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            Posts = database.Posts.Where(p => p.BlogId == BlogId).OrderBy(p => p.Title);
                            foreach (var item in Posts)
                            {
                                Console.WriteLine($"Blog: {item.Blog.Name}\nTitle: {item.Title}\nContent: {item.Content}\n");
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Blog Id");
                        }
                    }
                    else if (userChoice == "5")
                    {
                        // Choice 5: delete post
                        Console.WriteLine("Choose the post to delete");
                        var database = new BloggingContext();
                        var post = GetPost(database);
                        if (post != null)
                        {
                            database.DeletePost(post);
                        }
                        else
                        {
                            logger.Error("Invalid Post");
                        }
                    }
                    else if (userChoice == "6")
                    {
                        // Choice 6: Edit Post
                        Console.WriteLine("Choose the post to edit");
                        var database = new BloggingContext();
                        var post = GetPost(database);
                        if (post != null)
                        {
                            Post UpdatedPost = InputPost(database);
                            if (UpdatedPost != null)
                            {
                                UpdatedPost.PostId = post.PostId;
                                database.EditPost(UpdatedPost);
                            }
                            else
                            {
                                logger.Error("Invalid Post Update");
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Post");
                        }
                    }
                    else if (userChoice == "7")
                    {
                        //Choice 7: Display All Post From All Blogs
                        var database = new BloggingContext();
                        var query = database.Blogs.OrderBy(b => b.BlogId);
                        IEnumerable<Post> Posts;
                        Posts = database.Posts.OrderBy(p => p.Title);
                    }
                    Console.WriteLine();
                } while (userChoice.ToLower() != "x");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }

        public static Post InputPost(BloggingContext database)
        {
            Post post = new Post();
            Console.WriteLine("Enter the Post title");
            post.Title = Console.ReadLine();
            Console.WriteLine("Enter the Post content");
            post.Content = Console.ReadLine();

            return null;
        }

        public static Post GetPost(BloggingContext database)
        {
            var blogs = database.Blogs.Include("Posts").OrderBy(b => b.Name);
            foreach (Blog b in blogs)
            {
                Console.WriteLine(b.Name);
                if (b.Posts.Count() == 0)
                {
                    Console.WriteLine($"No Posts");
                }
                else
                {
                    foreach (Post p in b.Posts)
                    {
                        Console.WriteLine($"  {p.PostId}) {p.Title}");
                    }
                }
            }
            if (int.TryParse(Console.ReadLine(), out int PostId))
            {
                Post post = database.Posts.FirstOrDefault(p => p.PostId == PostId);
                if (post != null)
                {
                    return post;
                }
                else
                {
                    logger.Error("Please enter vaid post");
                }
            }
            return null;
        }
    }
}
