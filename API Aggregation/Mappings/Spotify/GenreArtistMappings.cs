using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Models.Spotify;

public class GenreArtistMappings : IEntityTypeConfiguration<GenreArtist>
{
    public void Configure(EntityTypeBuilder<GenreArtist> builder)
    {
        builder.HasKey(aa => new { aa.GenreId, aa.ArtistId });

    }
}