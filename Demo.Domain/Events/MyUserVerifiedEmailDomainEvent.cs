using Demo.Domain.AggregatesModels.UserAggregate;
using MediatR;

namespace Demo.Domain.Events
{
    public class MyUserVerifiedEmailDomainEvent : INotification
    {
        
        public MyUser MyUser { get; private set; }

        public MyUserVerifiedEmailDomainEvent(MyUser user)
        {
            MyUser = user;
        }
    }
}
