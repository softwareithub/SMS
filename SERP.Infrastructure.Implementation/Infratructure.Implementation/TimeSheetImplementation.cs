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
                        periodModel.SubjectId = modelData.SubjecId;
                        periodModel.FromTime = modelData.FromTime;
                        periodModel.ToTime = modelData.ToTime;
                        periodModel.Period = modelData.PeriodName;

                        periodVms.Add(periodModel);
                    }
                    timeTableModel.PeriodModels = periodVms;
                    timeSheetModels.Add(timeTableModel);
                }

                modelVm.TimeTableModels = timeSheetModels;
            }
            return modelVm;
        }

        public async Task<TimeSheetVm> GetTimeSheetDetailsByCourseIdBatchId(int courseId, int batchId)
        {
            TimeSheetVm modelTimeSheet = new TimeSheetVm();
            List<TimeTableVm> modelTimeTables = new List<TimeTableVm>();
           
            List<CompleteTimeSheetVm> completeTimeSheetVms = new List<CompleteTimeSheetVm>();

           var connection = baseContext.Database.GetDbConnection();
            SqlParameter[] objectParams = { };

            var result = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetTimeSheet, System.Data.CommandType.Text, objectParams);

            while (result.Read())
            {
                CompleteTimeSheetVm model = new CompleteTimeSheetVm();
                model.TimeTableDay= result.DefaultIfNull<string>("TimeTableDay");
                model.MasterId = result.DefaultIfNull<int>("MasterId");
                model.AttendenceId = result.DefaultIfNull<int>("AttendenceId");
                model.CourseName = result.DefaultIfNull<string>("CourseName");
                model.BatchName = result.DefaultIfNull<string>("BatchName");
                model.EmployeeName = result.DefaultIfNull<string>("EmployeeName");
                model.Photo = result.DefaultIfNull<string>("Photo");
                model.Phone = result.DefaultIfNull<string>("Phone");
                model.PeriodName = result.DefaultIfNull<string>("PeriodName");
                model.ToTime = result.DefaultIfNull<TimeSpan>("ToTime");
                model.FromTime = result.DefaultIfNull<TimeSpan>("FromTime");
                model.SubjectName = result.DefaultIfNull<string>("SubjectName");
                model.TimeTableDay = result.DefaultIfNull<string>("TimeTableDay");
                model.TeacherAttendence = result.DefaultIfNull<string>("TeacherAttendence");
                completeTimeSheetVms.Add(model);
            }

            foreach(var data in completeTimeSheetVms.GroupBy(x=>x.TimeTableDay))
            {
                List<PeriodVm> periodVms = new List<PeriodVm>();

                TimeTableVm timeTableVm = new TimeTableVm();
                timeTableVm.DayName = data.Key;

                foreach(var item in data)
                {
                    PeriodVm periodVm = new PeriodVm();
                    periodVm.Period = item.PeriodName;
                    periodVm.BatchName = item.BatchName;
                    periodVm.CourseName = item.CourseName;
                    periodVm.FromTime = item.FromTime;
                    periodVm.ToTime = item.ToTime;
                    periodVm.EmployeeImage = item.Photo;
                    periodVm.Phone = item.Phone;
                    periodVm.SubjectName = item.SubjectName;
                    periodVm.TeacherAttendence = item.TeacherAttendence;
                    periodVm.EmployeeName = item.EmployeeName;

                    periodVms.Add(periodVm);
                }
                timeTableVm.PeriodModels = periodVms;
                modelTimeTables.Add(timeTableVm);
            }
            modelTimeSheet.TimeTableModels = modelTimeTables;
            return modelTimeSheet;
        }
    }
}
