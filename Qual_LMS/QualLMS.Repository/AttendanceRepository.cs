﻿using Microsoft.EntityFrameworkCore;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using System.Data;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Repository
{
    public class AttendanceRepository (DataContext dataContext) : IAttendance
    {
        public ResponsesWithData CheckIn(AttendanceData attendance)
        {
            ResponsesWithData? responses = null;
            try
            {
                DateOnly? currentDate = attendance.CurrentDate;
                TimeOnly checkin = TimeOnly.Parse(Convert.ToDateTime(attendance.CheckIn).ToShortTimeString());

                var calender = (from c in dataContext.Calendar
                                join sc in dataContext.StudentCourse on c.CourseId equals sc.CourseId
                                where sc.StudentId == attendance.AppId
                                && c.Date == currentDate
                                && checkin > c.StartTime && checkin < c.EndTime
                                select c).ToList();

                if (calender.Count == 0)
                {
                    throw new Exception("Error Occured! No Course found at this time!");
                }
                else
                {
                    var data = dataContext.Attendance.FirstOrDefault(o => o.AttendanceDate == currentDate && o.ApplicationUserId == attendance.AppId);

                    if (data == null)
                    {
                        dataContext.Attendance.Add(new Attendance
                        {
                            Id = Guid.NewGuid(),
                            ApplicationUserId = attendance.AppId,
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
                DateOnly? currentDate = attendance.CurrentDate;

                var data = dataContext.Attendance.FirstOrDefault(a => a.ApplicationUserId == attendance.AppId && a.AttendanceDate == currentDate);

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
            List<AttendanceData> data = new List<AttendanceData>();
            try
            {
                data = dataContext.Attendance
                    .Include(a => a.ApplicationUser)
                    .Where(o => o.ApplicationUserId == Id).OrderByDescending(o => o.AttendanceDate)
                    .Select(a => new AttendanceData
                    {
                        Id = a.Id.ToString(),
                        AppId = a.ApplicationUserId,
                        CurrentDate = a.AttendanceDate,
                        CheckIn = a.CheckIn,
                        CheckOut = a.CheckOut,
                        Role = a.ApplicationUser.Role.ToString(),
                        FullName = a.ApplicationUser.FullName
                    })
                    .ToList();

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
                data = (from usr in dataContext.ApplicationUser.Where(o => o.OrganizationId == OrgId)
                           join att in dataContext.Attendance
                           on usr.Id equals att.ApplicationUserId
                        select new AttendanceData
                           {
                               Id = att.Id.ToString(),
                               AppId = att.ApplicationUserId,
                               CurrentDate = att.AttendanceDate,
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
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime istNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, istTimeZone);
                DateOnly ist = new DateOnly(istNow.Year, istNow.Month, istNow.Day);

                data = dataContext.Attendance.FirstOrDefault(o => o.ApplicationUserId == Id && o.AttendanceDate == ist);

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
                responses = new ResponsesWithData(false, null!, ex.Message);
            }

            return responses;
        }

        public ResponsesWithData GetMyAttendanceDateFilter(Guid Id, DateOnly StartDate, DateOnly EndDate)
        {
            ResponsesWithData responses;
            List<Attendance> data = new List<Attendance>();
            try
            {
                data = dataContext.Attendance.Where(o => o.ApplicationUserId == Id && o.AttendanceDate >= StartDate && o.AttendanceDate <= EndDate).ToList();

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
