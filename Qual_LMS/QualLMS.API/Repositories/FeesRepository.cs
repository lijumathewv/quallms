using QualLMS.API.Data;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.API.Repositories
{
    public class FeesRepository(DataContext context, CustomLogger logger) : IFees
    {
        public ServiceResponse.GeneralResponses AddOrUpdate(Fees model)
        {
            try
            {
                var data = context.Fees.FirstOrDefault(o => o.Id == model.Id);
                if (data == null)
                {
                    context.Fees.Add(model);
                }
                else
                {
                    data.FeesName = model.FeesName;
                    data.OrganizationId = model.OrganizationId;
                }
                context.SaveChanges();

                return new GeneralResponses(true, "Data Updated!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new GeneralResponses(false, "Error Occured!");
            }
        }

        public ServiceResponse.GeneralResponses Delete(string Id)
        {
            try
            {
                var data = context.Fees.FirstOrDefault(o => o.Id == new Guid(Id));
                if (data != null)
                {
                    context.Fees.Remove(data);
                    context.SaveChanges();

                    return new GeneralResponses(true, "Data Deleted!");
                }
                else
                {
                    return new GeneralResponses(true, "Data Already Deleted!");
                }

            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new GeneralResponses(false, "Error Occured!");
            }
        }

        public ServiceResponse.ResponsesWithData GetAll(string OrgId)
        {
            try
            {
                var data = context.Fees.Where(f => f.OrganizationId == new Guid(OrgId)).ToList();

                return new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data Retrieved!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", "Error Occured!");
            }
        }

        public ServiceResponse.ResponsesWithData Get(string Id)
        {
            try
            {
                var data = context.Fees.FirstOrDefault(h => h.Id == new Guid(Id));

                return new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data Retrieved!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", "Error Occured!");
            }
        }

        public ServiceResponse.ResponsesWithData GetStudents(string Id)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse.ResponsesWithData GetTeachers(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
