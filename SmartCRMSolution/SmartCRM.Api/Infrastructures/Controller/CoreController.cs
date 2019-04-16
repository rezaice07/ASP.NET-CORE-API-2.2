using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamrtCRM.Data.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace SmartCRM.Api.Infrastructures.Controller
{
    [Authorize]
    [ApiController]
    public class CoreController: ControllerBase
    {
        public User CurrentLoginUser
        {
            get
            {
                var userModel = new User();

                if (!User.Identity.IsAuthenticated)
                    return userModel;

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userClaims = claimsIdentity.Claims;

                if (userClaims == null)
                    return new User();

                userModel.Id = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "Id").Value);
                userModel.Username = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "Username").Value;
                userModel.Contact.FirstName = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "FirstName").Value;
                userModel.Contact.LastName = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "LastName").Value;
                userModel.Contact.CellPhone = claimsIdentity.Claims.FirstOrDefault(f => f.Type == "CellPhone").Value;
                userModel.Contact.Email = claimsIdentity.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email).Value;

                return userModel;
            }
        }
    }
}
