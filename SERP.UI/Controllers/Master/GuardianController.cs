using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Master
{
    public class GuardianController : Controller
    {
        private readonly IGenericRepository<GuardianMaster, int> _IGuardianRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentRepo;
        private readonly IHostingEnvironment _hostingEnviroment;


        public GuardianController(IGenericRepository<GuardianMaster, int> guardianRepo, 
            IGenericRepository<StudentMaster, int> studentRepo, IHostingEnvironment hostingEnviroment)
        {
            _IGuardianRepo = guardianRepo;
            _IStudentRepo = studentRepo;
            _hostingEnviroment = hostingEnviroment;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Students = await _IStudentRepo.GetList(x => x.IsActive == 1);
            var model = await _IGuardianRepo.GetSingle(z => z.Id == id);
            return PartialView("~/Views/GuardianMaster/_GuardianInfo.cshtml", model);
        }
        public async Task<IActionResult> GuradianList()
        {
            var models = (from GM in await _IGuardianRepo.GetList(x => x.IsActive == 1)
                          join SM in await _IStudentRepo.GetList(x => x.IsActive == 1)
                          on GM.StudentId equals SM.Id
                          select new GuardianMaster
                          {
                              StudentName = SM.Name,
                              StudentId = GM.StudentId,
                              GuradianName = GM.GuradianName,
                              GuardianImage = GM.GuardianImage,
                              Phone = GM.Phone,
                              Email = GM.Email,
                              IsVerified = GM.IsVerified
                          }).ToList();
            return PartialView("~/Views/GuardianMaster/_GuardianPartialList.cshtml",models);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GuardianMaster model, IFormFile GuardianImage)
        {
            List<IFormFile> formFiles = new List<IFormFile>
            {
                GuardianImage
            };

            var uploadImages = await UploadImage.UploadImageOnFolder(formFiles, _hostingEnviroment);
            model.GuardianImage = uploadImages.First();
                
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var response = await _IGuardianRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var response = await _IGuardianRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _IGuardianRepo.GetSingle(x => x.Id == id);
            var deleteModel=CommanDeleteHelper.CommanDeleteCode(model, 1);
            await _IGuardianRepo.CreateNewContext();
            var response = await _IGuardianRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}