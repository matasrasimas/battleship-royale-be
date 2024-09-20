using System.Text.Json.Serialization;

namespace battleship_royale_be.DTOs
{
    public class GameDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
