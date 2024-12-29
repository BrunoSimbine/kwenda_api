using System.Text.Json;
using System.Text.Json.Serialization;

namespace bilhete24.Models;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; } = "user";
    public string Phone { get; set; }
    public bool IsVerified { get; set; } = false;
    public bool IsDeleted
    {
        get
        {
            return DateDeleted.HasValue && DateDeleted.Value > DateTime.Now;
        }
    }

    [JsonIgnore]
    public byte[] PasswordHash { get; set; }

    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }
}
