﻿using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Application.Commands;
using WebApi.Application.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyUserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IMyUserRepository myUserRepository;
        private readonly ILogger<MyUserController> logger;

        public MyUserController(IMediator mediator, IMapper mapper, IMyUserRepository myUserRepository, ILogger<MyUserController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.myUserRepository = myUserRepository;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [Route("all")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MyUserVM>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllUsers()
        {
            var myusers = await myUserRepository.GetAllAsync();
            var list = mapper.Map<List<MyUserVM>>(myusers);

            logger.LogInformation($"get all users {list}");

            return Ok(list);
        }


        // POST api/<MyUserController>
        [HttpPost]
        [ProducesResponseType(typeof(MyUserVM), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MyUserVM>> CreateMyUserAsync([FromBody] CreateMyUserCommand createMyUserCommand)
        {
            var myUser = await mediator.Send(createMyUserCommand);
            var result = mapper.Map<MyUserVM>(myUser);
            return Ok(result);
        }


    }
}
