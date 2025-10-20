using System;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs.@base
{
    public class OperationResult<T>
    {        
        public OperationResult()
        {
            Error_Message = string.Empty;
            Data = Activator.CreateInstance<T>();
        }

        public T Data { get; set; }

        public bool Success { get; set; }

        public string Error_Message { get; set; }

        public int Error_Code { get; set; }

        public async Task<OperationResult<T>> AsyncRun(Func<Task<T>> func)
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
