using QualLMS.Domain.Models;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface IFees
    {
        ResponsesWithData Get();
        ResponsesWithData Get(string Id);
        GeneralResponses AddOrUpdate(Fees model);
        GeneralResponses Delete(string Id);

    }
}
