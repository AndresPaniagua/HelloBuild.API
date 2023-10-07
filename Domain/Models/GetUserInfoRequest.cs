using System.ComponentModel.DataAnnotations;

namespace HelloBuild.Domain.Models
{
    public class GetUserInfoRequest
    {
        [Required]
        public string? Email { get; set; }
    }
}
