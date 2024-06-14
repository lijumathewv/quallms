using QualLMS.Domain.APIModels;
using QualvationLibrary;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface IUser
    {
        GeneralResponses Add(UserRegister model);
        ResponsesWithData Get();
    }
}
