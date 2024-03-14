using API_Aggregation.Models.Spotify;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Mappings;

public class AlbumMappings : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(p => p.AlbumType).HasMaxLength(255);
        builder.Property(p => p.Href).HasMaxLength(512);
        builder.Property(p => p.Name).HasMaxLength(255);
        builder.Property(p => p.ReleaseDate).HasMaxLength(255);
        builder.Property(p => p.Type).HasMaxLength(255);
        builder.Property(p => p.SpotifyAlbumId).HasMaxLength(255);
        
        //ArtistAlbum
        builder.HasMany(p => p.Artists)
            .WithOne(p => p.Album)
            .HasForeignKey(p => p.AlbumId);

        //Tracks
        builder.HasMany(p => p.Tracks)
            .WithOne(p => p.Album)
            .HasForeignKey(p => p.AlbumId);

        //Images
        builder.HasMany(p => p.Images)
            .WithOne(p => p.Album)
            .HasForeignKey(p => p.AlbumId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}