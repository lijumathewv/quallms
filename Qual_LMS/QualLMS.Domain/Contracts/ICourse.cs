using QualLMS.Domain.Models;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface ICourse
    {
        ResponsesWithData Get();
        ResponsesWithData Get(string Id);
        ResponsesWithData GetStudents(string Id);
        ResponsesWithData GetTeachers(string Id);
        GeneralResponses AddOrUpdate(Course model);
        GeneralResponses Delete(string Id);

    }
}
