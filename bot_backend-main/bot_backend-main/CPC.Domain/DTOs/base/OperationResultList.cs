using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs.@base
{
    public class OperationResultList<T>
    {
        //private readonly ILogService _logService;

        //public OperationResultList(ILogService logService)
        //{
        //    _logService = logService;
        //}

        public OperationResultList() { }

        public List<T> Data { get; set; }

        public bool Success { get; set; }

        public string Error_Message { get; set; }

        public async Task<OperationResultList<T>> AsyncRun(Func<Task<List<T>>> func)
        {
            try
            {
                Data = await func.Invoke();
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
                Error_Message = e.Message;
                //_logService.WriteException(e.ToString());
            }
            return this;
        }
    }
}
