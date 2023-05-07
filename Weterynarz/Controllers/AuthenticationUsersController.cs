using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
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
            if(ModelState.IsValid)
            {
                var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);
                if(user_exist != null)
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
                

                if(is_created.Succeeded)
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                    var role = new IdentityRole("User");
                    await _userManager.AddToRoleAsync(new_user, "User");
                    IList<string> roles = new List<string>
                    {
                        role.Name
                    };
                    var token = 

                }
            }
        }
    }
}
