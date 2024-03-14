using System.Reflection;
using API_Aggregation.Models.News;
using API_Aggregation.Models.Spotify;
using API_Aggregation.Models.Weather;
using Microsoft.EntityFrameworkCore;

namespace API_Aggregation;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)   
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Weather> Weathers { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<ArtistAlbum> ArtistAlbums { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<GenreAlbum> GenreAlbums { get; set; }
    public DbSet<GenreTrack> GenreTracks { get; set; }
    public DbSet<GenreArtist> GenreArtists { get; set; }
}