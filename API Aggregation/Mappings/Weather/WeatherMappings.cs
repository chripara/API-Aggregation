using API_Aggregation.Models.Weather;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Aggregation.Mappings;

public class WeatherMappings : IEntityTypeConfiguration<Weather>
{
    public void Configure(EntityTypeBuilder<Weather> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(p => p.DayInKelvin).HasMaxLength(18);
        builder.Property(p => p.EveningTemperatureInKelvin).HasMaxLength(18);
        builder.Property(p => p.NightInKelvin).HasMaxLength(18);
        builder.Property(p => p.MorningInKelvin).HasMaxLength(18);
        builder.Property(p => p.FeelsLikeMorningInKelvin).HasMaxLength(18);
        builder.Property(p => p.FeelsLikeEveningInKelvin).HasMaxLength(18);
        builder.Property(p => p.FeelsLikeDayInKelvin).HasMaxLength(18);
        builder.Property(p => p.FeelsLikeNightInKelvin).HasMaxLength(18);
        builder.Property(p => p.Humidity).HasMaxLength(18);
        builder.Property(p => p.MinTemperatureInKelvin).HasMaxLength(18);
        builder.Property(p => p.MaxTemperatureInKelvin).HasMaxLength(18);
        builder.Property(p => p.WindInMeterPerSec).HasMaxLength(18);
        builder.Property(p => p.WindDegree).HasMaxLength(18);
        builder.Property(p => p.AtmosphericPressure).HasMaxLength(18);
    }
}