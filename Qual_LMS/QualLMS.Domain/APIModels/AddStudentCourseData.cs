using QualLMS.Domain.Models;

namespace QualLMS.Domain.APIModels
{
    public class AddStudentCourseData
    {
        public StudentCourseData Data { get; set; }

        public List<Course> Courses { get; set; }

        public List<UserAllData> Users { get; set; }
    }
}
