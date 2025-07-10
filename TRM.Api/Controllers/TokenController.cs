using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TRMApi.Data;

namespace TRMApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public TokenController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm]string username, [FromForm] string password, [FromForm] string grant_type)
        {
            if (await IsValidUsernameAndPassword(username, password))
            {
                var token = await GenerateToken(username);

                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(username);
                return await _userManager.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }

            return false;

        }

        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);

            var userRolesList = await _userManager.GetRolesAsync(user);

            var userRoles = _context.Roles.Where(r => userRolesList.Contains(r.Name)).ToList();

            var userData = from ur in userRoles
                           select new { UserId = user.Id, RoleId = ur.Id, ur.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            foreach(var role in userData)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                                    new JwtHeader(
                                        new SigningCredentials(
                                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Secrets:SecurityKey"))), SecurityAlgorithms.HmacSha256)
                                        ),
                                        new JwtPayload(claims));


            try
            {

                var output = new
                {
                    Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Username = user.UserName
                };

                return output;
            }
            catch (Exception ex)
            {

                var m = ex.Message;

            }

            return null;

           
        }

    }
}
