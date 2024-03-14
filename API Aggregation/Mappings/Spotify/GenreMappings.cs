using API_Aggregation.Models.Spotify;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Mappings;

public class GenreMappings : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(p => p.Name).HasMaxLength(255);
        
        //GenreArtist
        builder.HasMany(p => p.Artists)
            .WithOne(p => p.Genre)
            .HasForeignKey(p => p.GenreId);

        //GenreAlbum
        builder.HasMany(p => p.Albums)
            .WithOne(p => p.Genre)
            .HasForeignKey(p => p.GenreId);

        //GenreTrack
        builder.HasMany(p => p.Artists)
            .WithOne(p => p.Genre)
            .HasForeignKey(p => p.GenreId);

    }
}