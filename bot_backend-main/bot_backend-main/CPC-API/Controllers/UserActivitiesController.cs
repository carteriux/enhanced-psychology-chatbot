using CPC.Domain.Aggregations.UserActivities;
using CPC.Domain.DTOs;
using CPC.Domain.DTOs.@base;
using CPC.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPC.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserActivitiesController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IServiceUserActivities _service;
        private readonly OperationResult<ResponseMessageDTO> operationResultMessage;
        private readonly OperationResult<ResponseFileDTO> operationResultFile;
        private readonly OperationResult<List<Useractivity>> operationResultUserActivities;
        private readonly OperationResult<Useractivity> operationResultUserActivity;

        public UserActivitiesController(ILogger<UserController> logger, IServiceUserActivities service)
        {
            this._service = service;
            this._logger = logger;
            this.operationResultMessage = new OperationResult<ResponseMessageDTO>();
            this.operationResultFile = new OperationResult<ResponseFileDTO>();
            this.operationResultUserActivities = new OperationResult<List<Useractivity>>();
            this.operationResultUserActivity = new OperationResult<Useractivity>();
        }

        [HttpGet("GetActivitiesByUserId/{idUser}")]
        public async Task<ActionResult> Get(int idUser)
        {

            var response = new OperationResult<List<Useractivity>>();
            try
            {
                response = await operationResultUserActivities.AsyncRun(async () => await this._service.GetActivitiesByUserIdAsync(idUser));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        [HttpGet("GetActivityById")]
        public async Task<ActionResult> GetActivityById(int id, int idUser)
        {

            var response = new OperationResult<Useractivity>();
            try
            {
                response = await operationResultUserActivity.AsyncRun(async () => await this._service.GetActivityByIdAsync(id, idUser));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }
        
        [HttpPost("ActivityQuestions")]
        public async Task<ActionResult> ActivityQuestions(RequestUpdateActivityDTO request)
        {
            var response = new OperationResult<ResponseMessageDTO>();

            try
            {
                response = await operationResultMessage.AsyncRun(async () => await this._service.UpdateActivityByUserIdAsync(request));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            return Ok(response);
        }

        [HttpPost("EndActivity")]
        public async Task<ActionResult> EndActivity(RequestEndActivityDTO request)
        {
            var response = new OperationResult<ResponseFileDTO>();

            try
            {
                response = await operationResultFile.AsyncRun(async () => await this._service.EndActivityByUserIdAsync(request));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }
            response.Data.MemoryStream.Position = 0;
            return File(response.Data.MemoryStream, "application/pdf", response.Data.FileName);
        }

        [HttpGet("GetFileActivity")]
        public async Task<ActionResult> GetFileActivity(int id, int idUser, string fileName)
        {
            var request = new RequestGetActivityDTO { id = id, idUser = idUser, FileName = fileName };
            var response = new OperationResult<ResponseFileDTO>();

            try
            {
                response = await operationResultFile.AsyncRun(async () => await this._service.GetFileActivityByUserIdAsync(request));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                //this._telemetry.TrackException(ex);
            }

            response.Data.MemoryStream.Position = 0;
            return File(response.Data.MemoryStream, "application/pdf", response.Data.FileName);
        }

        [HttpPost("ResetActivities")]
        public async Task<ActionResult> ResetActivities(int idUser)
        {
            var response = new OperationResult<ResponseMessageDTO>();

            try
            {
                response = await operationResultMessage.AsyncRun(async () => await this._service.ResetUserActivitiesAsync(idUser));
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
