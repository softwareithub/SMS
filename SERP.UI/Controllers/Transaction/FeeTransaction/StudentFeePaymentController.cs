using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

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
        public StudentFeePaymentController(IFeeDepositRecieptRepo feeDepositRecieptRepo, IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<StudentPromote, int> studentPromoteRepo,
            IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<FeeDeposit, int> feeDepositRepo)
        {
            _feeDepositRecieptRepo = feeDepositRecieptRepo;
            _courseRepo = courseRepo;
            _batchRepo = batchRepo;
            _studentMasterRepo = studentRepo;
            _studentPromoteRepo = studentPromoteRepo;
            _feeDepositRepo = feeDepositRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CourseList = await _courseRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/FeeTransaction/_FeeReiceptHeaderPartial.cshtml");
        }

        public async Task<IActionResult> FeeRecipet(int studentId)
        {
            var studentFeeDeposit = (await _feeDepositRepo.GetList(x => x.StudentId == studentId)).Max(x=>x.Id);
            
            var response = await _feeDepositRecieptRepo.GetStudentFeeReciept(studentFeeDeposit);
            return PartialView("~/Views/FeeTransaction/_FeeRecieptPartial.cshtml",response);
        }
    }
}