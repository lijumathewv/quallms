using Microsoft.AspNetCore.Identity;
using QualLMS.API.Data;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.API.Repositories
{
    public class AttendanceRepository (UserManager<User> userManager, DataContext dataContext) : IAttendance
    {
        public ResponsesWithData CheckIn(AttendanceData attendance)
        {
            ResponsesWithData? responses = null;
            try
            {
                DateOnly currentDate = DateOnly.Parse(attendance.CurrentDate.ToShortDateString());
                TimeOnly checkin = TimeOnly.Parse(Convert.ToDateTime(attendance.CheckIn).ToShortTimeString());

                var calender = (from c in dataContext.Calendar
                                join sc in dataContext.StudentCourse on c.CourseId equals sc.CourseId
                                where sc.StudentId == attendance.AppId
                                && c.Date == currentDate
                                && c.StartTime < checkin && c.EndTime > checkin
                                select c).ToList();

                if (calender.Count == 0)
                {
                    throw new Exception("Error Occured! No Course found at this time!");
                }
                else
                {
                    var data = dataContext.Attendance.FirstOrDefault(o => o.AttendanceDate == currentDate && o.UserId == attendance.AppId);

                    if (data == null)
                    {
                        dataContext.Attendance.Add(new Attendance
                        {
                            Id = Guid.NewGuid(),
                            UserId = attendance.AppId,
                            AttendanceDate = currentDate,
                            CheckIn = attendance.CheckIn
                        });
                    }
                    else
                    {
                        data.CheckIn = attendance.CheckIn;
                        data.CheckOut = null;
                    }
                    dataContext.SaveChanges();

                    responses = new ResponsesWithData(true, null!, "Check-In Success!");
                }

            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, null!, ex.Message);
            }

            return responses;
        }

        public ResponsesWithData CheckOut(AttendanceData attendance)
        {
            ResponsesWithData? responses = null;
            try
            {
                DateOnly currentDate = new DateOnly(attendance.CurrentDate.Year, attendance.CurrentDate.Month, attendance.CurrentDate.Day);

                var data = dataContext.Attendance.FirstOrDefault(o => o.AttendanceDate == currentDate && o.UserId == attendance.AppId);

                if (data == null)
                {
                    throw new Exception("No Check-In found!");
                }
                else
                {
                    if (data.CheckIn == null)
                    {
                        throw new Exception("No Check-In found!");
                    }
                    else
                    {
                        data.CheckOut = attendance.CheckOut;
                    }
                }
                dataContext.SaveChanges();

                responses = new ResponsesWithData(true, null!, "Check-Out Success!");
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, null!, ex.Message);
            }

            return responses;
        }

        public ResponsesWithData GetMyAttendance(Guid Id)
        {
            ResponsesWithData responses;
            List<Attendance> data = new List<Attendance>();
            try
            {
                data = dataContext.Attendance.Where(o => o.UserId == Id.ToString()).OrderBy(o => o.AttendanceDate).ToList();

                if (data == null)
                {
                    throw new Exception("No data found!");
                }
                else
                {
                    responses = new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data found!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, JsonSerializer.Serialize(data), ex.Message);
            }

            return responses;
        }

        public ResponsesWithData GetOrganizationAttendance(Guid OrgId)
        {
            ResponsesWithData responses;
            List<AttendanceData> data = new List<AttendanceData>();
            try
            {
                data = (from usr in userManager.Users.Where(o => o.OrganizationId == OrgId)
                           join att in dataContext.Attendance
                           on usr.Id equals att.UserId
                           select new AttendanceData
                           {
                               Id = att.Id.ToString(),
                               AppId = att.UserId,
                               CurrentDate = DateTime.Parse(att.AttendanceDate.ToString()!),
                               CheckIn = att.CheckIn,
                               CheckOut = att.CheckOut,
                               FullName = usr.FullName!
                           }).ToList();

                //data = dataContext.Attendance.Where(o => Ids.Contains(o.Id.ToString())).OrderBy(o => o.AttendanceDate).ToList();

                if (data == null)
                {
                    throw new Exception("No data found!");
                }
                else
                {
                    responses = new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data found!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, JsonSerializer.Serialize(data), ex.Message);
            }

            return responses;
        }

        public ResponsesWithData GetAttendanceForToday(Guid Id)
        {
            ResponsesWithData responses;
            Attendance data = new Attendance();
            try
            {
                data = dataContext.Attendance.FirstOrDefault(o => o.UserId == Id.ToString() && DateOnly.FromDateTime(DateTime.Today) == o.AttendanceDate);

                if (data == null)
                {
                    throw new Exception("No data found!");
                }
                else
                {
                    responses = new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data found!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, JsonSerializer.Serialize(data)!, ex.Message);
            }

            return responses;
        }

        public ResponsesWithData GetMyAttendanceDateFilter(Guid Id, DateOnly StartDate, DateOnly EndDate)
        {
            ResponsesWithData responses;
            List<Attendance> data = new List<Attendance>();
            try
            {
                data = dataContext.Attendance.Where(o => o.UserId == Id.ToString() && o.AttendanceDate >= StartDate && o.AttendanceDate <= EndDate).ToList();

                if (data == null)
                {
                    throw new Exception("No data found!");
                }
                else
                {
                    responses = new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data found!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, JsonSerializer.Serialize(data), ex.Message);
            }

            return responses;
        }
    }
}
