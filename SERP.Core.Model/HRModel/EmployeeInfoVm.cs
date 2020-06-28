using SERP.Core.Entities.Entity.Core.HRModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.HRModel
{
    public class EmployeeInfoVm
    {
        public EmployeeBasicInfoModel EmployeeBasicInfoModel { get; set; }
        public List<EmployeeSalaryDetailModel> EmployeeSalaryModels { get; set; }
    }
}
