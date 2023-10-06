using System.ComponentModel.DataAnnotations;

namespace HelloBuild.Domain.Models
{
    public class UserExistRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
