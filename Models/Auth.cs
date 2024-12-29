using System.Text.Json;
using System.Text.Json.Serialization;

namespace bilhete24.Models;

public class Auth : BaseEntity
{
    [JsonIgnore]
    public User User { get; set; }
    public Guid UserId { get; set; }

    public string Device { get; set; }
    public string IpAddress { get; set; } = "127.0.0.1";
    public string Token { get; set; }
    public DateTime LastActivity { get; set; } = DateTime.Now;
    public bool IsDeleted
    {
        get
        {
            return DateDeleted.HasValue && DateDeleted.Value > DateTime.Now;
        }
    }
}
