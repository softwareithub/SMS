using SERP.Core.Model.LessonMasterDetail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface ILessonRepository
    {
        Task<List<LessonMasterVm>> GetLessonMasterAsync();
    }
}
