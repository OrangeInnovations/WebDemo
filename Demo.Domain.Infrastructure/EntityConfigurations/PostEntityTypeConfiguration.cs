using Demo.Domain.AggregatesModels.BlogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Infrastructure.EntityConfigurations
{
    class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("posts", BlogContext.DEFAULT_SCHEMA);
            builder.HasKey(c => c.Id);
            builder.Ignore(b => b.DomainEvents);
            builder.Property(o => o.Id)
               .UseHiLo("postseq");

            builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Content).HasMaxLength(1024);

            builder.Property(b => b.PosterId).IsRequired().HasMaxLength(256);
            builder.Property(b => b.CreatedTimeUtc).IsRequired();
            

            builder.Property(b => b.BlogId).IsRequired();
            builder.HasOne(b => b.Blog).WithMany(o => o.Posts).HasForeignKey(a => a.BlogId);

            
        }
    }
}
