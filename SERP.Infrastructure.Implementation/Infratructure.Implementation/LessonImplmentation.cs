using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Model.LessonMasterDetail;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class LessonImplmentation : ILessonRepository
    {
        private SERPContext baseContext = null;
        private readonly string _connectionString;

        public LessonImplmentation()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }
        public async  Task<List<LessonMasterVm>> GetLessonMasterAsync()
        {
            List<LessonMasterVm> models = new List<LessonMasterVm>();
            var commandText = "usp_GetLessonDetails";
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);

            while(await result.ReadAsync())
            {
                LessonMasterVm model = new LessonMasterVm();
                model.LessonId = result.DefaultIfNull<int>("Id");
                model.CourseName = result.DefaultIfNull<string>("Name");
                model.SubjectName = result.DefaultIfNull<string>("SubjectName");
                model.LessonName = result.DefaultIfNull<string>("LessonName");
                models.Add(model);
            }
            return models;
        }
    }
}
