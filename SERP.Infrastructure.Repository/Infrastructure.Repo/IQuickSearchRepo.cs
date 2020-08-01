using SERP.Core.Model.QuickSearchModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IQuickSearchRepo
    {
        Task<StudentQuickModel> GetStudentBasicInfo(int studentId);
        Task<List<StudentAttendenceReport>> GetStudentAttandenceReport(int studentId, int monthId, int year);
    }
}
