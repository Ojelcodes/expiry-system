using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using productExpiry_system.entities;
using productExpiry_system.Interface;
using productExpiry_system.models;

namespace productExpiry_system.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        private UserManager<ApplicationUser> _userManager;

        public UserController (IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            //var User = await _userRepository.SignUpAsync(model);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
                return BadRequest("User with this email already exists");

            var created = await _userRepository.SignUpAsync(model);
            if (!created.Succeeded)
                return BadRequest(created.Errors);

            var returnedUser = await _userManager.FindByEmailAsync(model.Email);

            return Ok(new { id = returnedUser.Id, Name = returnedUser.FirstName, Email = returnedUser.Email, PhoneNumber = returnedUser.PhoneNumber });
        }
        
    }
}
