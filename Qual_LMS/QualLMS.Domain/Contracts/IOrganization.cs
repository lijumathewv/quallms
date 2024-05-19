using QualLMS.Domain.APIModels;
using static QualLMS.Domain.Models.ServiceResponses;

namespace QualLMS.Domain.Contracts
{
    public interface IOrganization
    {
        GeneralResponses Add(OrganizationData model);
        ResponsesWithData Get();
    }
}
