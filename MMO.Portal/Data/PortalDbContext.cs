using Microsoft.EntityFrameworkCore;
using MMO.Portal.Models;

namespace MMO.Portal.Data;

public partial class PortalDbContext : DbContext
{
    public PortalDbContext(DbContextOptions<PortalDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
}