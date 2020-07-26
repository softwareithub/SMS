using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transaction.StudentTransaction
{
    public class StudentDocumentController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IGenericRepository<StudentDocumentUpload, int> _documentUploadRepo;
        private readonly IHostingEnvironment _hostingEnviroment;

        public StudentDocumentController(IGenericRepository<StudentMaster, int> studentRepo,
            IGenericRepository<StudentDocumentUpload, int> studentDocumentRepo, IHostingEnvironment hostingEnviroment)
        {
            _studentRepo = studentRepo;
            _documentUploadRepo = studentDocumentRepo;
            _hostingEnviroment = hostingEnviroment;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.StudentList = await _studentRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var educationDetail = await _documentUploadRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/StudentMaster/UploadDocumentPartial.cshtml", educationDetail);
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentDocumentUpload model, IFormFile DocumentPath)
        {
            model.DocumentPath =await  UploadDocument(DocumentPath);
            if (model.Id == 0)
            {
                var response = await _documentUploadRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var response = await _documentUploadRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        private async Task<string> UploadDocument(IFormFile file)
        {
            if (file != null)
            {
                var upload = Path.Combine(_hostingEnviroment.WebRootPath, "Document//");
                using (FileStream fs = new FileStream(Path.Combine(upload, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                return await Task.Run(() => "/Document/" + file.FileName);
            }
            return await Task.Run(() => string.Empty);

        }

        public async Task<IActionResult> GetDocumentDetails()
        {
            var models = (from SM in await _studentRepo.GetList(x => x.IsActive == 1)
                          join DD in await _documentUploadRepo.GetList(x => x.IsActive == 1)
                          on SM.Id equals DD.StudentId
                          select new StudentDocumentUpload
                          {
                              Id = DD.Id,
                              StudentName = SM.Name + " (" + SM.RegistrationNumber + ") ",
                              DocumentDescription= DD.DocumentDescription,
                              DocumentPath= DD.DocumentPath,
                              DocumentName=DD.DocumentName
                          }).ToList();

            return PartialView("~/Views/StudentMaster/_StudentUploadListPartial.cshtml", models);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _documentUploadRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<StudentDocumentUpload>(model,1);
            await _documentUploadRepo.CreateNewContext();
            var response = await _documentUploadRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}