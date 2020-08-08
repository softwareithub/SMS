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

        public async Task<List<BookDetailModel>> GetBookDetails()
        {
            List<BookDetailModel> models = new List<BookDetailModel>();
            var commandText = "usp_ProcGetBookDetailReport";
            var reader = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);
            while (reader.Read())
            {
                BookDetailModel model = new BookDetailModel();
                model.BookCount = reader.DefaultIfNull<int>("BookCount");
                model.TitleName = reader.DefaultIfNull<string>("TitleName");
                model.AuthorName = reader.DefaultIfNull<string>("AuthorName");
                model.Publisher = reader.DefaultIfNull<string>("PublisherName");
                model.StatusName = reader.DefaultIfNull<string>("StatusName");
                model.CostPrice = reader.DefaultIfNull<decimal>("CostPrice");
                model.CourseName = reader.DefaultIfNull<string>("Course");
                model.SubjectName = reader.DefaultIfNull<string>("SubjectName");
                model.ColorCode = reader.DefaultIfNull<string>("colorCode");

                models.Add(model);
            }

            return models;
        }

        public async Task<EmployeeInformationModel> GetEmployeeInformationModel(int employeeId)
        {
            EmployeeInformationModel model = new EmployeeInformationModel();
            List<EmployeeSalaryHead> salaryHeads = new List<EmployeeSalaryHead>();
            List<EmployeeAttandence> employeeAttandences = new List<EmployeeAttandence>();

            SqlParameter[] sqlParams = {
                new SqlParameter("@employeeId",employeeId),
            };

            var commandText = "Proc_EmployeeDetailInfo";
            var reader = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);

            if(reader.Read())
            {
                model.EmployeeId = reader.DefaultIfNull<int>("Id");
                model.EmployeeName = reader.DefaultIfNull<string>("EmployeeName");
                model.EmpCode = reader.DefaultIfNull<string>("EmpCode");
                model.FatherName = reader.DefaultIfNull<string>("FatherName");
                model.MotherName = reader.DefaultIfNull<string>("MotherName");
                model.Phone = reader.DefaultIfNull<string>("Phone");
                model.Email = reader.DefaultIfNull<string>("Email");
                model.CAddress = reader.DefaultIfNull<string>("C_Address");
                model.P_Address = reader.DefaultIfNull<string>("p_Address");
                model.Photo = reader.DefaultIfNull<string>("Photo");
                model.EmergencyPhone = reader.DefaultIfNull<string>("EmergencyPhone");
                model.ConvictedToCrime = reader.DefaultIfNull<int>("ConvictedForCrime");
                model.Gender = reader.DefaultIfNull<string>("Gender");
                model.BloodGroup = reader.DefaultIfNull<string>("BloodGroup");
                model.DepartmentName = reader.DefaultIfNull<string>("DepartmentName");
                model.DesignationName = reader.DefaultIfNull<string>("DesignationName");
                model.JoiningDate = reader.DefaultIfNull<DateTime>("JoiningDate");
                model.AccountNumber = reader.DefaultIfNull<string>("AccountNumber");
            }

            if(reader.NextResult())
            {
                while(reader.Read())
                {
                    EmployeeSalaryHead salaryHead = new EmployeeSalaryHead();
                    salaryHead.HeadName = reader.DefaultIfNull<string>("Name");
                    salaryHead.AdditionDeduction = reader.DefaultIfNull<string>("Addition_Deduction");
                    salaryHead.Amount = reader.DefaultIfNull<decimal>("Amount");
                    salaryHead.IsDependentOnPerDay = reader.DefaultIfNull<int>("IsDependentPerDay");
                    salaryHeads.Add(salaryHead);
                }
            }

            if(reader.NextResult())
            {
                while(reader.Read())
                {
                    EmployeeAttandence attandence = new EmployeeAttandence();
                    attandence.AttendenceCount = reader.DefaultIfNull<int>("AttandenceCount");
                    attandence.AttendenceType = reader.DefaultIfNull<string>("AttendenceType");
                    attandence.DateMonthName = reader.DefaultIfNull<string>("DateMonthName");
                    attandence.DateYear = reader.DefaultIfNull<int>("DateYear");
                    employeeAttandences.Add(attandence);
                }
            }

            model.EmployeeAttandence = employeeAttandences;
            model.EmployeeSalaryHeads = salaryHeads;

            return model;
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
                model.FatherName = reader.DefaultIfNull<string>("FatherName");
                model.MotherName = reader.DefaultIfNull<string>("MotherName");
                model.CourseName = reader.DefaultIfNull<string>("CourseName");
                model.BatchName = reader.DefaultIfNull<string>("BatchName");
                model.RollCode = reader.DefaultIfNull<string>("RollCode");
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

        public async Task<StudentFeeDetailModel> StudentFeeDetailReport(int studentId)
        {
            StudentFeeDetailModel model = new StudentFeeDetailModel();
            List<StudentFeeDpositDetail> studentFeeDpositDetails = new List<StudentFeeDpositDetail>();
            StudentFeeDisocunt discountModel = new StudentFeeDisocunt();
            List<StudentPayment> PaymentModels = new List<StudentPayment>();

            SqlParameter[] sqlParams = {
                new SqlParameter("@studentId",studentId)
            };
            var commandText = "Proc_GetStudentFeeDetailReport";
            var reader = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);
            while (reader.Read())
            {
                StudentPayment payment = new StudentPayment();
                payment.FeeParticular = reader.DefaultIfNull<string>("CategoryName");
                payment.Amount = reader.DefaultIfNull<decimal>("Amount");
                payment.PaymentType = reader.DefaultIfNull<string>("PaymentType");
                payment.YearlyPayment = reader.DefaultIfNull<decimal>("YearlyPayment");
                PaymentModels.Add(payment);
            }
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    StudentFeeDpositDetail feeDepositModel = new StudentFeeDpositDetail();
                    feeDepositModel.PaymentAmount = reader.DefaultIfNull<decimal>("PayableAmount");
                    feeDepositModel.DiscountAmount = reader.DefaultIfNull<decimal>("DiscountAmount");
                    feeDepositModel.FineAmount = reader.DefaultIfNull<decimal>("FineAmount");
                    feeDepositModel.Reason = reader.DefaultIfNull<string>("ReasonFine");
                    feeDepositModel.AmountPaid = reader.DefaultIfNull<decimal>("AmountPaid");
                    feeDepositModel.DueAmount = reader.DefaultIfNull<decimal>("DueAmount");
                    feeDepositModel.DepositDate = reader.DefaultIfNull<DateTime>("DateOfDeposit");
                    feeDepositModel.DepositId = reader.DefaultIfNull<int>("Id");
                    studentFeeDpositDetails.Add(feeDepositModel);
                }
            }
            if (reader.NextResult())
            {
                if (reader.Read())
                {
                    discountModel.DiscountName = reader.DefaultIfNull<string>("DiscountName");
                    discountModel.DiscountCode = reader.DefaultIfNull<string>("Code");
                    discountModel.Description = reader.DefaultIfNull<string>("Description");
                    discountModel.DiscountType = reader.DefaultIfNull<string>("DiscountType");
                    discountModel.DisocuntValue = reader.DefaultIfNull<decimal>("DiscountValue");
                }
            }
            model.PaymentModels = PaymentModels;
            model.FeeDiscountValue = discountModel;
            model.StudentFeeDpositDetails = studentFeeDpositDetails;
            return model;
        }
    }
}
