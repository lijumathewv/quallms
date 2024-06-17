using Microsoft.AspNetCore.Mvc.Rendering;
using QualLMS.Domain.Models;

namespace QualLMS.Domain.APIModels
{
    public class TeacherCalendarData
    {
        public string? Data { get; set; }

        public List<UserAllData> Users { get; set; } = null!;

        public List<Course> Courses { get; set; } = null!;
    }
}
