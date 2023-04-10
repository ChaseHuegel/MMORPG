using Microsoft.EntityFrameworkCore;
using MMO.Bridge.Models;

namespace MMO.DataServer.Data;

public partial class CharactersDbContext : DbContext
{
    public CharactersDbContext(DbContextOptions<CharactersDbContext> options)
        : base(options) { }

    public DbSet<CharacterClass> Classes { get; set; }

    public DbSet<CharacterRace> Races { get; set; }
}
