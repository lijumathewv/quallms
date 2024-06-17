using Microsoft.EntityFrameworkCore;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Repository
{
    public class CalendarRepository(DataContext context, CustomLogger logger) : ICalendar
    {
        public ServiceResponse.GeneralResponses AddOrUpdate(CalendarData model)
        {
            try
            {
                var data = context.Calendar.FirstOrDefault(o => o.Id == model.Id);
                if (data == null)
                {
                    var Model = new Calendar()
                    {
                        TeacherId = model.TeacherId,
                        CourseId = model.CourseId,
                        Date = model.Date,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        OrganizationId = model.OrganizationId
                    };
                    context.Calendar.Add(Model);
                }
                else
                {
                    data.CourseId = model.CourseId;
                    data.TeacherId = model.TeacherId;
                    data.Date = model.Date;
                    data.StartTime = model.StartTime;
                    data.EndTime = model.EndTime;
                    data.OrganizationId = model.OrganizationId;
                }
                context.SaveChanges();

                return new GeneralResponses(true, "Data Updated!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                string Message = "Error Occured!" + ex.Message;
                if (ex.InnerException != null)
                {
                    Message += "<br/>" + ex.InnerException.Message;
                }
                return new GeneralResponses(false, Message);
            }
        }

        public ServiceResponse.GeneralResponses Delete(string Id)
        {
            try
            {
                var data = context.Calendar.FirstOrDefault(o => o.Id == new Guid(Id));
                if (data != null)
                {
                    context.Calendar.Remove(data);
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
                var data = context.Calendar.Where(c => c.OrganizationId == new Guid(OrgId))
                    .Include(i => i.Course)
                    .Include(i=>i.Teacher)
                    .Select(s => new CalendarData
                    {
                        Id = s.Id,
                        TeacherId = s.TeacherId,
                        TeacherName = s.Teacher.FullName,
                        CourseId = s.CourseId,
                        CourseName = s.Course.CourseName,
                        Date = s.Date,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        OrganizationId = s.OrganizationId
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
                var s = context.Calendar.Include(i => i.Course).FirstOrDefault(h => h.Id == new Guid(Id));

                var user = context.ApplicationUser.FirstOrDefault(u => u.Id == s.Id);

                var data = new CalendarData
                {
                    Id = s.Id,
                    TeacherId = s.TeacherId,
                    TeacherName = user.FullName,
                    CourseId = s.CourseId,
                    CourseName = s.Course.CourseName,
                    Date = s.Date,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    OrganizationId = s.OrganizationId
                };

                return new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data Retrieved!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", "Error Occured!");
            }
        }

        public ServiceResponse.ResponsesWithData GetTeacherCalendar(string TeacherId)
        {
            try
            {
                var studs = context.Calendar.Include(i => i.Course).Where(s => s.TeacherId == new Guid(TeacherId)).ToList();

                var data = (from s in studs
                            join sc in context.ApplicationUser
                           on s.TeacherId equals sc.Id
                            select new CalendarData
                            {
                                Id = s.Id,
                                TeacherId = s.TeacherId,
                                TeacherName = sc.FullName,
                                CourseId = s.CourseId,
                                CourseName = s.Course.CourseName,
                                Date = s.Date,
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                OrganizationId = s.OrganizationId
                            }).ToList();

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
