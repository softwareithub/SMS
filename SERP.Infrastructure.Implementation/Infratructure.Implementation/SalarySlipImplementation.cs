using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class SalarySlipImplementation : ISalaryHeadRepo
    {
        private SERPContext baseContext = null;
        private readonly string _connectionString;

        public SalarySlipImplementation()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }
        public async Task<List<EmployeeSalarySlip>> GetEmployeeSalarySlip(int monthId, int employeeId, DateTime SalaryDate)
        {
            List<EmployeeSalarySlip> models = new List<EmployeeSalarySlip>();
            var commandText = "Proc_GetEmployeeSalaryInfo";

            SqlParameter[] @sqlParams = new SqlParameter[] {
                new SqlParameter("@empId",employeeId),
                new SqlParameter("@date",SalaryDate),
                new SqlParameter("@month",monthId),
            };

            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, @sqlParams);
            while (result.Read())
            {
                EmployeeSalarySlip model = new EmployeeSalarySlip();
                model.EmployeeName = result.DefaultIfNull<string>("Name");
                model.EmpCode = result.DefaultIfNull<string>("EmpCode");
                model.FatherName = result.DefaultIfNull<string>("FatherName");
                model.Phone = result.DefaultIfNull<string>("Phone");
                model.Email = result.DefaultIfNull<string>("Email");
                model.Address = result.DefaultIfNull<string>("C_Address");
                model.Department = result.DefaultIfNull<string>("DepartmentName");

                model.Designation = result.DefaultIfNull<string>("DesignationName");
                model.HeadName = result.DefaultIfNull<string>("HeadName");
                model.IsDependentPerDay = result.DefaultIfNull<int>("IsDependentPerDay");
                model.AdditionDeduction = result.DefaultIfNull<string>("Addition_Deduction");
                model.Amount = result.DefaultIfNull<decimal>("Amount");
                model.PerDayAmount = result.DefaultIfNull<decimal>("PerDayAmount");
                model.AbsentDays = result.DefaultIfNull<int>("AbsentDays");
                model.PresentDays = result.DefaultIfNull<int>("PresentDays");
                model.PayAmount = result.DefaultIfNull<decimal>("PayAmount");


                models.Add(model);

            }

            return models;
        }

        public async Task<List<EmployeeSalarySlip>> GetSalaryStatement(int monthId, int totalDays)
        {
            List<EmployeeSalarySlip> models = new List<EmployeeSalarySlip>();
            var commandText = "Proc_EmployeeSalaryStatement";

            SqlParameter[] @sqlParams = new SqlParameter[] {
                new SqlParameter("@monthId",monthId),
                new SqlParameter("@totalDays",totalDays),
            };

            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, @sqlParams);
            
            while (result.Read())
            {
                List<Tuple<string, decimal>> paySalary = new List<Tuple<string, decimal>>();
                EmployeeSalarySlip model = new EmployeeSalarySlip();
                model.EmployeeName = result.DefaultIfNull<string>("Name");
                model.EmpCode = result.DefaultIfNull<string>("EmpCode");
                model.Department = result.DefaultIfNull<string>("DepartmentName");

                model.Designation = result.DefaultIfNull<string>("DesignationName");
                model.IsDependentPerDay = result.DefaultIfNull<int>("IsDependentPerDay");
                model.AdditionDeduction = result.DefaultIfNull<string>("Addition_Deduction");
                model.Amount = result.DefaultIfNull<decimal>("Amount");
                model.PerDayAmount = result.DefaultIfNull<decimal>("PerDayAmount");
                model.AbsentDays = result.DefaultIfNull<int>("AbsentDays");
                model.PresentDays = result.DefaultIfNull<int>("PresentDays");
                for (int i = 10; i < result.FieldCount; i++)
                {
                    paySalary.Add(new Tuple<string, decimal>(result.GetName(i), result.DefaultIfNull<decimal>(result.GetName(i))));
                }
                model.PaySalary = paySalary;
                models.Add(model);
            }

            return models;
        }
    }
}
