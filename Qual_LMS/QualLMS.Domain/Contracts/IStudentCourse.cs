using QualLMS.Domain.APIModels;
using QualvationLibrary;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface IStudentCourse
    {
        ServiceResponse.ResponsesWithData GetAll(string OrgId);
        ServiceResponse.ResponsesWithData GetStudentCourse(string StudentId);
        ServiceResponse.ResponsesWithData GetBalanceAmount(string StudentId, string CourseId);
        ServiceResponse.ResponsesWithData Get(string Id);
        GeneralResponses AddOrUpdate(StudentCourseData model);
        GeneralResponses Delete(string Id);

    }
}
