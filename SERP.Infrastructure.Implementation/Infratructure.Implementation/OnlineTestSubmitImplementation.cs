using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Model.OnlineTest;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class OnlineTestSubmitImplementation : IOnlineTestSubmitRepository
    {
        private SERPContext baseContext = null;
        private readonly string _connectionString;

        public OnlineTestSubmitImplementation()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task<TestSubmissionModel> GetOnlineSubmitDetail(int userId, int testId)
        {
            TestSubmissionModel model = new TestSubmissionModel();
            string TestName = string.Empty;
            DateTime TestDateTime = default;
            int questionCount = default(int);
            int answereCount = default(int);
            try
            {
                var commandText = @"Proc_GetOnlineSubmit";

                var sqlParams = new SqlParameter[2];
                sqlParams[0] = new SqlParameter("@testId", testId);
                sqlParams[1] = new SqlParameter("@userId", userId);

                var reader = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, sqlParams);
                while (reader.Read())
                {
                    TestName = reader.DefaultIfNull<string>("TestName");
                    TestDateTime = reader.DefaultIfNull<DateTime>("TestDateTime");
                }
                reader.NextResult();
                while (reader.Read())
                {
                    questionCount = reader.DefaultIfNull<int>("QuestionCount");
                }
                reader.NextResult();
                while (reader.Read())
                {
                    answereCount = reader.DefaultIfNull<int>("AnwereCount");
                }
                model.TestName = TestName;
                model.AttemptedQuestion = answereCount;
                model.TotalQuestion = questionCount;
                model.UnAttemptedQuestion = (questionCount - answereCount);
            }
            finally
            {

            }
            return model;
        }
    }
}
