﻿using Demo.Domain.AggregatesModels.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Services.Commands
{
    public class CreateMyUserCommandHandler : IRequestHandler<CreateMyUserCommand, MyUser>
    {
        private readonly IMyUserRepository myUserRepository;
        private readonly ILoggerFactory logger;

        public CreateMyUserCommandHandler(IMyUserRepository myUserRepository, ILoggerFactory logger)
        {
            this.myUserRepository = myUserRepository;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<MyUser> Handle(CreateMyUserCommand request, CancellationToken cancellationToken)
        {
            MyUser myUser = new MyUser(request.FirstName, request.MiddleName, request.LastName, request.EmailAddress);

            myUserRepository.Add(myUser);

            await myUserRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return myUser;
        }
    }
}
