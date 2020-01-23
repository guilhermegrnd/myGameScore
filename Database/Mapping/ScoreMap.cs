using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myGameScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myGameScore.Database.Mapping
{
    public class ScoreMap : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Date)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(t => t.Points)
                .HasMaxLength(10)
                .IsRequired();

            builder.ToTable("Scores");
        }
    }
}
