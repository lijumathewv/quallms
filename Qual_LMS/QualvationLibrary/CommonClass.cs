using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QualvationLibrary
{
    public record UserSession(string? Id, string? Name, string? Email, string? Role);

    public enum Roles
    {
        SuperAdmin = 0,
        Admin = 1,
        Students = 2,
        Teachers = 3,
        Parents = 4,
        Employer = 5,
        Employee = 6,
        ServiceProvider = 7,
        Franchisee = 8,
        None = -1
    }

    public class ResultCommon
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("returnmodel")]
        public string? ReturnModel { get; set; }

        public APIError? ApiError { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class UserData
    {
        public string UserId { get; set; } = string.Empty;
        public Guid OrgId { get; set; }

        public string Data { get; set; } = string.Empty;
    }

    public class APIError
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>>? Errors { get; set; }

    }

    public class TimeComparisonAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public TimeComparisonAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (TimeOnly?)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            var comparisonValue = (TimeOnly?)property.GetValue(validationContext.ObjectInstance);

            if (currentValue.HasValue && comparisonValue.HasValue && currentValue > comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} should not be greater than {_comparisonProperty}");
            }

            return ValidationResult.Success!;
        }
    }

    public class AmountComparisonAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public AmountComparisonAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (int?)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            var comparisonValue = (int?)property.GetValue(validationContext.ObjectInstance);

            if (currentValue.HasValue && comparisonValue.HasValue && currentValue > comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} should not be greater than {_comparisonProperty}");
            }

            return ValidationResult.Success!;
        }
    }

    public class CustomDateTimeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime == DateTime.MinValue)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success!;
        }
    }

    public class RequiredGuid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Guid guid && guid != Guid.Empty)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "The GUID field is required.");
        }
    }

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal server error",
                Detailed = exception.Message,
                StackTrace = exception.StackTrace
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public static class EnumHelper
    {
        public static SelectList ToSelectList<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            var values = Enum.GetValues(typeof(TEnum))
                             .Cast<TEnum>()
                             .Select(e => new
                             {
                                 Value = e,
                                 Text = e.ToString()
                             });

            return new SelectList(values, "Value", "Text");
        }
    }
}
