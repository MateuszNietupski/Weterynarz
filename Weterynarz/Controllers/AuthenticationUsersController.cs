using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Weterynarz.Models;
using Weterynarz.Models.DTOs;

namespace Weterynarz.Controllers
{
    [Route("api[controller]")]
    [ApiController]
    public class AuthenticationUsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthenticationUsersController(UserManager<IdentityUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);
                if (user_exist != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Email already exist"
                        }
                    });
                }
                var new_user = new IdentityUser()
                {
                    Email = requestDto.Email,
                    //  UserName = requestDto.Name


                };
                var is_created = await _userManager.CreateAsync(new_user, requestDto.Password);


                if (is_created.Succeeded)
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                    var role = new IdentityRole("User");
                    await _userManager.AddToRoleAsync(new_user, "User");
                    var token = GenerateJwtToken(user_exist, role);
                    return Ok(new AuthResult()
                    {
                        Token = token,
                        Result = true
                    });
                }
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                {
                    "Server error"
                },
                    Result = false
                });
            }
            return BadRequest();
        }
        [Route("Login")]
        [HttpPost]

        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                //Sprawdzenie czy user istnieje
                var existing_user = await _userManager.FindByEmailAsync(loginRequest.Email);
                IdentityRole? user_role = (IdentityRole)await _userManager.GetRolesAsync(existing_user);

                if (existing_user == null)
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Blad logowania"
                        },
                        Result = false
                    });
                var isCorrect = await _userManager.CheckPasswordAsync(existing_user, loginRequest.Password);

                if (!isCorrect)
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid credentials"
                        },
                        Result = false
                    });
                // await _roleManager.FindByIdAsync(_roleManager.GetRoleIdAsync();
                var jwtToken = GenerateJwtToken(existing_user, user_role);
                return Ok(new AuthResult()
                {
                    Token = jwtToken,
                    Result = true
                });
            }
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                {
                    "Blad w logowaniu"
                },
                Result = false
            });
        }
        private string GenerateJwtToken(IdentityUser user,IdentityRole role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id",user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()),
                    new Claim(ClaimTypes.Role,role.Name)
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
