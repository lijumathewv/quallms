using QualLMS.Domain.APIModels;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface IFeesReceived
    {
        ResponsesWithData GetAll(string OrgId);
        ResponsesWithData Get(string Id);
        GeneralResponses AddOrUpdate(FeesReceivedData model);
        GeneralResponses Delete(string Id);

    }
}
