using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BusinessZoneCentral_UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }

        protected ActionResult HandleResult(BaseResponse result)
        {
            if (result.Code == "00" || result.Code == "0") return Ok(result);
            if (result.Code == "25") return NotFound(result);
            return BadRequest(result);
        }
    }
}
