using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Entities.StudentTransaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class ReportImplementation : IReportRepository
    {
        private SERPContext baseContext = null;
        public ReportImplementation()
        {
            this.baseContext = new SERPContext();
        }
        public async Task<IEnumerable<AttendanceModel>> GetAttendanceModels(int studentId, int yearId, int monthId)
        {
            List<AttendanceModel> models = new List<AttendanceModel>();

            var connection = baseContext.Database.GetDbConnection();

            SqlParameter[] sqlParams = {
            new SqlParameter("@studentId",studentId){SqlDbType= System.Data.SqlDbType.Int,
                Direction= System.Data.ParameterDirection.Input },
             new SqlParameter("@Month",monthId){SqlDbType= System.Data.SqlDbType.Int,
                Direction= System.Data.ParameterDirection.Input },
              new SqlParameter("@Year",yearId){SqlDbType= System.Data.SqlDbType.Int,
                Direction= System.Data.ParameterDirection.Input },
            };

            var reader = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetStudentAttendance,
                System.Data.CommandType.StoredProcedure, sqlParams);

            while (reader.Read())
            {
                var model = new AttendanceModel();
                model.AttendanceDate = reader.DefaultIfNull<DateTime>("AttendenceDate").ToString();
                model.Type = reader.DefaultIfNull<string>("AttendenceType").ToString();
                model.DayName = reader.DefaultIfNull<string>("Day Name").ToString();
                model.Type = model.Type.ToUpper();
                models.Add(model);
            }
            if (models.Any()) {
                models.First().PresentCount = models.Where(x => x.Type == "P").Count();
                models.First().AbsentCount = models.Where(x => x.Type == "A").Count();
                models.First().OtherHolidayCount = models.Where(x => x.Type != "A" && x.Type != "P").Count();
                models.First().PresentPerc = (models.First().PresentCount / (models.First().PresentCount + models.First().AbsentCount
                    + models.First().OtherHolidayCount)).ToString();

                models.First().AbsentPerc = (models.First().AbsentCount / (models.First().PresentCount + models.First().AbsentCount
                  + models.First().OtherHolidayCount)).ToString();
            }

            return models;
        }
    }
}
