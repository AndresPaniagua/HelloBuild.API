using System.Text.Json.Serialization;

namespace HelloBuild.Domain.Models
{
    public class BadResponse
    {
        [JsonPropertyName("mensaje")]
        public string? Message { get; set; }
    }
}
