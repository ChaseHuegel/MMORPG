using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MMO.Portal.Models;

public class Account
{
    [Key]
    public string User { get; init; }

    public string Email { get; set; }

    public string Roles { get; set; }

    public DateTime? CreatedAt { get; init; }

    [JsonIgnore]
    public uint ID { get; init; }

    [JsonIgnore]
    public string Salt { get; init; }

    [JsonIgnore]
    public string Hash { get; init; }

    [JsonIgnore]
    public DateTime? LastEmailUpdate { get; set; }

    [JsonIgnore]
    public DateTime? LastPasswordUpdate { get; set; }
}