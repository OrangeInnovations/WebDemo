using Demo.Domain.AggregatesModels.UserAggregate;
using MediatR;

namespace Demo.Domain.Events
{
    public class MyUserVerifiedDomainEvent : INotification
    {
        
        public MyUser MyUser { get; private set; }

        public MyUserVerifiedDomainEvent(MyUser user)
        {
            MyUser = user;
        }
    }
}
