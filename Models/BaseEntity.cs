using System.Text.Json;
using System.Text.Json.Serialization;

namespace bilhete24.Models;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    
    [JsonIgnore]
    public DateTime DateCreated { get; set; } = DateTime.Now;

    [JsonIgnore]
    public DateTime? DateUpdated { get; set; }

    [JsonIgnore]
    public DateTime? DateDeleted { get; set; }
}