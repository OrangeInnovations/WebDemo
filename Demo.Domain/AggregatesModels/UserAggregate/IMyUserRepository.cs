using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.AggregatesModels.UserAggregate
{
    public interface IMyUserRepository: IRepository<MyUser>
    {
        MyUser Add(MyUser myUser);
        void Update(MyUser myUser);

        Task<MyUser> GetAsync(string myUserId);

        Task<List<MyUser>> GetAllAsync();
    }
}
