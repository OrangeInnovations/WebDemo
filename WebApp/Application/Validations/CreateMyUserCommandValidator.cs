using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Commands;

namespace WebApp.Application.Validations
{
    public class CreateMyUserCommandValidator: AbstractValidator<CreateMyUserCommand>
    {
        public CreateMyUserCommandValidator(ILogger<CreateMyUserCommandValidator> logger)
        {
            RuleFor(command => command.FirstName).NotEmpty().Length(1,256).WithMessage("FirstName is required.");

            RuleFor(command => command.LastName).NotEmpty().Length(1, 256).WithMessage("LastName is required.");

            RuleFor(command => command.EmailAddress).NotEmpty().WithMessage("Email address is required")
                     .EmailAddress().WithMessage("A valid email is required");


            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }

       
    }
}
