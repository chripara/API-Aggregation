using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Models.Spotify;

public class GenreTrackMappings : IEntityTypeConfiguration<GenreTrack>
{
    public void Configure(EntityTypeBuilder<GenreTrack> builder)
    {
        builder.HasKey(aa => new { aa.GenreId, aa.TrackId });

    }
}