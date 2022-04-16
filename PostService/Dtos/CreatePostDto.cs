using System;
using System.ComponentModel.DataAnnotations;

namespace PostService.Dtos{
    public class CreatePostDto{
        public string UserId { get; set; }
        [Required]
        public string PostMessage { get; set; }
        public DateTime PostDateTime { get; set; }
    }
}