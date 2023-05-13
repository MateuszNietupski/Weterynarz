using Microsoft.AspNetCore.Mvc;
using Weterynarz.Data;

namespace Weterynarz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private static AppDbContext _context;
        public ApplicationUserController(AppDbContext context) {
            _context = context;
        }
        //sagadsgasgaeayrhh
    }
}
