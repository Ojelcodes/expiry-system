using Application.ConfigSettings;
using Application.DTOs;
using Application.InfraInterfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.Data;

namespace Application.Services.Implementations
{
    public class OTPService : IOTPService
    {
        private readonly ILogger<OTPService> _logger;
        private readonly IEncryptionService _encryptionService;
        private readonly ApplicationDbContext _context;
        private readonly AppEndpointSettings _appEndpoint;

        public OTPService( ILogger<OTPService> logger, IEncryptionService encryptionService, IOptions<AppEndpointSettings> appEndpoint, ApplicationDbContext context)
        {
            _logger = logger;
            _encryptionService = encryptionService;
            _context = context;
            _appEndpoint = appEndpoint.Value;
        }

        public async Task<ResponseMessage<string>> CreateOTP(long userId, string action)
        {
            var otpCode = new Random().Next(100000, 999999).ToString();
            if (_appEndpoint.Environment == "Development" && (action.ToLower() == OTPActions.ConfirmAccount.ToString().ToLower()))
            {
                otpCode = "123456";
            }
            var userOTP = new OneTimePassword
            {
                Action = action,
                ApplicationUserId = userId,
                ExpiresAt = DateTime.Now.AddMinutes(7),
                OTP = _encryptionService.SHA512(otpCode)
            };
            _context.OneTimePasswords.Add(userOTP);
            await _context.SaveChangesAsync();
            return new ResponseMessage<string> { Code = "00", Description = "Approved or Created Successfully", Data = otpCode };
        }

        public async Task<BaseResponse> ValidateOTP(long userId, string otpCode, string action)
        {
            var hashedOTP = _encryptionService.SHA512(otpCode);
            var otp = await _context.OneTimePasswords.Where(x => x.ApplicationUserId == userId && x.Action.ToLower() == action.ToLower() && x.Status == false)
               .OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync();
            if (otp != null)
            {
                if (otp.ExpiresAt > DateTimeOffset.Now)
                {
                    if (!otp.Status)
                    {
                        if (otp.OTP == hashedOTP)
                        {
                            otp.Status = true;
                            _context.OneTimePasswords.Update(otp);
                            await _context.SaveChangesAsync();
                            return BaseResponse<OneTimePassword>.Success(otp, "00", "Approved or completed Successfully");
                        }
                        _logger.LogInformation($"Validate OTP Terminated [Reason : OTP is old | UserId : {userId} | Action : {action} ]\n");
                        return BaseResponse<OneTimePassword>.Failure("12", "OTP is invalid" );
                    }
                    _logger.LogInformation($"Validate OTP Terminated [Reason : OTP has been used | UserId : {userId} | Action : {action} ]\n");
                    return BaseResponse<OneTimePassword>.Failure("12", "OTP is invalid");
                }
                _logger.LogInformation($"Validate OTP Terminated [Reason : OTP has expired | UserId : {userId} | Action : {action} ]\n");
                return BaseResponse<OneTimePassword>.Failure("12", "OTP is invalid" );
            }
            _logger.LogInformation($"Validate OTP Terminated [Reason : No OTP found | UserId : {userId} | Action : {action} ]\n");
            return BaseResponse<OneTimePassword>.Failure("12", "OTP is invalid");
        }
    }
}
