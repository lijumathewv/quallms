using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using static QualLMS.Domain.Models.ServiceResponses;


namespace QualLMS.Domain.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponses> CreateAccount(UserRegister user);
        Task<LoginResponses> LoginAccount(Login user);
        ResponsesWithData AllUsers(Guid OrganizationId);
    }
}
