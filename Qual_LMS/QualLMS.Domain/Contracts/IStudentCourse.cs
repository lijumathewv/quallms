using QualLMS.Domain.APIModels;
using QualvationLibrary;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface IStudentCourse
    {
        ServiceResponse.ResponsesWithData Get();
        ServiceResponse.ResponsesWithData GetStudentCourse(string StudentId);
        ServiceResponse.ResponsesWithData GetBalanceAmount(string StudentId, string CourseId);
        ServiceResponse.ResponsesWithData Get(string Id);
        GeneralResponses AddOrUpdate(StudentCourseData model);
        GeneralResponses Delete(string Id);

    }
}
