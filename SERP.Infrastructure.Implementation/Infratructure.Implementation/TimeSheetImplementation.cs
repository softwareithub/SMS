using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Helpers;
using SERP.Utilities;
using SERP.Utilities.SqlHelper;
using SERP.Core.Model.TimeTable;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SERP.Core.Entities.TimeTable;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class TimeSheetImplementation : GenericImplementation<TimeTableMasterModel, int>, ITimeSheetRepo
    {
        private SERPContext baseContext = null;
        public TimeSheetImplementation()
        {
            baseContext = new SERPContext();
        }

        public async Task<ResponseStatus> DeactivateTimeSheet(int courseId, int batchId)
        {
            var courseDataList = await baseContext.TimeTableMasterModels.Where(item => item.CourseId == courseId && item.BatchId == batchId)
                .Include(p => p.TimeTableAssignSubjTeacherModels).ToListAsync();
            //if (courseData.TimeTableAssignSubjTeacherModels.Any())
            //{
            //    baseContext.TimeTableAssignSubjTeacherModels.UpdateRange(baseContext.TimeTableAssignSubjTeacherModels);
            //    await baseContext.SaveChangesAsync();
            //}

            foreach (var courseData in courseDataList)
            {
                courseData.IsActive = 0;
                courseData.IsDeleted = 1;

                foreach (var data in courseData.TimeTableAssignSubjTeacherModels)
                {
                    data.IsActive = 0;
                    data.IsDeleted = 1;

                    foreach (var timeTable in data.TimeTableMasterModel.TimeTableAssignSubjTeacherModels)
                    {
                        timeTable.IsActive = 0;
                        timeTable.IsDeleted = 1;
                    }
                }
            }

            try
            {
                baseContext.UpdateRange(courseDataList);
                await baseContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return ResponseStatus.ServerError;
            }
            return ResponseStatus.UpdatedSuccessFully;
        }

        public async Task<TimeSheetVm> GetAllIncludeTimeSheetDetails(int courseId, int batchId)
        {
            TimeSheetVm modelVm = new TimeSheetVm();
            List<TimeTableVm> timeSheetModels = new List<TimeTableVm>();

            using (baseContext)
            {
                var result = await baseContext.TimeTableMasterModels.Where(x => x.IsActive == 1 && x.IsDeleted == 0 && x.CourseId == courseId && x.BatchId == batchId)
                                    .Include(x => x.TimeTableAssignSubjTeacherModels).Where(x => x.IsDeleted == 0 && x.IsActive == 1).ToListAsync();

                foreach (var data in result)
                {
                    TimeTableVm timeTableModel = new TimeTableVm();
                    timeTableModel.DayName = data.Name;
                    List<PeriodVm> periodVms = new List<PeriodVm>();

                    foreach (var modelData in data.TimeTableAssignSubjTeacherModels)
                    {
                        PeriodVm periodModel = new PeriodVm();
                        periodModel.EmployeeId = modelData.TeacherId;
                        //periodModel.SubjectId = modelData.SubjecId;
                        periodModel.FromTime = modelData.FromTime;
                        periodModel.ToTime = modelData.ToTime;
                        //periodModel.Period = modelData.PeriodName;

                        periodVms.Add(periodModel);
                    }
                    timeTableModel.PeriodModels = periodVms;
                    timeSheetModels.Add(timeTableModel);
                }

                modelVm.TimeTableModels = timeSheetModels;
            }
            return modelVm;
        }

        public async Task<List<TimeTableModel>> GetTimeSheetDetailsByCourseIdBatchId(int courseId, int batchId)
        {
            var models = new List<TimeTableModel>();
            var connection = baseContext.Database.GetDbConnection();

            SqlParameter[] objectParams = {
            new SqlParameter("@courseId",courseId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@batchId",batchId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            };

            var result = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetTimeTableDetail, System.Data.CommandType.StoredProcedure, objectParams);

            while (result.Read())
            {
                TimeTableModel model = new TimeTableModel();
                model.Id = result.DefaultIfNull<int>("Id");
                model.CourseName = result.DefaultIfNull<string>("CourseName");
                model.CourseId = result.DefaultIfNull<int>("courseId");
                model.BatchId = result.DefaultIfNull<int>("BatchId");

                model.BatchName = result.DefaultIfNull<string>("BatchName");
                model.SubjectId = result.DefaultIfNull<int>("Id");
                model.SubjectName = result.DefaultIfNull<string>("SubjectName");

                model.EmployeeDetail = result.DefaultIfNull<string>("Name") + result.DefaultIfNull<string>("EmpCode");
                model.FromTime = result.DefaultIfNull<TimeSpan>("FromTime");
                model.ToTime = result.DefaultIfNull<TimeSpan>("ToTime");
                model.DayName = result.DefaultIfNull<string>("TimeTableDay");
                models.Add(model);
            }
            return models;

        }

        public async Task<List<PeriodVm>> GetSubjectTeacher(int subjectId)
        {
            List<PeriodVm> periods = new List<PeriodVm>();
            var connection = baseContext.Database.GetDbConnection();
            SqlParameter[] objectParams = {
            new SqlParameter("@subjectId",subjectId){SqlDbType= System.Data.SqlDbType.Int, Direction=System.Data.ParameterDirection.Input }
            };

            var result = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetTeacherBySubjectId,
                System.Data.CommandType.StoredProcedure, objectParams);
            while (result.Read())
            {
                PeriodVm model = new PeriodVm();
                model.EmployeeName = result.DefaultIfNull<string>("Name");
                model.EmployeeId = result.DefaultIfNull<int>("Id");

                periods.Add(model);
            }
            return periods;
        }

        public async Task<List<FreeEmployeeModel>> AssignTeacherTemp(TimeSpan fromTime, TimeSpan ToTime)
        {
            List<FreeEmployeeModel> freeEmployeeList = new List<FreeEmployeeModel>();

            var connection = baseContext.Database.GetDbConnection();

            SqlParameter[] objectParams = {
            new SqlParameter("@fromTime",fromTime){SqlDbType= System.Data.SqlDbType.Time, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@toTIme",ToTime){SqlDbType= System.Data.SqlDbType.Time, Direction= System.Data.ParameterDirection.Input },
            };

            var result = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetFreeTeacher, System.Data.CommandType.Text, objectParams);

            while (result.Read())
            {
                FreeEmployeeModel model = new FreeEmployeeModel();
                model.EmployeeName = result.DefaultIfNull<string>("EmployeeName");
                model.Photo = result.DefaultIfNull<string>("Photo");
                model.EmployeeId = result.DefaultIfNull<int>("TeacherId");
                freeEmployeeList.Add(model);
            }

            return freeEmployeeList;
        }

        public async Task<List<TimeTableModel>> GetTimeTableModels(int courseId, int batchId)
        {
            var models = new List<TimeTableModel>();
            var connection = baseContext.Database.GetDbConnection();

            SqlParameter[] objectParams = {
            new SqlParameter("@courseId",courseId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@batchId",batchId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            };

            var result = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetTimeTableDetail, System.Data.CommandType.StoredProcedure, objectParams);

            while (result.Read())
            {
                TimeTableModel model = new TimeTableModel();
                model.Id = result.DefaultIfNull<int>("Id");
                model.CourseName = result.DefaultIfNull<string>("CourseName");
                model.CourseId = result.DefaultIfNull<int>("courseId");
                model.BatchId = result.DefaultIfNull<int>("BatchId");

                model.BatchName = result.DefaultIfNull<string>("BatchName");
                model.SubjectId = result.DefaultIfNull<int>("Id");
                model.SubjectName = result.DefaultIfNull<string>("SubjectName");

                model.EmployeeDetail = result.DefaultIfNull<string>("Name") + result.DefaultIfNull<string>("EmpCode");
                model.FromTime = result.DefaultIfNull<TimeSpan>("FromTime");
                model.ToTime = result.DefaultIfNull<TimeSpan>("ToTime");
                model.DayName = result.DefaultIfNull<string>("TimeTableDay");
                model.FromT = result.DefaultIfNull<TimeSpan>("FromTime").ToString();
                model.ToT = result.DefaultIfNull<TimeSpan>("ToTime").ToString();
                models.Add(model);
            }
            return models;
        }

        public async Task<ResponseStatus> DeleteTimeTable(int courseId, int batchId, int dayId, TimeSpan fromTime, TimeSpan toTime, int subjectId)
        {
            var connection = baseContext.Database.GetDbConnection();
            SqlParameter[] objectParams = {
            new SqlParameter("@courseId",courseId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@batchId",batchId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@subjectId",subjectId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@dayId",dayId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@fromTime",fromTime){SqlDbType= System.Data.SqlDbType.Time, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@toTime",toTime){SqlDbType= System.Data.SqlDbType.Time, Direction= System.Data.ParameterDirection.Input },
            };

            var result = await SqlHelperExtension.ExecuteNonQuery(connection.ConnectionString, SqlConstant.DeleteTimetable, System.Data.CommandType.StoredProcedure, objectParams);
            return ResponseStatus.UpdatedSuccessFully;
        }

        Task<TimeSheetVm> ITimeSheetRepo.GetTimeSheetDetailsByCourseIdBatchId(int courseId, int batchId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MappedTeacherModel>> GetMappedTecherModel(int courseId, int batchId)
        {
            var models= new List<MappedTeacherModel>();
            var connection = baseContext.Database.GetDbConnection();
            SqlParameter[] objectParams = {
            new SqlParameter("@courseId",courseId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            new SqlParameter("@batchId",batchId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            };

            var result = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetMappedTeacher, System.Data.CommandType.StoredProcedure, objectParams);

            while (result.Read())
            {
                MappedTeacherModel model = new MappedTeacherModel();
                model.Id = result.DefaultIfNull<int>("Id");
                model.EmployeeName = result.DefaultIfNull<string>("Name");
                model.Phone = result.DefaultIfNull<string>("Phone");
                model.Email = result.DefaultIfNull<string>("Email");
                model.Photo = result.DefaultIfNull<string>("Photo");
                model.SubjectName = result.DefaultIfNull<string>("SubjectName");
                models.Add(model);
            }
            return models;
        }
    }
}
