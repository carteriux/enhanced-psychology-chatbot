using CPC.Domain.Aggregations.Users;
using CPC.Domain.DTOs;
using CPC.Domain.DTOs.@base;
using CPC.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CPC.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IServiceUsers _serviceUsers;
        private readonly OperationResult<ResponseUsersDTO> operationResultUsers;
        private readonly OperationResult<ResponseUserDTO> operationResultUser;
        private readonly OperationResult<bool> operationResultBool;        

        public UserController(ILogger<UserController> logger, IServiceUsers serviceUsers)
        {
            this._logger = logger;
            this._serviceUsers = serviceUsers;
            this.operationResultUsers = new OperationResult<ResponseUsersDTO>();
            this.operationResultUser = new OperationResult<ResponseUserDTO>();
            this.operationResultBool = new OperationResult<bool>();            
        }

        // GET: api/<UserController>
        [HttpGet]        
        public async Task<ActionResult> Get()
        {

            var response = new OperationResult<ResponseUsersDTO>();
            try
            {
                response = await operationResultUsers.AsyncRun(async () => await this._serviceUsers.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        // GET: api/<UserController>/cohort
        [HttpGet("cohort")]
        public async Task<ActionResult> GetByCohort([FromQuery] string? cohort)
        {
            var response = new OperationResult<ResponseUsersDTO>();
            try
            {
                response = await operationResultUsers.AsyncRun(async () => await this._serviceUsers.GetUsersByCohortAsync(cohort));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var response = new OperationResult<ResponseUserDTO>();
            try
            {
                response = await operationResultUser.AsyncRun(async () => await this._serviceUsers.GetUserByIdAsync(id));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult> Post(RequestCreateUserDTO request)
        {
            var response = new OperationResult<ResponseUserDTO>();

            try
            {
                response = await operationResultUser.AsyncRun(async () => await this._serviceUsers.CreateUserAsync(request));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("createmultipleusers")]
        public async Task<ActionResult> Post(List<RequestCreateUserDTO> request)
        {
            var response = new OperationResult<ResponseUsersDTO>();

            try
            {
                response = await operationResultUsers.AsyncRun(async () => await this._serviceUsers.CreateMultipleUsersAsync(request));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        // PUT api/<UserController>/5
        [HttpPut()]
        public async Task<ActionResult> Put(User request)
        {
            var response = new OperationResult<bool>();

            try
            {
                response = await operationResultBool.AsyncRun(async () => await this._serviceUsers.UpdateUserAsync(request));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        // DELETE api/<UserController>/5
        [HttpDelete()]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new OperationResult<bool>();

            try
            {
                response = await operationResultBool.AsyncRun(async () => await this._serviceUsers.DeleteUserAsync(id));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }
    }
}
