using Common.Entities;
using Demo.Domain.AggregatesModels.UserAggregate;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Demo.Domain.Infrastructure.Repositories
{
    public class MyUserRepository : IMyUserRepository
    {
        private readonly BlogDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public MyUserRepository(BlogDbContext blogContext)
        {
            _context = blogContext ?? throw new ArgumentNullException(nameof(blogContext));
        }
        public MyUser Add(MyUser myUser)
        {
            return _context.MyUsers.Add(myUser).Entity;
        }

        public async Task<MyUser> GetAsync(string myUserId)
        {
            var myuser= await _context.MyUsers.FirstOrDefaultAsync(d => d.Id == myUserId); 
            if(myuser==null)
            {
                myuser =  _context.MyUsers.Local.FirstOrDefault(d => d.Id == myUserId);
            }
            return myuser;

        }

        public void Update(MyUser myUser)
        {
            _context.Entry(myUser).State = EntityState.Modified;
        }
    }
}
