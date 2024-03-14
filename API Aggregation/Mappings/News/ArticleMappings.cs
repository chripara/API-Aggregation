using API_Aggregation.Models.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Mappings.News;

public class ArticleMappings : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(p => p.SourceName).HasMaxLength(200).IsRequired(false);
        builder.Property(p => p.Author).HasMaxLength(60).IsRequired(false);
        builder.Property(p => p.Title).HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(20000).IsRequired(false);
        builder.Property(p => p.Url).HasMaxLength(500).IsRequired(false);
        builder.Property(p => p.UrlToImage).HasMaxLength(500).IsRequired(false);
        builder.Property(p => p.Content).HasMaxLength(20000);
    }
}