using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using productExpiry_system.entities;
using productExpiry_system.Interface;
using productExpiry_system.models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace productExpiry_system.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

      public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
      {
            var NewUser = new ApplicationUser()
            {
                Email = signUpModel.Email,
                PhoneNumber = signUpModel.PhoneNumber,
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                UserName = signUpModel.FirstName,
                CreatedAt= signUpModel.CreatedDate,
                EmailConfirmed = true
            };
           return await userManager.CreateAsync(NewUser, signUpModel.Password);
      }
        public async Task<JwToken> LoginAsync(LoginModel model)
        {
            var ReturnUser = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!ReturnUser.Succeeded )
            {
                return null;

            }

            var authenticationclaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, model.Email),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var AuthSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:secret"]));
            var Token = new JwtSecurityToken(
                issuer: _configuration[" JWT:validIssuer"],
                audience: _configuration["JWT:validAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authenticationclaims,
                signingCredentials: new SigningCredentials(AuthSignInKey, SecurityAlgorithms.HmacSha256Signature)
                );
            var user = await userManager.FindByEmailAsync(model.Email);
            var token = new JwToken()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(Token)

            };

            return token;
        }

    }
}
