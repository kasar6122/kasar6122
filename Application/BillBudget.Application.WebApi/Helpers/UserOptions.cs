using System.Collections.Generic;
using System.Security.Claims;
using BillBudget.Application.WebApi.Dtos;
using Microsoft.Extensions.Configuration;

namespace BillBudget.Application.WebApi.Helpers
{
    public class UserOptions
    {
        private readonly IConfiguration _configurationRoot;

        public UserOptions(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public UserFromDiuAuth GetUser(IEnumerable<Claim> claims)
        {
            var section = _configurationRoot.GetSection("UserFromDiuAuth");
            var userFromDiuAuth = new UserFromDiuAuth();
            //userFromDiuAuth.SupervisorId = claims.FirstOrDefault(c => c.Type == _userOptions.Value.SupervisorId).Value;
            //userFromDiuAuth.SupervisorName = claims.FirstOrDefault(c => c.Type == _userOptions.Value.SupervisorName).Value;
            //userFromDiuAuth.SupervisorDepartment = claims.FirstOrDefault(c => c.Type == _userOptions.Value.SupervisorDepartment).Value;
            //userFromDiuAuth.Designation = claims.FirstOrDefault(c => c.Type == _userOptions.Value.Designation).Value;
            //userFromDiuAuth.Department = claims.FirstOrDefault(c => c.Type == _userOptions.Value.Department).Value;
            //userFromDiuAuth.Email = claims.FirstOrDefault(c => c.Type == _userOptions.Value.Email).Value;
            //userFromDiuAuth.Name = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;

            //userFromDiuAuth.SupervisorId = claims.FirstOrDefault(c => c.Type == _userOptions.Value.SupervisorId).Value;
            //userFromDiuAuth.SupervisorName = claims.FirstOrDefault(c => c.Type == _userOptions.Value.SupervisorName).Value;
            //userFromDiuAuth.SupervisorDepartment = claims.FirstOrDefault(c => c.Type == _userOptions.Value.SupervisorDepartment).Value;
            //userFromDiuAuth.Designation = claims.FirstOrDefault(c => c.Type == _userOptions.Value.Designation).Value;
            //userFromDiuAuth.Department = claims.FirstOrDefault(c => c.Type == _userOptions.Value.Department).Value;
            //userFromDiuAuth.Email = claims.FirstOrDefault(c => c.Type == _userOptions.Value.Email).Value;
            //userFromDiuAuth.Name = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
            return userFromDiuAuth;
        }
    }
}
