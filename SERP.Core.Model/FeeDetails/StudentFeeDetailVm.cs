using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.FeeDetails
{
    public class StudentFeeDetailVm
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RollCode { get; set; }
        public string FatherName { get; set; }
        public string FatherEmail { get; set; }
        public string StudentPhone { get; set; }
        public string AcademicName { get; set; }
        public string CategoryName { get; set; }
        public int DependentOnParticular { get; set; }
        public string PertDiscountType { get; set; }
        public decimal PertDiscountValue { get; set; }
        public string MotherName { get; set; }
        public string StudentImage { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountValue { get; set; }
        public string DiscountType { get; set; }
        public string FeePaymentType { get; set; }
        public decimal NetAmount { get; set; }
        public bool IsRemove { get; set; } = false;
        public decimal YearlyAmount { get; set; }
    }

}
