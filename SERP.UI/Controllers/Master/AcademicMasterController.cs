using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using SERP.Core.Entities.Entity.Core.Master;

namespace SERP.UI.Controllers.Master
{
    public class AcademicMasterController : Controller
    {
        public IActionResult Index()
        {
            Thread.Sleep(5000);
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AcademicMaster model)
        {
            return await Task.Run(() => Json("Data Added"));
        }
    }
}