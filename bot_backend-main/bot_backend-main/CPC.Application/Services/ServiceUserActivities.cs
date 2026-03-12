using CPC.Domain.Aggregations.UserActivities;
using CPC.Domain.Aggregations.Users;
using CPC.Domain.DTOs;
using CPC.Domain.Entities;
using CPC.Domain.ValueObject;
using CPC.Infrastructure.Crosscutting.Helpers;
using CPC.Infrastructure.CrossCutting.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CPC.Application.Services
{
    public class ServiceUserActivities : IServiceUserActivities
    {
        private readonly IRepositoryUserActivities _repository;
        private readonly IRepositoryUsers _userRepository;
        private readonly APIBaseSettings _settings;
        private readonly GoogleStorageHelper _storageHelper;
        private static int questionMaxCount = 60;
        private static int questionAlertCount = 50;
        private readonly string buckeName = "activities-storage";

        public ServiceUserActivities(IRepositoryUserActivities repository, IRepositoryUsers userRepository, IOptionsSnapshot<APIBaseSettings> settings) 
        { 
            this._repository = repository;
            this._userRepository = userRepository;
            this._settings = settings.Value;
            this._storageHelper = new GoogleStorageHelper(buckeName);
        }

        public async Task<List<Useractivity>> GetActivitiesByUserIdAsync(int idUser)
        {
            var result = await this._repository.GetActivitiesByUserIdAsync(idUser);            
            return result;
        }

        public async Task<Useractivity> GetActivityByIdAsync(int id, int idUser)
        {
            var result = await this._repository.GetActivitysById(id, idUser);            
            return result;
        }

        public async Task<ResponseMessageDTO> UpdateActivityByUserIdAsync(RequestUpdateActivityDTO request)
        {
            var response = new ResponseMessageDTO();
            var activity = await this._repository.GetActivitysById(request.id, request.idUser);
            var validate = ValidateNumberOfMessages(activity.Count);
            response.Result = validate.Item1;

            if (!validate.Item2)
            {
                var user = await this._userRepository.GetUserByIdAsync(request.idUser);

                var requestChatBot = new RequestChatBotDTO
                {
                    activity_id = "DA" + activity.IdActivity.ToString(),
                    user_id = user.EnrollmentNumber,
                    question = request.question
                };

                var responseCB = await WSHelper.CallWebServiceJsonAsync<ResponseChatBotDTO>(this._settings.ExternalAPIs.ChatBot + "/bot", requestChatBot.TextJsonSerializerToString(), HttpMethod.Post);

                if (responseCB.result?.code == 200)
                {
                    activity.Count++;

                    if (activity.Count == 1) { activity.StartDateTime = DateTime.Now; }
                    if (activity.Count == questionMaxCount) { activity.EndDateTime = DateTime.Now; }

                    activity.ProgressPercentage = (decimal?)(((double)activity.Count / questionMaxCount) * 100);

                    var result = await this._repository.UpdateActivityByUserIdAsync(activity);
                    if (result != null)
                    {
                        response.Message = responseCB.data;
                    }                    
                }
                else
                {
                    new Exception("Error en el servicio de chatbot");
                }
                return response;
            }
            else if (validate.Item2 && string.IsNullOrEmpty(activity.FilePath))
            {
                await this.EndActivityByUserIdAsync(new RequestEndActivityDTO { id = request.id, idUser = request.idUser });
            }            

            return response;
        }

        public async Task<ResponseFileDTO> EndActivityByUserIdAsync(RequestEndActivityDTO request)
        {
            var memoryStream = new MemoryStream();

            var activity = await this._repository.GetActivitysById(request.id, request.idUser);

            if (activity != null && string.IsNullOrEmpty(activity.FilePath))
            {
                var user = await this._userRepository.GetUserByIdAsync(request.idUser);

                var requestChatBot = new RequestSaveActivityDTO
                {
                    activity_id = "DA" + activity.IdActivity.ToString(),
                    user_id = user.EnrollmentNumber
                };

                var responseCB = await WSHelper.CallAPIStreamAsync(this._settings.ExternalAPIs.ChatBot + "/generate_chat_pdf", requestChatBot.TextJsonSerializerToString(), HttpMethod.Post);

                var filename = $"{requestChatBot.activity_id}_{user.EnrollmentNumber}.pdf";
                await responseCB.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await this._storageHelper.UploadFileAsync(memoryStream, filename);

                activity.FilePath = filename;
                activity.EndDateTime = DateTime.Now;
                await this._repository.UpdateActivityByUserIdAsync(activity);
            }
            else
            {
                throw new Exception("La actividad ya se encuentra cerrada!");
            }

            return new ResponseFileDTO { MemoryStream = memoryStream, FileName = activity.FilePath };

        }

        public async Task<ResponseFileDTO> GetFileActivityByUserIdAsync(RequestGetActivityDTO request)
        {
            var activity = await this._repository.GetActivitysById(request.id, request.idUser);
            
            if (activity.FilePath == null && activity.FilePath != request.FileName)
            {
                throw new Exception("Actividad de usuario no encontrada");
            }            

            var memoryStream = await this._storageHelper.DownloadFileAsync(activity.FilePath);
            return new ResponseFileDTO { MemoryStream = memoryStream, FileName = activity.FilePath };
        }

        public async Task<ResponseMessageDTO> ResetUserActivitiesAsync(int idUser)
        {
            var response = new ResponseMessageDTO();
            try
            {
                var activities = await this._repository.GetActivitiesByUserIdAsync(idUser);
                
                foreach (var activity in activities)
                {
                    // Reset activity progress
                    activity.Count = 0;
                    activity.ProgressPercentage = 0;
                    activity.StartDateTime = DateTime.Now;
                    activity.EndDateTime = null;
                    activity.FilePath = null;
                    
                    await this._repository.UpdateActivityByUserIdAsync(activity);
                }

                // Limpiar historial de Firestore para que el bot Python no bloquee al alumno
                var user = await this._userRepository.GetUserByIdAsync(idUser);
                var resetHistoryRequest = new RequestResetHistoryDTO { user_id = user.EnrollmentNumber };
                await WSHelper.CallWebServiceJsonAsync<object>(
                    this._settings.ExternalAPIs.ChatBot + "/reset_history",
                    resetHistoryRequest.TextJsonSerializerToString(),
                    HttpMethod.Post);

                response.Result = new ResultMessage 
                {
                    Success = true,
                    Warning_Message = "Actividades del usuario restauradas exitosamente"
                };
                response.Message = "Actividades del usuario restauradas exitosamente";
            }
            catch (Exception ex)
            {
                response.Result = new ResultMessage 
                {
                    Success = false,
                    Warning_Message = $"Error al restaurar actividades: {ex.Message}"
                };
                response.Message = $"Error al restaurar actividades: {ex.Message}";
            }
            
            return response;
        }

        private Tuple<ResultMessage, bool> ValidateNumberOfMessages(int numberOfMessages)
        {
            var message = new ResultMessage();
            Tuple<ResultMessage, bool> result = null;

            if (numberOfMessages == questionAlertCount)
            {
                message.Success = true;
                message.Warning_Message = "Quedan 10 preguntas por realizar!";
                result = Tuple.Create(message, false);
            }
            else if(numberOfMessages == questionMaxCount)
            {
                message.Success = true;
                message.Warning_Message = "Numero maximo de pregunta realizado!";
                result = Tuple.Create(message, true);
            }
            else
            {
                result = Tuple.Create(message, false);
            }

            return result;
        }
    }
}
