using SERP.Core.Entities.StudentTransaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IReportRepository
    {
        Task<IEnumerable<AttendanceModel>> GetAttendanceModels(int studentId, int yearId, int monthId);
    }
}
