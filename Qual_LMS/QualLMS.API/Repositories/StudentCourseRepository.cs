using Microsoft.AspNetCore.Identity;
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
    public class StudentCourseRepository(DataContext context, CustomLogger logger) : IStudentCourse
    {
        public ServiceResponse.GeneralResponses AddOrUpdate(StudentCourseData model)
        {
            try
            {
                var data = context.StudentCourse.FirstOrDefault(o => o.Id == model.Id);
                if (data == null)
                {
                    var Model = new StudentCourse()
                    {
                        StudentId = model.StudentId,
                        CourseId = model.CourseId,
                        RecentEducation = model.RecentEducation,
                        AdmissionNumber = model.AdmissionNumber,
                        CourseFees = model.CourseFees,
                        OrganizationId = model.OrganizationId
                    };
                    context.StudentCourse.Add(Model);
                }
                else
                {
                    data.CourseId = model.CourseId;
                    data.StudentId = model.StudentId;
                    data.RecentEducation = model.RecentEducation;
                    data.AdmissionNumber = model.AdmissionNumber;
                    data.CourseFees = model.CourseFees;
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
                var data = context.StudentCourse.FirstOrDefault(o => o.Id == new Guid(Id));
                if (data != null)
                {
                    context.StudentCourse.Remove(data);
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
                var studs = context.StudentCourse.Where(s => s.OrganizationId == new Guid(OrgId)).Include(i => i.Course).ToList();

                var data = (from s in studs join sc in context.ApplicationUser
                           on s.StudentId equals sc.Id
                            select new StudentCourseData
                            {
                                Id = s.Id,
                                StudentId = s.StudentId,
                                StudentName = sc.FullName,
                                CourseId = s.CourseId,
                                CourseName = s.Course.CourseName,
                                RecentEducation = s.RecentEducation,
                                AdmissionNumber = s.AdmissionNumber,
                                CourseFees = s.CourseFees,
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

        public ServiceResponse.ResponsesWithData Get(string Id)
        {
            try
            {
                var s = context.StudentCourse.Include(i => i.Course).FirstOrDefault(h => h.Id == new Guid(Id));

                var user = context.ApplicationUser.FirstOrDefault(u => u.Id == s.StudentId);

                var data = new StudentCourseData
                {
                    Id = s.Id,
                    StudentId = s.StudentId,
                    StudentName = user.FullName,
                    CourseId = s.CourseId,
                    CourseName = s.Course.CourseName,
                    RecentEducation = s.RecentEducation,
                    AdmissionNumber = s.AdmissionNumber,
                    CourseFees = s.CourseFees,
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

        public ResponsesWithData GetBalanceAmount(string StudentId, string CourseId)
        {
            try
            {
                var data = context.StudentCourse.FirstOrDefault(h => h.StudentId == new Guid(StudentId) && h.CourseId == new Guid(CourseId) && !h.Completed);

                int BalanceAmount = 0;
                int CourseFees = 0;
                if (data != null)
                {
                    CourseFees = data.CourseFees;
                    int ReceiptFees = context.FeesReceived.Where(f => f.StudentId ==  new Guid(StudentId) && f.CourseId == new Guid(CourseId)).Sum(s => s.ReceiptFees);

                    BalanceAmount = CourseFees - ReceiptFees;
                }

                return new ResponsesWithData(true, JsonSerializer.Serialize(BalanceAmount), "Data Retrieved!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", "Error Occured!");
            }
        }

        public ServiceResponse.ResponsesWithData GetStudentCourse(string StudentId)
        {
            try
            {
                var studs = context.StudentCourse.Include(i => i.Course).Where(s => s.StudentId == new Guid(StudentId) && !s.Completed).ToList();

                var data = (from s in studs
                            join sc in context.ApplicationUser
                           on s.StudentId equals sc.Id
                            select new StudentCourseData
                            {
                                Id = s.Id,
                                StudentId = s.StudentId,
                                StudentName = sc.FullName,
                                CourseId = s.CourseId,
                                CourseName = s.Course.CourseName,
                                RecentEducation = s.RecentEducation,
                                AdmissionNumber = s.AdmissionNumber,
                                CourseFees = s.CourseFees,
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
