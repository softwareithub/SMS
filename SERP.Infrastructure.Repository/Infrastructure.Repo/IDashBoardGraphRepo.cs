using SERP.Core.Model.DashBoardModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IDashBoardGraphRepo
    {
        Task<List<StudentCourseBatchCountModel>> BatchCourseCount();

        Task<List<RootObjectModel>> GetRootList<T>(T entity);

        Task<List<StudentAttendenceReport>> GetStudentAttendenceReport(int year, int month, int day);
        Task<List<TeacherAbsentModel>> GetAbsentTeacher(DateTime attendenceDate);
        Task<DashBoardDetailVm> GetDashBoardDetails();
        Task<List<FeeDetailVm>> GetFeeDetails();
        Task<List<StudentOnlineTestResultVm>> GetStudentOnlineTestResult(int userId);

        Task<List<YearMonthWiseFeeDetail>> YearWiseFeeDetails();
        Task<List<DefaulterListModel>> DefaulterList(string monthName, int year);
    }
}
