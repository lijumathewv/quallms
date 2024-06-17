using QualLMS.Domain.Models;

namespace QualLMS.Domain.APIModels
{
    public class AddFeesReceivedData : FeesReceivedData
    {
        public List<UserAllData> Students { get; set; } = null!;

        public List<StudentCourse> Courses { get; set; } = null!;

        public List<Fees> Fees { get; set; } = null!;
    }
}
