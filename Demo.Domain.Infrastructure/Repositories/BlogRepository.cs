using Common.Entities;
using Demo.Domain.AggregatesModels.UserAggregate;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Demo.Domain.AggregatesModels.BlogAggregate;

namespace Demo.Domain.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public BlogRepository(BlogContext blogContext)
        {
            _context = blogContext ?? throw new ArgumentNullException(nameof(blogContext));
        }
        public Blog Add(Blog blog)
        {
            return _context.Blogs.Add(blog).Entity;
        }

        public async Task<Blog> GetAsync(int blogId)
        {
            var blog= await _context.Blogs.FirstOrDefaultAsync(d => d.Id == blogId); 
            if(blog==null)
            {
                blog =  _context.Blogs.Local.FirstOrDefault(d => d.Id == blogId);
            }
            return blog;

        }

        public void Update(Blog blog)
        {
            _context.Entry(blog).State = EntityState.Modified;
        }
    }
}
