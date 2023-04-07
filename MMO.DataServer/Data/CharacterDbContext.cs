using Microsoft.EntityFrameworkCore;

namespace MMO.DataServer.Data;

public partial class CharactersDbContext : DbContext
{
    public CharactersDbContext(DbContextOptions<CharactersDbContext> options) : base(options) { }
}