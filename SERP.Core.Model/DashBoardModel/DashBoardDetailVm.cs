using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class DashBoardDetailVm
    {
        public int StudentCount { get; set; }
        public int BoysCount { get; set; }
        public int GirlsCount { get; set; }
        public int CourseCount { get; set; }
        public int BatchCount { get; set; }
        public int EmployeeCount { get; set; }
        public int AbsentStudentCount { get; set; }
        public int AbsentEmployeeCount { get; set; }
        public int EmployeeWorkAniversaryCount { get; set; }
        public int StudentBirthDayCount { get; set; }
        public int VisitorCount { get; set; }
        public decimal PayableAmountTillDate { get; set; }
        public decimal DiscountAmountTillDate { get; set; }
        public decimal FineAmountTillDate { get; set; }
        public decimal AmountPaidTillDate { get; set; }
        public decimal AmountDueTillDate { get; set; }
    }
}
