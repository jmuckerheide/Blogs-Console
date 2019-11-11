using System.ComponentModel.DataAnnotations;

namespace BlogsConsole.Models
{
    public class Post
    {
        private const string V = "Please enter a maximum of 30 characters";

        [Key]
        public int PostId { get; set; }
        [Required (ErrorMessage = "Please enter a Title")]
        public string Title { get; set; }
        [Range(1, 30, ErrorMessage = V)]
        public string Content { get; set; }
        [Key]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
