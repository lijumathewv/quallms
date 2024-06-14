using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface IOrganization
    {
        ResponsesWithData Get();
        ResponsesWithData Get(string Id);
        GeneralResponses AddOrUpdate(Organization model);
        GeneralResponses Delete(string Id);
    }
}
