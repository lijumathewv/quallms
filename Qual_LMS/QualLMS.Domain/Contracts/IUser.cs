using QualLMS.Domain.APIModels;
using static QualLMS.Domain.Models.ServiceResponses;

namespace QualLMS.Domain.Contracts
{
    public interface IUser
    {
        GeneralResponses Add(UserRegister model);
        ResponsesWithData Get();
    }
}
