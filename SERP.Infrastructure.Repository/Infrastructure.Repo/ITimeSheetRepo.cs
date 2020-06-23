using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.TransactionViewModel;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface ITimeSheetRepo : IGenericRepository<TimeTableMasterModel, int>
    {
        Task<TimeSheetVm> GetAllIncludeTimeSheetDetails(int courseId, int batchId);

        Task<TimeSheetVm> GetTimeSheetDetailsByCourseIdBatchId(int courseId, int batchId);

        Task<ResponseStatus> DeactivateTimeSheet(int courseId, int batchId);
        Task<List<PeriodVm>> GetSubjectTeacher(int teacherId);
        Task<List<FreeEmployeeModel>> AssignTeacherTemp(TimeSpan fromTime, TimeSpan ToTime);
    }
}
