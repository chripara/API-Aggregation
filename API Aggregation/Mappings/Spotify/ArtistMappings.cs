using API_Aggregation.Models.Spotify;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Mappings;

public class ArtistMappings : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(p => p.SpotifyUrl).HasMaxLength(255);
        builder.Property(p => p.Alias).HasMaxLength(250);
        builder.Property(p => p.Href).HasMaxLength(500);

        //ArtistGenre
        builder.HasMany(p => p.Genres)
            .WithOne(p => p.Artist)
            .HasForeignKey(p => p.ArtistId);
        
        //ArtistGenre
        builder.HasMany(p => p.Images)
            .WithOne(p => p.Artist)
            .HasForeignKey(p => p.ArtistId)
            .OnDelete(DeleteBehavior.NoAction);
            
    }
}