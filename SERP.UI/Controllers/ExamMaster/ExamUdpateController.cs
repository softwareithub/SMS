using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.ExamMaster
{
    public class ExamUdpateController : Controller
    {
        private readonly IGenericRepository<ExamUpdate, int> _IExamUpdateRepo;

        public ExamUdpateController (IGenericRepository<ExamUpdate, int> examUpdateRepo)
        {
            _IExamUpdateRepo = examUpdateRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var response =await  _IExamUpdateRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/ExamMaster/_ExamUpdatePartial.cshtml",response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent(ExamUpdate model)
        {
            if (model.Id == 0)
            {
                var result = await _IExamUpdateRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                var result = await _IExamUpdateRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEventDetails()
        {
            var responseDetails = await _IExamUpdateRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/ExamMaster/_ExamEventDetails.cshtml", responseDetails);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var responseData = await _IExamUpdateRepo.GetSingle(x => x.Id == id);
            responseData.IsActive = 0;
            responseData.IsDeleted = 1;
            await _IExamUpdateRepo.CreateNewContext();
            var response = await _IExamUpdateRepo.Update(responseData);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}