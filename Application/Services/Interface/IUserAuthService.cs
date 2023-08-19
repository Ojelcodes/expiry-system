using Application.DTOs;
using Application.DTOs.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserAuthService
    {
        Task<BaseResponse> ChangePassword(ChangePasswordDTO changePassword);
        Task<BaseResponse> ForgotPassword(ForgotPasswordDTO forgotPassword);
        Task<BaseResponse> Login(LoginDTO loginDTO);
        Task<BaseResponse> RefreshToken(RefreshTokenDTO refreshToken);
        Task<BaseResponse> RegisterUser(RegisterDTO registerUser);
        Task<BaseResponse> ConfirmAccount(ConfirmAccountModel confirmAccount);
        Task<BaseResponse> ResendConfirmationOTP(string email);
        Task<BaseResponse> ResetPassword(string email, string emailToken, ResetPasswordDTO resetPasswordModel);
    }
}
