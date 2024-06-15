using Microsoft.EntityFrameworkCore;
using QualLMS.API.Data;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.API.Repositories
{
    public class FeesReceivedRepository(DataContext context, CustomLogger logger) : IFeesReceived
    {
        public ServiceResponse.GeneralResponses AddOrUpdate(FeesReceivedData model)
        {
            try
            {
                var data = context.FeesReceived.FirstOrDefault(o => o.Id == model.Id);
                if (data == null)
                {
                    var Model = new FeesReceived
                    {
                        UserId = model.UserId,
                        CourseId = model.CourseId,
                        FeesId = model.FeesId,
                        ReceiptNumber = model.ReceiptNumber,
                        ReceiptDate = model.ReceiptDate,
                        ReceiptFees = model.ReceiptFees,
                        Mode = model.Mode,
                        ModeDetails = model.ModeDetails,
                        OrganizationId = model.OrganizationId
                    };

                    context.FeesReceived.Add(Model);
                }
                else
                {
                    data.CourseId = model.CourseId;
                    data.FeesId = model.FeesId;
                    data.ReceiptNumber = model.ReceiptNumber;
                    data.ReceiptDate = model.ReceiptDate;
                    data.ReceiptFees = model.ReceiptFees;
                    data.Mode = model.Mode;
                    data.ModeDetails = model.ModeDetails;
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
                var data = context.FeesReceived.FirstOrDefault(o => o.Id == new Guid(Id));
                if (data != null)
                {
                    context.FeesReceived.Remove(data);
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

        public ServiceResponse.ResponsesWithData Get()
        {
            try
            {
                var data = context.FeesReceived
                    .Include(i => i.Course)
                    .Include(i => i.Fees)
                    .Include(i => i.User)
                    .Select(s => new FeesReceivedData
                    {
                        Id = s.Id,
                        StudentName = s.User.FullName,
                        CourseId = s.CourseId,
                        CourseName = s.Course.CourseName,
                        FeesId = s.FeesId,
                        FeesName = s.Fees.FeesName,
                        ReceiptNumber = s.ReceiptNumber,
                        ReceiptDate = s.ReceiptDate,
                        ReceiptFees = s.ReceiptFees,
                        Mode = s.Mode,
                        ModeDetails = s.ModeDetails,
                    })
                    .ToList();

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
                var data = context.FeesReceived
                    .Include(i => i.Course)
                    .Include(i => i.Fees)
                    .Select(s => new FeesReceivedData
                    {
                        Id = s.Id,
                        CourseId = s.CourseId,
                        CourseName = s.Course.CourseName,
                        FeesId = s.FeesId,
                        FeesName = s.Fees.FeesName,
                        ReceiptNumber = s.ReceiptNumber,
                        ReceiptDate = s.ReceiptDate,
                        ReceiptFees = s.ReceiptFees,
                        Mode = s.Mode,
                        ModeDetails = s.ModeDetails,
                    }).FirstOrDefault(h => h.Id == new Guid(Id));

                return new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data Retrieved!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", "Error Occured!");
            }
        }
    }
}
