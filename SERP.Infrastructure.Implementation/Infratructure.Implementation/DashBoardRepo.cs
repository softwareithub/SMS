using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Model.DashBoardModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class DashBoardRepo : IDashBoardGraphRepo
    {
        private SERPContext baseContext = null;
        private readonly string _connectionString;

        public DashBoardRepo()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }
        public async Task<List<StudentCourseBatchCountModel>> BatchCourseCount()
        {
            List<StudentCourseBatchCountModel> models = new List<StudentCourseBatchCountModel>();
            var commandText = "Master.Proc_GetStudentBatchCourseGraph";
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);
            while (result.Read())
            {
                StudentCourseBatchCountModel model = new StudentCourseBatchCountModel();
                model.CoureBatchName = result.DefaultIfNull<string>("CourseBatchName");
                model.StudentCount = result.DefaultIfNull<int>("StudentCount");
                models.Add(model);

            }
            return models;
        }

        public async Task<List<RootObjectModel>> GetRootList<T>(T entity)
        {

            List<RootObjectModel> models = new List<RootObjectModel>();
            var commandText = "Proc_GetAttendenceByDate";
            SqlParameter[] sqlParams = {
                new SqlParameter("@in_date",entity){SqlDbType= System.Data.SqlDbType.DateTime, Direction= System.Data.ParameterDirection.Input }
            };
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);
            while (result.Read())
            {
                RootObjectModel model = new RootObjectModel();
                model.name = result.DefaultIfNull<string>("AttendenceType");
                model.y = (double)result.DefaultIfNull<int>("StudentCount");
                models.Add(model);
            }

            return models;
        }

        public async Task<List<StudentAttendenceReport>> GetStudentAttendenceReport(int year, int month, int day)
        {
            List<StudentAttendenceReport> models = new List<StudentAttendenceReport>();
            var commandText = string.Empty;
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            if (day == 0)
            {
                commandText = "Proc_GetStudentAttendenceByMonth"; ;
                sqlParams.Add(new SqlParameter("@in_year", year) { SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Input });
                sqlParams.Add(new SqlParameter("@in_month", month) { SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Input });
            }
            else
            {
                commandText = "Proc_GetStudentAttendenceByMonthDate";
                sqlParams.Add(new SqlParameter("@in_year", year) { SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Input });
                sqlParams.Add(new SqlParameter("@in_month", month) { SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Input });
                sqlParams.Add(new SqlParameter("@date", day) { SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Input });
            }

            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams.ToArray());
            while (result.Read())
            {
                StudentAttendenceReport model = new StudentAttendenceReport();
                model.CourseBatchName = result.DefaultIfNull<string>("Name") + result.DefaultIfNull<string>("BatchName");
                if (result.DefaultIfNull<string>("AttendenceType") == "P")
                {
                    model.PresentCount = result.DefaultIfNull<int>("StudentCount");
                }
                else
                {
                    model.AbsentCount = result.DefaultIfNull<int>("StudentCount");
                }
                models.Add(model);
            }

            return models;
        }

        public async Task<List<TeacherAbsentModel>> GetAbsentTeacher(DateTime attendenceDate)
        {
            List<TeacherAbsentModel> models = new List<TeacherAbsentModel>();
            var commandText = "Proc_GetAbsentTeachers";
            SqlParameter[] sqlParams = {
                new SqlParameter("@date",attendenceDate){SqlDbType= System.Data.SqlDbType.DateTime, Direction= System.Data.ParameterDirection.Input }
            };
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);
            while (result.Read())
            {
                TeacherAbsentModel model = new TeacherAbsentModel();
                model.EmployeeName = result.DefaultIfNull<string>("Name");
                model.EmployeeCode = result.DefaultIfNull<string>("EmpCode");
                model.Photo = result.DefaultIfNull<string>("Photo");
                model.PeriodName = result.DefaultIfNull<string>("PeriodName");
                model.FromTime = result.DefaultIfNull<TimeSpan>("FromTime");
                model.ToTime = result.DefaultIfNull<TimeSpan>("ToTime");
                model.CourseId = result.DefaultIfNull<int>("CourseId");
                model.BatchId = result.DefaultIfNull<int>("BatchId");
                model.SubjectId = result.DefaultIfNull<int>("SubjecId");
                model.CourseName = result.DefaultIfNull<string>("CourseName");
                model.BatchName = result.DefaultIfNull<string>("BatchName");
                model.SubjectName = result.DefaultIfNull<string>("SubjectName");
                models.Add(model);
            }

            return models;
            
        }

        public async Task<DashBoardDetailVm> GetDashBoardDetails()
        {
            DashBoardDetailVm model = new DashBoardDetailVm();
            Dictionary<string, int> genderWiseCount = new Dictionary<string, int>();
            

            var commandText = "usp_DashBoardDetails";
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);
            while (result.Read())
            {
                model.StudentCount = result.DefaultIfNull<int>("StudentCount");
            }
            if (result.NextResult())
            {
                while (result.Read())
                {
                    genderWiseCount.Add(result.DefaultIfNull<string>("Gender"), result.DefaultIfNull<int>("StudentCount"));
                }
            }
            if (result.NextResult())
            {
                while(result.Read())
                {
                    model.CourseCount = result.DefaultIfNull<int>("CourseCount");
                }
               
            }
            if (result.NextResult())
            {
                while (result.Read())
                {
                    model.BatchCount = result.DefaultIfNull<int>("BatchCount");
                }
                
            }
            if (result.NextResult())
            {
                while (result.Read())
                {
                    model.EmployeeCount = result.DefaultIfNull<int>("EmployeeCount");
                }
                
            }
            if (result.NextResult())
            {
                while (result.Read())
                {
                    model.AbsentEmployeeCount = result.DefaultIfNull<int>("AbsentEmployeeCount");
                }
                
            }
            if (result.NextResult())
            {
                while (result.Read())
                {
                    model.VisitorCount = result.DefaultIfNull<int>("VisitorCount");
                }
               
            }
            if (result.NextResult())
            {
                while (result.Read())
                {
                    model.EmployeeWorkAniversaryCount = result.DefaultIfNull<int>("EmployeeWorkAniversaryCount");
                }
                
            }
            if (result.NextResult())
            {
                while (result.Read())
                {
                    model.StudentBirthDayCount = result.DefaultIfNull<int>("StudentBirthDayCount");
                }
                
            }
            if(result.NextResult())
            {
                while(result.Read())
                {
                    model.PayableAmountTillDate = result.DefaultIfNull<decimal>("PayableAmountTillDate");
                    model.DiscountAmountTillDate = result.DefaultIfNull<decimal>("DiscountAmountTillDate");
                    model.FineAmountTillDate = result.DefaultIfNull<decimal>("FineAmountTillDate");
                    model.AmountPaidTillDate = result.DefaultIfNull<decimal>("AmountPaidTillDate");
                    model.AmountDueTillDate = result.DefaultIfNull<decimal>("AmountDueTillDate");
                }
            }
            genderWiseCount.ToList().ForEach(item =>
            {
                if (item.Key == "Male")
                {
                    model.BoysCount = item.Value;
                }
                if(item.Key=="Female")
                {
                    model.GirlsCount = item.Value;
                }
            });
            return model;
        }

        public async Task<List<FeeDetailVm>> GetFeeDetails()
        {
            List<FeeDetailVm> models = new List<FeeDetailVm>();
            var commandText = "usp_GetFeeDetailInformation";
           
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);
            while (result.Read())
            {
                FeeDetailVm model = new FeeDetailVm();
                model.PayableAmountTillDate = result.DefaultIfNull<decimal>("PayableAmountTillDate");
                model.DiscountAmountTillDate = result.DefaultIfNull<decimal>("DiscountAmountTillDate");
                model.FineAmountTillDate = result.DefaultIfNull<decimal>("FineAmountTillDate");
                model.AmountPaidTillDate = result.DefaultIfNull<decimal>("AmountPaidTillDate");
                model.AmountDueTillDate = result.DefaultIfNull<decimal>("AmountDueTillDate");
                model.MonthYearAcademic = result.DefaultIfNull<string>("MonthName") ;
                models.Add(model);
            }

            return models;
        }
    }
}
