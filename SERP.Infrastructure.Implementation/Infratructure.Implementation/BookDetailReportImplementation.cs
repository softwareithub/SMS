using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using SERP.Core.Entities.Context;
using SERP.Core.Entities.LibraryReport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class BookDetailReportImplementation : IBookDetailReport
    {
        private SERPContext baseContext = null;
        private readonly string _connectionString;

        public BookDetailReportImplementation()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }
        public async Task<List<BookDetailReport>> GetBookDetailReport()
        {
            List<BookDetailReport> models = new List<BookDetailReport>();
            var commandText = "usp_ProcGetBookDetailReport";
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);
            while (result.Read())
            {
                BookDetailReport model = new BookDetailReport();
                model.BookCount = result.DefaultIfNull<int>("BookCount");
                model.TitleName = result.DefaultIfNull<string>("TitleName");
                model.AuthorName = result.GetValueOrDefault<string>("AuthorName");
                model.PublisherName = result.GetValueOrDefault<string>("PublisherName");
                model.StatusName = result.GetValueOrDefault<string>("StatusName");
                model.ImagePath = result.GetValueOrDefault<string>("ImagePath");
                model.UnitPrice = result.GetValueOrDefault<decimal>("CostPrice");
                model.CourseName = result.GetValueOrDefault<string>("Course");
                model.SubjectName = result.GetValueOrDefault<string>("SubjectName");
                models.Add(model);

            }

            return models;
        }
    }
}
