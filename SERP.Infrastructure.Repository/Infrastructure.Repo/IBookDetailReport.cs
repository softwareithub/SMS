using SERP.Core.Entities.LibraryReport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IBookDetailReport
    {
        public Task<List<BookDetailReport>> GetBookDetailReport();
    }
}
