using Microsoft.AspNetCore.Identity;

namespace Weterynarz.Models
{
    public class ApplicationUser : IdentityUser
    {

        public new int Id { get; set; }
    }
}
