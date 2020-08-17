using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;

namespace SERP.UI.Controllers.Transaction.FeeTransaction
{
    public class StudentFeePaymentController : Controller
    {
        private readonly IFeeDepositRecieptRepo _feeDepositRecieptRepo;
        private readonly IGenericRepository<CourseMaster, int> _courseRepo;
        private readonly IGenericRepository<BatchMaster, int> _batchRepo;
        private readonly IGenericRepository<StudentPromote, int> _studentPromoteRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        private readonly IGenericRepository<FeeDeposit, int> _feeDepositRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        
        public StudentFeePaymentController(IFeeDepositRecieptRepo feeDepositRecieptRepo,
                                           IGenericRepository<CourseMaster, int> courseRepo, 
                                           IGenericRepository<BatchMaster, int> batchRepo,
                                           IGenericRepository<StudentPromote, int> studentPromoteRepo,
                                           IGenericRepository<StudentMaster, int> studentRepo,
                                           IGenericRepository<FeeDeposit, int> feeDepositRepo,
                                           IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _feeDepositRecieptRepo = feeDepositRecieptRepo;
            _courseRepo = courseRepo;
            _batchRepo = batchRepo;
            _studentMasterRepo = studentRepo;
            _studentPromoteRepo = studentPromoteRepo;
            _feeDepositRepo = feeDepositRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.CourseList = await _courseRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/FeeTransaction/_FeeReiceptHeaderPartial.cshtml");
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> FeeRecipet(int studentId)
        {
            try
            {
                var studentFeeDeposit = (await _feeDepositRepo.GetList(x => x.StudentId == studentId)).Max(x => x.Id);

                var response = await _feeDepositRecieptRepo.GetStudentFeeReciept(studentFeeDeposit);
                return PartialView("~/Views/FeeTransaction/_FeeRecieptPartial.cshtml", response);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }
    }
}