using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Models.Spotify;

public class GenreAlbumMappings : IEntityTypeConfiguration<GenreAlbum>
{
    public void Configure(EntityTypeBuilder<GenreAlbum> builder)
    {
        builder.HasKey(aa => new { aa.GenreId, aa.AlbumId });

    }
}