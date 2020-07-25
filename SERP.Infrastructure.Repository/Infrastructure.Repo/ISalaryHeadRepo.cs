using SERP.Core.Entities.Entity.Core.HRModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface ISalaryHeadRepo
    {
        public Task<List<EmployeeSalarySlip>> GetEmployeeSalarySlip(int monthId, int employeeId, DateTime SalaryDate);
        public Task<List<EmployeeSalarySlip>> GetSalaryStatement(int monthId, int totalDays);
    }
}
