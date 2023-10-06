using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HelloBuild.Domain.Models
{
    public class LoanRequest
    {
        [Required]
        public Guid Isbn { get; set; }

        [Required]
        [MaxLength(10)]
        public string? IdentificacionUsuario { get; set; }

        [Required]
        [Range(1, 3)]
        public int TipoUsuario { get; set; }

        [IgnoreDataMember]
        public DateTime TiempoDevolucion => GetReturnTime();

        private DateTime GetReturnTime()
        {
            DayOfWeek[] weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
            int loanDays = TipoUsuario switch
            {
                1 => 10,
                2 => 8,
                3 => 7,
                _ => -1,
            };

            DateTime returnDate = DateTime.Now;

            while (loanDays > 0)
            {
                returnDate = returnDate.AddDays(1);
                if (!weekend.Contains(returnDate.DayOfWeek))
                {
                    loanDays--;
                }
            }

            return returnDate;
        }
    }
}
