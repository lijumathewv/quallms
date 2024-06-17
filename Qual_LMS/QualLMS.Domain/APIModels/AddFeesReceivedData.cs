using QualLMS.Domain.Models;

namespace QualLMS.Domain.APIModels
{
    public class AddFeesReceivedData : FeesReceivedData
    {
        public List<UserAllData> Students { get; set; }

        public List<StudentCourse> Courses { get; set; }

        public List<Fees> Fees { get; set; }
    }
}
