using System.ComponentModel.DataAnnotations;

namespace HelloBuild.Domain.Models
{
    public class UserRequest
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
