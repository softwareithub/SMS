using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class AcademicCalenderController : Controller
    {
        private readonly IGenericRepository<AcademicCalender, int> _IAcademicCalenderRepo;

        public AcademicCalenderController(IGenericRepository<AcademicCalender, int> academiCalenderRepo)
        {
            _IAcademicCalenderRepo = academiCalenderRepo;
        }
        public async Task<IActionResult> AcademicCalender(int id)
        {
            var responseData = await _IAcademicCalenderRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/AcademicCalender/_AcademicCalenderPartial.cshtml", responseData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAcademicCalender(AcademicCalender model)
        {
            if (model.Id > 0)
            {
                var response = await _IAcademicCalenderRepo.Update(CommanDeleteHelper.CommanUpdateCode<AcademicCalender>(model, 1));
                return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(response));
            }
            return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(await _IAcademicCalenderRepo.CreateEntity(model)));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<AcademicCalender>(await _IAcademicCalenderRepo.GetSingle(x => x.Id == id), 1);
            await _IAcademicCalenderRepo.CreateNewContext();
            return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(await _IAcademicCalenderRepo.Update(deleteModel)));
        }

        public async Task<IActionResult> GetAcademicCalender()
        {
            return PartialView("~/Views/AcademicCalender/_AcademicCalenderListPartial.cshtml", await _IAcademicCalenderRepo.GetList(x => x.IsActive == 1));
        }
             
    }
}