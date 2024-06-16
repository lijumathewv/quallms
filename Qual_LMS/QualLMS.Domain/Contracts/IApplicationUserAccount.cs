using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualvationLibrary;
using static QualvationLibrary.ServiceResponse;


namespace QualLMS.Domain.Contracts
{
    public interface IApplicationUserAccount
    {
        GeneralResponses CreateAccount(UserRegister user);
        LoginResponses LoginAccount(Login user);
        ResponsesWithData AllUsers(Guid OrganizationId);
        ResponsesWithData GetUser(string Id);
        ResponsesWithData GetTeachers(string OrgId);
        ResponsesWithData GetStudents(string OrgId);
    }
}
