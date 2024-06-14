using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface IAttendance
    {
        ResponsesWithData CheckIn(AttendanceData attendance);
        ResponsesWithData CheckOut(AttendanceData attendance);

        ResponsesWithData GetMyAttendance(Guid Id);
        ResponsesWithData GetOrganizationAttendance(Guid OrgId);
        ResponsesWithData GetAttendanceForToday(Guid Id);
        ResponsesWithData GetMyAttendanceDateFilter(Guid Id, DateOnly StartDate, DateOnly EndDate);
    }
}
