using Application.DTOs;
using Application.DTOs.ApplicationUser;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Application.InfraInterfaces;
using System.Web;
using Microsoft.Extensions.Options;
using Application.ConfigSettings;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Domain.Enums;

namespace Application.Services.Implementations.Auth
{
    public class UserAuthService : BaseService, IUserAuthService
    {
        private readonly ILogger<UserAuthService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEncryptionService _encryptionService;
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOTPService _otpService;
        private readonly AppEndpointSettings _appEndpointSettings;

        public UserAuthService(ILogger<UserAuthService> logger , IWebHostEnvironment environment,UserManager<ApplicationUser> userManager
            , ITokenService tokenService,IOptions<AppEndpointSettings> appEndpointSettings, SignInManager<ApplicationUser> signInManager ,
            IOTPService otpService, IEncryptionService encryptionService,ApplicationDbContext context, IHttpContextAccessor accessor) : base(accessor)
        {
            _logger = logger;
            _environment = environment;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _appEndpointSettings = appEndpointSettings.Value;
            _otpService = otpService;
            _encryptionService = encryptionService;
            _context = context;
        }

        public async Task<BaseResponse> RegisterUser(RegisterDTO registerUser)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == registerUser.Email);
            if (user != null)
            {
                return BaseResponse.Failure("26", "Duplicate Record Found - Email already exist");
            }


            var store = new Store
            {
                Name = registerUser.StoreName,
                Address = registerUser.Address
            };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                PhoneNumber = registerUser.PhoneNumber,
                StoreId  = store.Id                
            };
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.Admin.ToString());
                await _context.SaveChangesAsync();

                var createOTP = await _otpService.CreateOTP(user.Id, OTPActions.ConfirmAccount.ToString());
                var otpCode = createOTP.Data;
                // Send Notification Email for email confirmation

                return BaseResponse.Success();
            }
            string error = result.Errors.FirstOrDefault().Description;
            _logger.LogInformation($"Register user terminated [Reason : Creating user failed | Error : {error}]\n");
            return BaseResponse.Failure("30", error);
        }

        public async Task<BaseResponse> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users.Include(x => x.Store).SingleOrDefaultAsync(x => x.Email == loginDTO.Email);
            if (user is null)
            {
                _logger.LogInformation($"Login Terminated [Reason : Email not found | Email :  {loginDTO.Email}]");
                return BaseResponse.Failure("12", "Login failed - Email or password do not match for an existing user");
            }
            if (user.IsDeactivated)
            {
                _logger.LogInformation($"Login Terminated [Reason : Account is not active | Email :  {loginDTO.Email}]");
                return BaseResponse.Failure("12", "Login failed - Your account has been deactiavted. Please contact your admin for more details");
            }
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                _logger.LogInformation($"Login terminated [Reason : USer is locked out | Email : {loginDTO.Email}] \n");
                return BaseResponse.Failure("12", "Login failed - Your account is locked please try again with correct email and/or password in 60 minutes");
            }
            if (!user.EmailConfirmed)
            {
                _logger.LogInformation($"Login Terminated [Reason : Email not confirmed | Email :  {loginDTO.Email}]");
                return BaseResponse.Failure("12", "Kindly confirm your email");
            }
            var passwordCheck = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, true);
            if (passwordCheck.IsLockedOut)
            {
                _logger.LogInformation($"Login terminated [Reason : USer is locked out | Email : {loginDTO.Email}] \n");
                return BaseResponse.Failure("12", "Login failed - Your account is locked please try again with correct email and/or password in 60 minutes");
            }
            if (!passwordCheck.Succeeded)
            {
                _logger.LogInformation($"Login terminated [Reason : Password Check Failed | Email : {loginDTO.Email}] \n");
                return BaseResponse.Failure("12", "Login failed - Email or password do not match for an existing user");
            }
            await _userManager.ResetAccessFailedCountAsync(user);
            var authResponse = await _tokenService.GetAuthenticationResultForUserAsync(user);
            return authResponse;
        }

        public async Task<BaseResponse> RefreshToken(RefreshTokenDTO refreshToken)
        {
            _logger.LogInformation($"Refresh Token Request processing \n");
            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken.Token);
            var userId = principal.Identity.Name;
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogInformation($"Refresh Token terminated [Reason : User not found | Email : {user.Email}] \n");
                return BaseResponse.Failure("25", "User could not found");
            }
            if (user.RefreshToken != refreshToken.RefreshToken)
            {
                _logger.LogInformation($"Login terminated [Reason : Invalid Refresh Token | Email : {user.Email}] \n");
                return BaseResponse.Failure("12", "Invalid refresh token");
            }
            if (user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _logger.LogInformation($"Login terminated [Reason : Refresh Token has expired | Email : {user.Email}] \n");
                return BaseResponse.Failure("12", "Invalid refresh token - token has expired");
            }

            var authResponse = await _tokenService.GetAuthenticationResultForUserAsync(user);
            return authResponse;
        }

        public async Task<BaseResponse> ForgotPassword(ForgotPasswordDTO forgotPassword)
        {
            _logger.LogInformation($"Forgot Password Processing [Email : {forgotPassword.Email}] \n");
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var email = user.UserName;

                var passwordResetLink = $"{_appEndpointSettings.FrontendBaseUrl}{_appEndpointSettings.ResetPassword}?email={HttpUtility.UrlEncode(email)}" +
                 $"&emailToken={HttpUtility.UrlEncode(token)}";

                // Send reset password notification

                return BaseResponse.Success("A password reset link would be sent to the email address if it exist");
            }
            _logger.LogInformation($"Forgot Password Terminated [ Reason :Email Does not exist | Email : {forgotPassword.Email}] \n");
            return BaseResponse.Failure("00", "A password reset link would be sent to the email address if it exist");
        }

        public async Task<BaseResponse> ResetPassword(string email, string emailToken, ResetPasswordDTO resetPasswordModel)
        {
            _logger.LogInformation($"Reset Password Processing [Email : {email} ] \n");

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                _logger.LogInformation($"ResetPassword terminated [Reason : User not found | Email : {email}] \n");
                return BaseResponse.Failure("25", "User record not found");
            }

            emailToken = emailToken.Replace(" ", "+");
            var userPassword = await _userManager.ResetPasswordAsync(user, emailToken, resetPasswordModel.Password);
            if (userPassword.Succeeded)
            {
                user.EmailConfirmed = true;
                user.LockoutEnd = null;
                await _userManager.ResetAccessFailedCountAsync(user);
                await _userManager.UpdateAsync(user);

                // Send Email for password change success
                return BaseResponse.Success("Password was changed successfully");
            }
            else if (!userPassword.Succeeded && userPassword.Errors.Any(x => x.Code == "InvalidToken"))
            {
                _logger.LogInformation($"Reset Password terminated [ Reason :Invalid Token | Email : {email}] \n");
                return BaseResponse.Failure("12", "Invalid Token");

            }
            return BaseResponse.Failure("12", userPassword.Errors.FirstOrDefault().Description);
        }

        public async Task<BaseResponse> ChangePassword(ChangePasswordDTO changePassword)
        {
            _logger.LogInformation($"Change Password [UserID :{UserId}]\n");

            if (changePassword.NewPassword == changePassword.CurrentPassword)
            {
                return BaseResponse.Failure("30", "Current password cannot be the same with new password");
            }

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == UserId);

            var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                await _userManager.UpdateAsync(user);

                return BaseResponse.Success("Password was changed successfully");
            }
            return BaseResponse.Failure("12", result.Errors.FirstOrDefault().Description);
        }

        public async Task<BaseResponse> ConfirmAccount(ConfirmAccountModel confirmAccount)
        {
            _logger.LogInformation($"Confirm Account [ Email : {confirmAccount.Email} ]\n");
            var user = await _context.Users.Include(x => x.Store).SingleOrDefaultAsync(x => x.Email == confirmAccount.Email);
            if (user is null) return BaseResponse.Failure("25", "User records not found" );

            var confirmOTP = await _otpService.ValidateOTP(user.Id, confirmAccount.OTP, OTPActions.ConfirmAccount.ToString());
            if (confirmOTP.Code != "00")
            {
                _logger.LogInformation($"Confirm Account Terminate [Reason : OTP is invalid | Email : {confirmAccount.Email} | UserId : {user.Id} ]\n");
                return BaseResponse.Failure( confirmOTP.Code, confirmOTP.Description );
            }

            _logger.LogInformation($"User was confirmed successfully [Email :{confirmAccount.Email}] \n");
            user.EmailConfirmed = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            var authResponse = await _tokenService.GetAuthenticationResultForUserAsync(user);
            return authResponse;
        }

        public async Task<BaseResponse> ResendConfirmationOTP(string email)
        {
            _logger.LogInformation($" Resent Confirm Account OTP [ Email : {email} ]\n");
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return BaseResponse.Failure( "25", "User record not found" );

            var createOTP = await _otpService.CreateOTP(user.Id, OTPActions.ConfirmAccount.ToString());
            var otpCode = createOTP.Data;

            // Send OTP notification mail 

            return BaseResponse.Success() ;
        }
    }
}
