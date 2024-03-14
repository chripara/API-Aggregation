using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Models.Spotify;

public class ArtistAlbumMappings : IEntityTypeConfiguration<ArtistAlbum>
{
    public void Configure(EntityTypeBuilder<ArtistAlbum> builder)
    {
        builder.HasKey(aa => new { aa.AlbumId, aa.ArtistId });
    }
}