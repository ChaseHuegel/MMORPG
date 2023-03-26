using Microsoft.EntityFrameworkCore;

namespace MMO.Portal.Models;

public partial class PortalDbContext : DbContext
{
    public PortalDbContext(DbContextOptions<PortalDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
}