using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class OperationResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public OperationResult()
        {
            Success = true;
            Message = String.Empty;
        }

        public static OperationResult CreateSuccessResult()
        {
            return CreateSuccessResult(null);
        }

        public static OperationResult CreateSuccessResult(string message)
        {
            return new OperationResult { Success = true, Message = message ?? String.Empty };
        }

        public static OperationResult CreateErrorResult(string message)
        {
            return new OperationResult { Success = false, Message = message ?? String.Empty };
        }

        public static OperationResult CreateErrorResult(Exception exception)
        {
            return CreateErrorResult(exception, false);
        }

        public static OperationResult CreateErrorResult(Exception exception, bool printStackTrace)
        {
            string message = exception.Message;
            if (printStackTrace)
            {
                message += Environment.NewLine + exception.StackTrace;
            }

            return new OperationResult { Success = false, Message = message };
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            return ToJson();
        }
    }
}
