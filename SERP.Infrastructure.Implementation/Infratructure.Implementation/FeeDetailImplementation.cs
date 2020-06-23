using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Model.FeeDetails;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SERPHelper;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class FeeDetailImplementation : IFeeDetailRepo
    {
        private SERPContext baseContext = null;

        private readonly string _connectionString;
        public FeeDetailImplementation()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }
        public async Task<List<StudentFeeDetailVm>> GetFeeDetailRepo(int studentId)
        {
            List<StudentFeeDetailVm> models = new List<StudentFeeDetailVm>();
            var commandText = "TransactionSch.usp_GetStudentFeeDetails";
            SqlParameter[] sqlParams = {
                new SqlParameter("@in_id",studentId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input }
            };
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);
            while (result.Read())
            {
                StudentFeeDetailVm model = new StudentFeeDetailVm();
                model.StudentName = result.DefaultIfNull<string>("Name");
                model.FatherName = result.DefaultIfNull<string>("FatherName");
                model.RollCode = result.DefaultIfNull<string>("RollCode");
                model.FatherEmail = result.DefaultIfNull<string>("FatherEmail");
                model.StudentPhone = result.DefaultIfNull<string>("StudentPhone");
                model.AcademicName = result.DefaultIfNull<string>("AcademicName");
                model.CategoryName = result.DefaultIfNull<string>("ParticularName");
                model.Amount = result.DefaultIfNull<decimal>("Amount");
                model.FeePaymentType = result.DefaultIfNull<string>("FeePaymentType");
                model.DependentOnParticular = result.DefaultIfNull<int>("DependentOnParticular");
                model.DiscountType = result.DefaultIfNull<string>("DiscountType");
                model.DiscountValue = result.DefaultIfNull<decimal>("DiscountValue");
                model.PertDiscountType = result.DefaultIfNull<string>("ParticularDiscountType");
                model.PertDiscountValue = result.DefaultIfNull<decimal>("ParticularDiscountValue");
                model.CategoryId = result.DefaultIfNull<int>("CategoryId");

                models.Add(model);

            }
            return models;

        }

        public async Task<List<FeeDepositVm>> GetPaymentHistory(int studentId)
        {
            List<FeeDepositVm> models = new List<FeeDepositVm>();

            var commandText = "TransactionSch.usp_StudentFeeDepositDetails";
            SqlParameter[] sqlParams = {
                new SqlParameter("@in_studentId",studentId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input }
            };
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);

            while(result.Read())
            {
                FeeDepositVm model = new FeeDepositVm();
                model.Id = result.DefaultIfNull<int>("Id");
                model.PayableAmount = result.DefaultIfNull<decimal>("payableAmount");
                model.DiscountAmount = result.DefaultIfNull<decimal>("DiscountAmount");
                model.FineAmount = result.DefaultIfNull<decimal>("FineAmount");
                model.FineReason = result.DefaultIfNull<string>("ReasonFine");
                model.AmountPaid = result.DefaultIfNull<decimal>("AmountPaid");
                model.DueAmount = result.DefaultIfNull<decimal>("DueAmount");
                model.DepositDate = result.DefaultIfNull<DateTime>("DateOfDeposit");
                model.CategoryName= result.DefaultIfNull<string>("Name");
                model.PaymentType = result.DefaultIfNull<string>("PaymentType");
                model.PaymentFor = PaymentFor.GetPaymentFor(result.DefaultIfNull<string>("PaymentFor"), model.PaymentType);

                models.Add(model);
            }

            return models;
        }
    }
}
