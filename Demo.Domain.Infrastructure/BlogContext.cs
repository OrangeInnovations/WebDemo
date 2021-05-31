﻿using Common.Entities;
using Demo.Domain.AggregatesModels.BlogAggregate;
using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Infrastructure
{
    public class BlogContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "blogging";
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<MyUser> MyUsers { get; set; }

        private readonly IMediator _mediator;

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        public BlogContext(DbContextOptions<BlogContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MyUserEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection.
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes(from the Command Handler and Domain Event Handlers)
            // performed through the DbContext will be committed

            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
