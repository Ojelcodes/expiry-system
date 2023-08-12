using Microsoft.AspNetCore.Mvc;
using productExpiry_system.Interface;
using productExpiry_system.models;

namespace productExpiry_system.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

       public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(loginModel);

            var Result = await _userRepository.LoginAsync(loginModel);
            if (Result == null)
            {
                return Unauthorized();
            }
            return Ok(Result);
        }
    }
}
