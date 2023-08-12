using Microsoft.AspNetCore.Identity;
using productExpiry_system.models;

namespace productExpiry_system.Interface
{
    public interface IUserRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
        Task<JwToken> LoginAsync(LoginModel model);
    }
}
