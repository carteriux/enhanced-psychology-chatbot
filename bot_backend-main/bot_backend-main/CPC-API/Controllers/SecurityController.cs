using CPC.Domain.Aggregations.Users;
using CPC.Domain.DTOs;
using CPC.Domain.DTOs.@base;
using CPC.Domain.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CPC.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IServiceUsers _serviceUsers;
        private readonly OperationResult<ResponseLoginDTO> operationResultUser;
        private readonly APIBaseSettings _apiSetting;        

        public SecurityController(ILogger<UserController> logger, IServiceUsers serviceUsers, IOptions<APIBaseSettings> apiSettings) 
        {
            this._logger = logger;
            this._serviceUsers = serviceUsers;
            this.operationResultUser = new OperationResult<ResponseLoginDTO>();
            this._apiSetting = apiSettings.Value;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO request)
        {
            var response = new OperationResult<ResponseLoginDTO>();
            try
            {

                response = await operationResultUser.AsyncRun(async () => await this._serviceUsers.ValidateUserAsync(request.ID, request.ID, request.Password, this._apiSetting.Jwt.Key));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }
            return Ok(response);
        }
    }
}
