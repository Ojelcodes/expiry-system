
using Application.DTOs;
using Application.DTOs.ApplicationUser;
using Application.Services.Interfaces;
using BusinessZoneCentral_UserAPI.Controllers;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace productExpiry_system.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserAuthService _userAuthService;
        public AuthController(ILogger<AuthController> logger, IUserAuthService userAuthService)
        {
            _logger = logger;
            _userAuthService = userAuthService;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerUser)
        {
           if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
           return HandleResult(await _userAuthService.RegisterUser(registerUser));
        }

        /// <summary>
        /// Signin User
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse<LoginResponseDTO>))]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _userAuthService.Login(login));
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPassword)
        {
            _logger.LogInformation($"Forgot Password Request \n");
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _userAuthService.ForgotPassword(forgotPassword));
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="email"></param>
        /// <param name="emailToken"></param>
        /// <returns></returns>
        [HttpPatch("[action]")]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO viewModel, [Required, EmailAddress] string email, [Required] string emailToken)
        {
            _logger.LogInformation($"Reset Password Request \n");
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _userAuthService.ResetPassword(email, emailToken, viewModel));
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPatch("[action]")]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse<LoginResponseDTO>))]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO refreshToken)
        {
            _logger.LogInformation($"Refresh Token Request \n");
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _userAuthService.RefreshToken(refreshToken));
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("[action]")]
        [Authorize]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            _logger.LogInformation($"Change Password Request \n");
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _userAuthService.ChangePassword(model));
        }

        /// <summary>
        /// Confirm Account
        /// </summary>
        /// <param name="confirmAccount"></param>
        /// <returns></returns>
        [HttpPatch("[action]")]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        public async Task<IActionResult> ConfirmAccount(ConfirmAccountModel confirmAccount)
        {
            _logger.LogInformation($"Confrim Account Request \n");
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _userAuthService.ConfirmAccount(confirmAccount));
        }

        /// <summary>
        /// Resned Confirmation OTP
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        public async Task<IActionResult> ResendOTP(ResendOTPDTO resendOTP)
        {
            _logger.LogInformation($"Resend Confrim Account Request \n");
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _userAuthService.ResendConfirmationOTP(resendOTP.Email));
        }
    }
}
