using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using SERP.Core.Entities.Context;
using SERP.Core.Model.FeeDetails;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class FeeDepositRepo : IFeeDepositRecieptRepo
    {
        private SERPContext baseContext = null;

        public FeeDepositRepo()
        {
            this.baseContext = new SERPContext();
        }
        public async Task<List<FeeRecieptModel>> GetStudentFeeReciept(int feeDepositId)
        {
            List<FeeRecieptModel> models = new List<FeeRecieptModel>();
            var connection = baseContext.Database.GetDbConnection();
            SqlParameter[] sqlParams = {
            new SqlParameter("@in_id",feeDepositId){SqlDbType= System.Data.SqlDbType.Int, Direction= System.Data.ParameterDirection.Input },
            };
            var reader = await SqlHelperExtension.ExecuteReader(connection.ConnectionString, SqlConstant.GetFeeDepositReciept, System.Data.CommandType.StoredProcedure, sqlParams);
            while(reader.Read())
            {
                FeeRecieptModel model = new FeeRecieptModel();
                model.FeeDepositId = reader.GetValueOrDefault<int>("Id");
                model.RollCode = reader.GetValueOrDefault<string>("RollCode");
                model.Registration = reader.GetValueOrDefault<string>("RegistrationNumber");
                model.Name = reader.GetValueOrDefault<string>("Name");
                model.FatherName = reader.GetValueOrDefault<string>("FatherName");
                model.FatherPhone = reader.GetValueOrDefault<string>("FatherPhone");
                model.Address = reader.GetValueOrDefault<string>("C_Address");
                model.ClassName = reader.GetValueOrDefault<string>("ClassName");
                model.SectionName = reader.GetValueOrDefault<string>("BatchName");
                model.PayableAmount = reader.GetValueOrDefault<decimal>("PayableAmount");
                model.DiscountAmount = reader.GetValueOrDefault<decimal>("DiscountAmount");
                model.FineAmount = reader.GetValueOrDefault<decimal>("FineAmount");
                model.ReasonForFine = reader.GetValueOrDefault<string>("ReasonFine");
                model.AmountPaid = reader.GetValueOrDefault<decimal>("AmountPaid");
                model.DueAmount = reader.GetValueOrDefault<decimal>("DueAmount");
                model.DateOfDeposit = reader.GetValueOrDefault<DateTime>("DateOfDeposit");
                model.CategoryName = reader.GetValueOrDefault<string>("CategoryName");
                model.PaymentFor = reader.GetValueOrDefault<string>("PaymentFor");
                models.Add(model);
            }

            return models;
        }
    }
}
