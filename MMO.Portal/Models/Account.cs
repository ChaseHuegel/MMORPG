using System.ComponentModel.DataAnnotations;

namespace MMO.Portal.Models;

public class Account
{
    [Key]
    public string User { get; set; }
    public string Email { get; set; }
    public string Roles { get; set; }
    public DateTime? CreatedAt { get; set; }

    internal uint ID { get; set; }
    internal string Salt { get; set; }
    internal string Hash { get; set; }
    internal DateTime? LastEmailUpdate { get; set; }
    internal DateTime? LastPasswordUpdate { get; set; }
}