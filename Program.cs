using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass : BloggingContext
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void  Main(string[] args)
        {
            logger.Info("Program started");
            try
            {

                string userChoice;
                do
                {
                    //ask user for input
                    Console.WriteLine("1) Display all blogs");
                    Console.WriteLine("2) Add Blog");
                    Console.WriteLine("3) Create Post");
                    Console.WriteLine("4) Display Posts");
                    Console.WriteLine("5) Delete Post");
                    Console.WriteLine("6) Edit Post");
                    Console.WriteLine("Enter x to exit");
                    userChoice = Console.ReadLine();
                    //clear user input
                    Console.Clear();
                    //log user input
                    logger.Info("Option {userChoice} selected", userChoice);

                    BloggingContext choice = new BloggingContext();
                    if (userChoice == "1")
                    {
                        //display all blogs
                        choice.DisplayBlog();
                    } 
                    else if (userChoice == "2")
                    {
                        //add blog
                        choice.AddBlog();
                    }
                    else if (userChoice == "3")
                    {
                        //create post
                        choice.CreatePost();
                    }
                    else if (userChoice == "4")
                    {
                        //display post
                        choice.DisplayPost();
                    }
                    else if (userChoice == "5")
                    {
                        //delete post
                        choice.DeletePost();
                    }
                    else if (userChoice == "6")
                    {
                        //edit post
                        choice.EditPost();
                    }
                } while (userChoice.ToLower() != "x");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
