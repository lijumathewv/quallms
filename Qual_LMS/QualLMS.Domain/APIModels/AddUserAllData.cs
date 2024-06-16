using Microsoft.AspNetCore.Mvc.Rendering;
using QualLMS.Domain.Models;
using QualvationLibrary;

namespace QualLMS.Domain.APIModels
{
    public class AddUserAllData : UserAllData
    {
        public Roles LoginRole { get; set; }
        public Roles role { get; set; }

        public SelectList orgSelect { get; set; }
    }
}
