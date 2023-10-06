using System.Text.Json.Serialization;

namespace HelloBuild.Domain.Models
{
    public class LoanResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("fechaMaximaDevolucion")]
        public DateTime ReturnDate { get; set; }
    }
}
