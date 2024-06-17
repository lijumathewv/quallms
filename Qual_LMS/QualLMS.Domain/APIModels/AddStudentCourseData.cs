using QualLMS.Domain.Models;

namespace QualLMS.Domain.APIModels
{
    public class AddStudentCourseData
    {
        public StudentCourseData Data { get; set; } = null!;

        public List<Course> Courses { get; set; } = null!;

        public List<UserAllData> Users { get; set; } = null!;
    }
}
