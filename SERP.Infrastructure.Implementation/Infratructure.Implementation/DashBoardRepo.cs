using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Model.DashBoardModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                commandText= "Proc_GetStudentAttendenceByMonth"; ;
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
                if(result.DefaultIfNull<string>("AttendenceType")=="P")
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
            throw new NotImplementedException();
        }
    }
}
