using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SERP.API.Model.Authenticate;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IGenericRepository<Authenticate, int> _IAuthenticateRepo;

        public AuthenticateController(IGenericRepository<Authenticate, int> _authenticateRepo)
        {
            _IAuthenticateRepo = _authenticateRepo;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody]UserModel model)
        {
            var user = await _IAuthenticateRepo.GetSingle(x => x.UserName == model.UserName && x.Password == model.Password && x.IsActive == 1 && x.IsExpired == 0 && x.IsLocked == 0);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SERPStudentPortalAPKKEY");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            model.Token = tokenHandler.WriteToken(token);
            model.UserName = user.UserName;
            model.StudentId = user.StudentId;


            return Ok(model);
        }
    }
}