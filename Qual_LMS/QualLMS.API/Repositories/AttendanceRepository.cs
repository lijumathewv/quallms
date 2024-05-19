using QualLMS.API.Data;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using System.Collections.ObjectModel;
using static QualLMS.Domain.Models.ServiceResponses;

namespace QualLMS.API.Repositories
{
    public class AttendanceRepository (DataContext dataContext) : IAttendance
    {
        public ResponsesWithData CheckIn(AttendanceData attendance)
        {
            ResponsesWithData? responses = null;
            try
            {
                DateOnly currentDate = new DateOnly(attendance.CurrentDate.Year, attendance.CurrentDate.Month, attendance.CurrentDate.Day);
                var data = dataContext.Attendance.FirstOrDefault(o => o.AttendanceDate == currentDate && o.AppUserId == attendance.AppId);

                if (data == null)
                {
                    dataContext.Attendance.Add(new Attendance
                    {
                        Id = Guid.NewGuid(),
                        AppUserId = attendance.AppId,
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

                var data = dataContext.Attendance.FirstOrDefault(o => o.AttendanceDate == currentDate && o.AppUserId == attendance.AppId);

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
            List<Attendance> attendances = new List<Attendance>();
            try
            {
                attendances = dataContext.Attendance.Where(o => o.AppUserId == Id.ToString()).OrderBy(o => o.AttendanceDate).ToList();

                if (attendances == null)
                {
                    throw new Exception("No data found!");
                }
                else
                {
                    responses = new ResponsesWithData(true, attendances, "Data found!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, attendances, ex.Message);
            }

            return responses;
        }

        public ResponsesWithData GetAttendanceForToday(Guid Id)
        {
            ResponsesWithData responses;
            Attendance attendance = new Attendance();
            try
            {
                attendance = dataContext.Attendance.FirstOrDefault(o => o.AppUserId == Id.ToString() && DateOnly.FromDateTime(DateTime.Today) == o.AttendanceDate);

                if (attendance == null)
                {
                    throw new Exception("No data found!");
                }
                else
                {
                    responses = new ResponsesWithData(true, attendance, "Data found!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, attendance!, ex.Message);
            }

            return responses;
        }

        public ResponsesWithData GetMyAttendanceDateFilter(Guid Id, DateOnly StartDate, DateOnly EndDate)
        {
            ResponsesWithData responses;
            List<Attendance> attendances = new List<Attendance>();
            try
            {
                attendances = dataContext.Attendance.Where(o => o.AppUserId == Id.ToString() && o.AttendanceDate >= StartDate && o.AttendanceDate <= EndDate).ToList();

                if (attendances == null)
                {
                    throw new Exception("No data found!");
                }
                else
                {
                    responses = new ResponsesWithData(true, attendances, "Data found!");
                }
            }
            catch (Exception ex)
            {
                responses = new ResponsesWithData(false, attendances, ex.Message);
            }

            return responses;
        }
    }
}
