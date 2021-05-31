using Demo.Domain.AggregatesModels.BlogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Infrastructure.EntityConfigurations
{
    class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("blogs", BlogDbContext.DEFAULT_SCHEMA);
            builder.HasKey(c => c.Id);
            builder.Ignore(b => b.DomainEvents);
            builder.Property(o => o.Id)
               .UseHiLo("postseq");

            builder.Property(b => b.Url).IsRequired().HasMaxLength(256);

            builder.Property(b => b.OwnerId).IsRequired().HasMaxLength(256);
            builder.Property(b => b.CreatedTimeUtc).IsRequired();
            

            builder.Property(b => b.Rating).IsRequired();
           
        }
    }
}
