using API_Aggregation.Models.Spotify;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Mappings;

public class TrackMappings : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(p => p.ExternalUrl).HasMaxLength(255);
        builder.Property(p => p.Name).HasMaxLength(255).IsRequired();
        builder.Property(p => p.Href).HasMaxLength(512);
        builder.Property(p => p.Popularity).HasMaxLength(18).IsRequired(false);
        builder.Property(p => p.SpotifyTrackID).HasMaxLength(30).IsRequired();
        builder.Property(p => p.Type).HasMaxLength(255);
        builder.Property(p => p.AvailableMarkets).HasMaxLength(1000).IsRequired(false);
        
        //TrackGenre
        builder.HasMany(p => p.Genres)
            .WithOne(p => p.Track)
            .HasForeignKey(p => p.TrackId);

        //Image
        builder.HasMany(p => p.Genres)
            .WithOne(p => p.Track)
            .HasForeignKey(p => p.TrackId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}