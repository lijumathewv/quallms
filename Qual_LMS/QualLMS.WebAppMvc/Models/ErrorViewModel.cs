using QualLMS.WebAppMvc.Controllers;

namespace QualLMS.WebAppMvc.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorMessage { get; set; }

        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string ErrorPath { get; set; }
        public Exception? InnerException { get; set; }
    }
}
