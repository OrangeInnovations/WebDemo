using Demo.Domain.AggregatesModels.BlogAggregate;
using Demo.Domain.AggregatesModels.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Infrastructure.EntityConfigurations
{
    class MyUserEntityTypeConfiguration : IEntityTypeConfiguration<MyUser>
    {
        public void Configure(EntityTypeBuilder<MyUser> builder)
        {
            builder.ToTable("myusers", BlogDbContext.DEFAULT_SCHEMA);
            builder.HasKey(c => c.Id);
            builder.Ignore(b => b.DomainEvents);
            builder.Property(c => c.Id).HasMaxLength(256);
           

            builder.Property(b => b.FirstName).IsRequired().HasMaxLength(256);

            builder.Property(b => b.LastName).IsRequired().HasMaxLength(256);

            builder.Property(b => b.MiddleName).HasMaxLength(256);

            builder.Property(b => b.EmailAddress).IsRequired().HasMaxLength(256);

            builder.Property(b => b.CreatedTimeUtc).IsRequired();

            builder.HasMany(b => b.MyBlogs).WithOne().HasForeignKey(a => a.OwnerId);
            builder.HasMany(b => b.MyPosts).WithOne().HasForeignKey(a => a.PosterId);
           
        }
    }
}
