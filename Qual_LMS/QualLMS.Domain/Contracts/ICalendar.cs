using QualLMS.Domain.APIModels;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.Domain.Contracts
{
    public interface ICalendar
    {
        ResponsesWithData GetAll(string OrgId);
        ResponsesWithData Get(string Id);
        ResponsesWithData GetTeacherCalendar(string Id);
        GeneralResponses AddOrUpdate(CalendarData model);
        GeneralResponses Delete(string Id);

    }
}
