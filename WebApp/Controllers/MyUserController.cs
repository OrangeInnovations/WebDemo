﻿using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Services.Commands;
using Demo.Domain.Services.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
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

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MyUserVM>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllUsers()
        {
            var myusers = await myUserRepository.GetAllAsync();
            var list = mapper.Map<List<MyUserVM>>(myusers);

            var listJson = JsonConvert.SerializeObject(list);

            logger.LogInformation($"get all users {listJson}");

            return Ok(list);
        }


        /// <summary>
        /// Create a user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///        "FirstName": "sam",
        ///        "MiddleName": "delta",
        ///        "LastName": "Smith",
        ///        "EmailAddress": "abc@hotmail.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="createMyUserCommand"></param>
        /// <returns></returns>
        [HttpPost]
       // [Authorize(Roles = "OperationGroup,AdminGroup")]
        [ProducesResponseType(typeof(MyUserVM), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MyUserVM>> CreateMyUserAsync([FromBody] CreateMyUserCommand createMyUserCommand)
        {
            var myUser = await mediator.Send(createMyUserCommand);
            var result = mapper.Map<MyUserVM>(myUser);
            return Ok(result);
        }


    }
}
