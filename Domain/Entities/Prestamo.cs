using System.ComponentModel.DataAnnotations;

namespace HelloBuild.Domain.Entities
{
    public class Prestamo
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string? UserId { get; set; }

        [Required]
        public Guid Isbn { get; set; }

        [Required]
        public int UserType { get; set; }

        [Required]
        public DateTime MaxDateReturn { get; set; }
    }
}
