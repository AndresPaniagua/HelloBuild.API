using System.Text.Json.Serialization;

namespace HelloBuild.Domain.Models
{
    public class LoanUserResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("isbn")]
        public string? Isbn { get; set; }

        [JsonPropertyName("identificacionUsuario")]
        public string? UserId { get; set; }


        [JsonPropertyName("tipoUsuario")]
        public int UserType { get; set; }

        [JsonPropertyName("fechaMaximaDevolucion")]
        public DateTime ReturnDate { get; set; }
    }
}
