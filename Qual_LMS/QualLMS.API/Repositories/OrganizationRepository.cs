using QualLMS.API.Data;
using QualLMS.Domain.Contracts;
using static QualLMS.Domain.Models.ServiceResponses;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.API.Repositories
{
    public class OrganizationRepository(DataContext dataContext) : IOrganization
    {
        public GeneralResponses Add(OrganizationData organization)
        {
            GeneralResponses responses;
            try
            {
                var org = dataContext.Organization.FirstOrDefault(o => o.EmailId == organization.EmailId);
                if (org == null)
                {
                    dataContext.Organization.Add(new Domain.Models.Organization
                    {
                        Id = Guid.NewGuid(),
                        EmailId = organization.EmailId,
                        FullName = organization.FullName,
                        PhoneNumber = organization.PhoneNumber,
                        Address = organization.Address,
                        DomainName = organization.DomainName,
                        OfficeGeoLocation = organization.OfficeGeoLocation
                    });
                    dataContext.SaveChanges();

                    responses = new GeneralResponses(true, "Data Updated!");
                }
                else
                {
                    throw new Exception("Organization already Exists!");
                }
            }
            catch (Exception ex)
            {
                responses = new GeneralResponses(false, ex.Message);
            }

            return responses;
        }

        public ResponsesWithData Get()
        {
            ResponsesWithData responses;
            try
            {
                var data = dataContext.Organization.OrderBy(o => o.FullName).ToList();
                if (data != null)
                {
                    responses = new ResponsesWithData(true, data, "Data found!");
                }
                else
                {
                    throw new Exception("Organization already Exists!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false,null!, ex.Message);
            }

            return responses;
        }
    }
}
