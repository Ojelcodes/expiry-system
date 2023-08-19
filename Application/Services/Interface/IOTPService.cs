using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IOTPService
    {
        Task<ResponseMessage<string>> CreateOTP(long userId, string action);
        Task<BaseResponse> ValidateOTP(long userId, string encryptedOTP, string action);
    }
}
