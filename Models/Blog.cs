using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogsConsole.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Required (ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }

        public List<Post> Posts { get; set; }
    }
}
