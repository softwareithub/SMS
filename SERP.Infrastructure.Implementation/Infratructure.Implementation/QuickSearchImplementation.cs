using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Model.QuickSearchModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class QuickSearchImplementation : IQuickSearchRepo
    {
        private readonly string _connectionString;
        private SERPContext baseContext = null;
        public QuickSearchImplementation()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<StudentAttendenceReport>> GetStudentAttandenceReport(int studentId, int monthId, int year)
        {
            List<StudentAttendenceReport> models = new List<StudentAttendenceReport>();

            SqlParameter[] sqlParams = {
                new SqlParameter("@studentId",studentId),
                new SqlParameter("@monthId",monthId),
                new SqlParameter("@yearId",year),
            };
            var commandText = "Proc_GetStudentAttandenceReport";
            var reader = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);
            while (reader.Read())
            {
                StudentAttendenceReport model = new StudentAttendenceReport();
                model.AttandenceCount = reader.DefaultIfNull<int>("AttendCount");
                model.AttandenceType = reader.DefaultIfNull<string>("AttendenceType");
                model.AttandenceMonth = reader.DefaultIfNull<string>("AttendMonthName");
                model.AttandenceYear = reader.DefaultIfNull<int>("AttendYear"); 
                model.AttandeceMonthId = reader.DefaultIfNull<int>("AttendMonthId");
                models.Add(model);
            }

            return models;
        }

        public async Task<StudentQuickModel> GetStudentBasicInfo(int studentId)
        {
            StudentQuickModel model = new StudentQuickModel();
            SqlParameter[] sqlParams = {
                new SqlParameter("@studentId",studentId)
            };
            var commandText = "Proc_StudentDetailInfo";
            var reader = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);
            while (reader.Read())
            {
                model.StudentName = reader.DefaultIfNull<string>("Name");
                model.FatherName  = reader.DefaultIfNull<string>("FatherName");
                model.MotherName  = reader.DefaultIfNull<string>("MotherName");
                model.CourseName  = reader.DefaultIfNull<string>("CourseName");
                model.BatchName  = reader.DefaultIfNull<string>("BatchName");
                model.RollCode  = reader.DefaultIfNull<string>("RollCode");
                model.DateOfBirth = reader.DefaultIfNull<DateTime>("DateOfBirth");
                model.Gender = reader.DefaultIfNull<string>("Gender");
                model.BloodGroup = reader.DefaultIfNull<string>("BloodGroup");
                model.ReligionName = reader.DefaultIfNull<string>("ReligionName");
                model.FeeCategory = reader.DefaultIfNull<string>("FeeCategory");
                model.FatherEmail = reader.DefaultIfNull<string>("FatherEmail");
                model.EmergencyPhone = reader.DefaultIfNull<string>("EmergencyPhone");
                model.StudentPhone = reader.DefaultIfNull<string>("StudentPhone");
                model.MotherPhone = reader.DefaultIfNull<string>("MotherPhone");
                model.PAddress = reader.DefaultIfNull<string>("P_Address");
                model.CAddress = reader.DefaultIfNull<string>("C_Address");
                model.StudentPhoto = reader.DefaultIfNull<string>("StudentPhoto");
                model.ParentPhoto = reader.DefaultIfNull<string>("ParentsPhoto");
            }

            return model;
        }
    }
}
