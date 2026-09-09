using System.Text.Json.Serialization;

namespace Cereal_API.Shared.Models;

public class ChangeLog()
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Username { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int CerealDataId { get; set; }
    [JsonIgnore] public CerealData? CerealData { get; set; }
}
