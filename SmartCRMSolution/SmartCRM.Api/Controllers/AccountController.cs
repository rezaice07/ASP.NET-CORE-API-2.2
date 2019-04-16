using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SamrtCRM.Data.Models;
using SmartCRM.Api.ViewModels.Accounts;
using SmartCRM.Service.Encriptions;
using SmartCRM.Service.Users;
using static SmartCRM.Core.Utilities.AppConstants;

namespace SmartCRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AccountController(IMapper mapper,
            IConfiguration configuration,
            IUserService userService
            )
        {
            _mapper = mapper;
            _userService = userService;
            _configuration = configuration;
        }


        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> RegisterNewUser([FromBody] UserRegistrationViewModel model)
        {
            var newUser = _mapper.Map<User>(model); //if list then _mapper.Map<List<User>>(model);
            
            newUser.RoleId = UserRoleConstants.Customer;

            var isRegistered = _userService.Add(newUser);

            if (isRegistered)
            {
                return Ok(new { IsSuccess = false, Message = "There was an error while trying to register!" });
            }
            return Ok(new { IsSuccess = true, Message = "You have registered successfully" });
        }

        [Route("login")] // /login
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = _userService.GetDetailsByUsername(model.Username);

            if (user == null)
                return Ok(new { IsSuccess = false, Message = "Incorrect username or password.!" });

            if (user.Status != StatusConstants.Active && user.PasswordHash != EncryptionService.HashPassword(model.Password, user.PasswordSalt))
            {
                var message = "Incorrect Username or Password, Please try again.";
                return Ok(new { IsSuccess = false, Message = message });
            }

            var claims = new[] {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("CellPhone", user.Contact.CellPhone),
                    new Claim("Email", user.Contact.Email),
                    new Claim("FirstName", user.Contact.FirstName),
                    new Claim("LastName", user.Contact.LastName),
                    new Claim("Username", user.Username)
                };

            var signinKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

            int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

            var token = new JwtSecurityToken(
              claims: claims,
              issuer: _configuration["Jwt:Site"],
              audience: _configuration["Jwt:Site"],
              expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
              signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(
              new
              {
                  token = new JwtSecurityTokenHandler().WriteToken(token),
                  expiration = token.ValidTo
              });
        }
    }
}
