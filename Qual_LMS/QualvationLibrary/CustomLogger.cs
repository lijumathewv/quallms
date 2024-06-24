using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace QualvationLibrary
{
    public class Logger
    {
        public string BaseURL { get; set; } = string.Empty;

        public bool IsLogged { get; set; } = false;

        public bool IsError { get; set; } = false;
        public bool IsSuccess { get; set; } = false;

        public string SuccessMessage { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;
        public string InnerMessage { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;

        public LoginProperties LoginDetails { get; set; }

        public void ClearMessages()
        {
            IsError = false;
            IsSuccess = false;
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            InnerMessage = string.Empty;
            StackTrace = string.Empty;
        }

        public string ParseAPIError(APIError error)
        {
            string ErrorMessage = error.Title;
            
            if (error.Errors != null)
            {
                foreach (var item in error.Errors)
                {
                    if (item.Value != null)
                    {
                        foreach (var er in item.Value)
                        {
                            ErrorMessage += "<br/>" + er;
                        }
                    }
                }
            }

            return ErrorMessage;
        }

        public DateTime CurrentDateTime(DateTime date)
        {
            DateTime utcNow = date;
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, istTimeZone);
        }
    }

    public class CustomLogger(ILogger<CustomLogger> logger) : Logger
    {
        public void GenerateException(Exception exc)
        {
            IsError = true;

            ErrorMessage = exc.Message;
            LogMessage("Error occured:" + ErrorMessage, "Error");

            StackTrace = exc.StackTrace!;
            LogMessage("Error occured (StackTrace):" + StackTrace, "Error");

            if (exc.InnerException != null)
            {
                InnerMessage = exc.InnerException.Message;
                LogMessage("Error occured (InnerMessage):" + InnerMessage, "Error");
            }
        }

        public string GenerateDetailedException(Exception exc)
        {
            string Error=exc.Message;
            Error += "Stack Trace::" + exc.StackTrace!;
            if (exc.InnerException != null)
            {
                Error += "InnerMessage::" + exc.InnerException.Message;
            }

            return Error;
        }

        public void LogMessage(string Message, string type = "Information")
        {
            switch (type)
            {
                case "Information":
                    {
                        logger.LogInformation(Message);
                        break;
                    }
                case "Error":
                    {
                        logger.LogError(Message);
                        break;
                    }
                case "Warning":
                    {
                        logger.LogWarning(Message);
                        break;
                    }
                default:
                    {
                        logger.LogDebug(Message);
                        break;
                    }
            }
        }
    }
}
