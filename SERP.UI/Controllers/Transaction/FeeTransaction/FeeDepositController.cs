using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.MasterViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Helper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transaction.FeeTransaction
{
    public class FeeDepositController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        private readonly IGenericRepository<BatchMaster, int> _IBatchMaster;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMaster;
        private readonly IGenericRepository<ReligionMaster, int> _IReligionMaster;
        private readonly IGenericRepository<CasteMaster, int> _ICategoryMaster;
        private readonly IGenericRepository<AcademicMaster, int> _IAcademicRepo;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<FeeDiscountModel, int> _IFeeDiscountRepo;
        private readonly IFeeDetailRepo _feeDetailRepo;
        private readonly IGenericRepository<FeeDeposit, int> _feeDepositRepo;
        private readonly IGenericRepository<StudentFeeDepositParticular, int> _feeDepositParticularRepo;

        public FeeDepositController(IGenericRepository<StudentMaster, int> studentRepo,
           IGenericRepository<BatchMaster, int> batchRepo,
           IGenericRepository<CourseMaster, int> courseRepo,
           IGenericRepository<ReligionMaster, int> religionRepo,
           IGenericRepository<CasteMaster, int> categoryRepo,
           IGenericRepository<AcademicMaster, int> academicRepo,
            IGenericRepository<StudentPromote, int> studentPromote,
            IGenericRepository<FeeDiscountModel, int> feeDiscountRepo,
            IFeeDetailRepo feeDetailRepo,
            IGenericRepository<FeeDeposit, int> feeDepositRepo,
            IGenericRepository<StudentFeeDepositParticular, int> feeParticularRepo)
        {
            _IStudentMaster = studentRepo;
            _IBatchMaster = batchRepo;
            _ICourseMaster = courseRepo;
            _IReligionMaster = religionRepo;
            _ICategoryMaster = categoryRepo;
            _IAcademicRepo = academicRepo;
            _IStudentPromote = studentPromote;
            _IFeeDiscountRepo = feeDiscountRepo;
            _feeDetailRepo = feeDetailRepo;
            _feeDepositRepo = feeDepositRepo;
            _feeDepositParticularRepo = feeParticularRepo;
        }
        public async Task<IActionResult> Index()
        {
           
            ViewBag.CourseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/FeeTransaction/_FeeDepositStudentDetail.cshtml");
        }

        public async Task<IActionResult> FeeDeposit(int studentId)
        {
            var result = await _feeDetailRepo.GetFeeDetailRepo(studentId);

            HttpContext.Session.SetInt32("StudentId", studentId);

            result.ForEach(x =>
            {
                switch (x.FeePaymentType)
                {
                    case "OT":
                        x.FeePaymentType = "One Time";
                        x.YearlyAmount = x.Amount;
                        break;
                    case "YL":
                        x.FeePaymentType = "Yearly";
                        x.YearlyAmount = x.Amount;
                        break;
                    case "HY":
                        x.FeePaymentType = "Half Yearly";
                        x.YearlyAmount = x.Amount * 2;
                        break;
                    case "QY":
                        x.FeePaymentType = "Quaterly";
                        x.YearlyAmount = x.Amount * 3;
                        break;
                    case "MT":
                        x.FeePaymentType = "Monthly";
                        x.YearlyAmount = x.Amount * 12;
                        break;
                }
                if (x.DependentOnParticular == 1)
                {
                    if (x.PertDiscountType.ToLower().Trim() == "per")
                        x.NetAmount = x.Amount - ((x.Amount * x.PertDiscountValue) / 100);
                    else
                        x.NetAmount = x.Amount - x.PertDiscountValue;
                }
                else
                {
                    x.NetAmount = x.Amount;
                    //if (x.DiscountType.ToLower().Trim() == "per")
                    //    x.NetAmount = x.Amount - ((x.Amount * x.DiscountValue) / 100);
                    //else
                    //    x.NetAmount = x.Amount - x.DiscountValue;
                }
            });

            var studentFeeDposited = (await _feeDepositRepo.GetList(x => x.StudentId == studentId && x.IsActive == 1 && x.IsDeleted == 0)).OrderByDescending(x=>x.Id).FirstOrDefault();
            if(studentFeeDposited!=null)
            {
                var feeDepositPartialurs = await _feeDepositParticularRepo.GetList(x => x.StudentFeeDepositId == studentFeeDposited.StudentId && x.IsDeleted == 0 && x.IsActive == 1);

                ViewBag.PreviousDueAmount = studentFeeDposited?.DueAmount;

                result.ForEach(x =>
                {
                    feeDepositPartialurs.ToList().ForEach(item =>
                    {
                        if (x.CategoryId == item.ParticularId && item.PaymentFor == null)
                        {
                            x.IsRemove = true;
                        }
                    });
                });
            }
            return PartialView("~/Views/FeeTransaction/_FeeDeposit.cshtml", result.Where(x=>x.IsRemove==false).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> FeeSubmit()
        {

            List<StudentFeeDepositParticular> fdParticulars = new List<StudentFeeDepositParticular>();
            var maxDepositId = await InsertFeeDeposit();

            var categories = Request.Form["CategoryId"];

            for (int i = 0; i < categories.Count(); i++)
            {
                StudentFeeDepositParticular model = new StudentFeeDepositParticular();
                model.StudentFeeDepositId = maxDepositId;
                model.ParticularId = Convert.ToInt32(categories[i]);
                model.PaymentFor = Request.Form[categories[i].ToString()];
                fdParticulars.Add(model);
            }

            var result = await _feeDepositParticularRepo.Add(fdParticulars.ToArray());

            return Json(ResponseData.Instance.GenericResponse(result));
        }

        private async Task<int> InsertFeeDeposit()
        {
            FeeDeposit model = new FeeDeposit();

            model.StudentId = Convert.ToInt32(HttpContext.Session.GetInt32("StudentId"));
            model.DiscountAmount = Convert.ToDecimal(Request.Form["extraDiscount"].ToString().EmptyToDefault<decimal>());
            model.FineAmount = Convert.ToDecimal(Request.Form["fineAmount"].ToString().EmptyToDefault<decimal>());
            model.ReasonFine = Request.Form["Reason"];
            model.AmountPaid = Convert.ToDecimal(Request.Form["txtAmountPaid"].ToString().EmptyToDefault<decimal>());
            model.DueAmount = Convert.ToDecimal(Request.Form["txtDueAmount"].ToString().EmptyToDefault<decimal>());
            model.DateOfDeposit = DateTime.Now.Date;
            model.PayableAmount = model.DueAmount + model.AmountPaid;

            var paymentResult = await _feeDepositRepo.CreateEntity(model);
            await _feeDepositRepo.CreateNewContext();
            var maxDepositId = (await _feeDepositRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0)).Max(x => x.Id);
            return maxDepositId;
        }

        public async Task<IActionResult> GetPaymentHistory(int studentId)
        {
            var result = await _feeDetailRepo.GetPaymentHistory(studentId);
            return PartialView("~/Views/FeeTransaction/_PaymentHistoryPartial.cshtml", result);
        }

        public async Task<IActionResult> FeeReciept()
        {
            return await Task.Run(() =>PartialView("~/Views/FeeTransaction/_FeeRecieptPartial.cshtml"));
        }
        public async Task<List<StudentPartialInfoViewModel>> GetStudentVm()
        {
            var studentPromotes = await _IStudentPromote.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            await _IStudentPromote.CreateNewContext();
            var studentList = await _IStudentMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            await _IStudentMaster.CreateNewContext();
            var courseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            await _IStudentMaster.CreateNewContext();
            var sectionList = await _IBatchMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            await _IStudentMaster.CreateNewContext();
            var religionList = await _IReligionMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            await _IStudentMaster.CreateNewContext();
            var categoryList = await _IFeeDiscountRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);


            var studentViewModel = (from sp in studentPromotes
                                    from sl in studentList
                                    where (sp.StudentId == sl.Id
                                     && sp.CourseId == sl.CourseId
                                     && sp.BatchId == sl.BatchId)
                                    join Cl in courseList
                                    on sp.CourseId equals Cl.Id
                                    join bl in sectionList
                                    on sp.BatchId equals bl.Id
                                    join rl in religionList
                                    on sl.Religion equals rl.Id
                                    join ct in categoryList
                                    on sl.FeeCategoryId equals ct.Id

                                    select new StudentPartialInfoViewModel
                                    {
                                        Id = sl.Id,
                                        RollCode = sl.RollCode,
                                        Registration = sl.RegistrationNumber,
                                        CourseName = Cl.Name,
                                        BatchName = bl.BatchName,
                                        StudentName = sl.Name,
                                        DateOfBirth = sl.DateOfBirth,
                                        JoiningDate = sl.JoiningDate,
                                        Category = ct.Name,
                                        FatherEmail = sl.FatherEmail,
                                        StudentEmail = sl.StudentEmail,
                                        FatherPhone = sl.FatherPhone,
                                        StudentPhoto = sl.StudentPhoto,
                                        StudentPhone = sl.StudentPhone,
                                        MotherPhone = sl.MotherPhone,
                                        MotherName = sl.MotherName,
                                        P_Address = sl.P_Address,
                                        C_Address = sl.C_Address,
                                        Religion = rl.Name,
                                        ParentPhoto = sl.ParentsPhoto
                                    }).ToList();
            return studentViewModel;
        }
    }
}