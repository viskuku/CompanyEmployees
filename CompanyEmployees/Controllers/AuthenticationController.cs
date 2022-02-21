using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILoggerManager loggerManager, IMapper mapper, UserManager<User> userManager, IAuthenticationManager authenticationManager)
        {
            _logger = loggerManager;
            _mapper = mapper;
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }


        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            try
            {
                var role = await _userManager.AddToRoleAsync(user, "Manager");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuth)
        {
            var validateUser = !await _authenticationManager.ValidateUser(userForAuth);

            if (validateUser)
            {
                _logger.LogError($"{nameof(Authenticate)}: Authentication Failed. User name and password failed.");

                return Unauthorized();
            }

            return Ok(new { token = await _authenticationManager.CreateToken() });
        }


    }
}
