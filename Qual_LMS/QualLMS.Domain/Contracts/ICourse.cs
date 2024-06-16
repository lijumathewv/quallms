using QualLMS.Domain.Models;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface ICourse
    {
        ResponsesWithData GetAll(string OrgId);
        ResponsesWithData Get(string Id);
        ResponsesWithData GetFees(string Id);
        ResponsesWithData GetStudents(string Id);
        ResponsesWithData GetTeachers(string Id);
        GeneralResponses AddOrUpdate(Course model);
        GeneralResponses Delete(string Id);

    }
}
