using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Models{
    public class Post{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string PostMessage { get; set; }
        [Required]
        public string PostDateTime { get; set; }

    }
}