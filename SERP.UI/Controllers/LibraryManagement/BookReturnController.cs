using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LibraryManagement;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class BookReturnController : Controller
    {
        private readonly IGenericRepository<BookTransaction, int> _bookTransactionRepo;
        private readonly IGenericRepository<BookItemModel, int> _bookItemRepo;
        private readonly IGenericRepository<BookMasterModel, int> _bookMasterRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IGenericRepository<BookStatusModel, int> _bookStatusRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public BookReturnController(IGenericRepository<BookTransaction, int> bookTransactionRepo,
                                    IGenericRepository<BookItemModel, int> bookItemRepo, 
                                    IGenericRepository<BookMasterModel, int> bookMasterRepo,
                                    IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
                                    IGenericRepository<StudentMaster, int> studentRepo,
                                    IGenericRepository<BookStatusModel, int> bookStatusRepo,
                                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _bookTransactionRepo = bookTransactionRepo;
            _bookItemRepo = bookItemRepo;
            _bookMasterRepo = bookMasterRepo;
            _employeeRepo = employeeRepo;
            _studentRepo = studentRepo;
            _bookStatusRepo = bookStatusRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.BookStatus = await _bookStatusRepo.GetList(x => x.IsActive == 1);
                var issueDetailModel = await GetBookItemDetail(id);
                return PartialView("~/Views/LibraryManagement/_BookReturnPartial.cshtml", issueDetailModel.FirstOrDefault());
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

        [HttpPost]
        public async Task<IActionResult> ReturnBook(BookIssueDetailModel model)
        {
            try
            {
                if (model.ActualReturnDate.Year == 1)
                {
                    return Json("Return Date is not valid.Please select valid Date");
                }
                else
                {
                    var bookTransactionModel = await _bookTransactionRepo.GetSingle(x => x.Id == model.Id);

                    bookTransactionModel.ActualReturnDate = model.ActualReturnDate;
                    bookTransactionModel.FineAmount = model.FineAmount;
                    bookTransactionModel.FineReason = model.FineReason;
                    bookTransactionModel.DiscountReason = model.DiscountReason;
                    bookTransactionModel.FineDiscountAmount = model.DiscountAmount;

                    await _bookTransactionRepo.CreateNewContext();
                    var response = await _bookTransactionRepo.Update(bookTransactionModel);

                    if (response == Utilities.ResponseUtilities.ResponseStatus.UpdatedSuccessFully)
                    {
                        var booItemModel = await _bookItemRepo.GetSingle(x => x.Id == model.BookItemId);
                        booItemModel.BookStatus = model.BookCondition == 4 ? 2 : model.BookCondition;

                        await _bookItemRepo.CreateNewContext();

                        var bookItemResponse = await _bookItemRepo.Update(booItemModel);
                        return Json(ResponseData.Instance.GenericResponse(bookItemResponse));
                    }

                    return Json(ResponseData.Instance.GenericResponse(response));
                }
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }

        }

        #region Private Method
        private async Task<List<BookIssueDetailModel>> GetBookItemDetail(int id)
        {
            var bookIsuseDetails = (from BT in await _bookTransactionRepo.GetList(x => x.IsActive == 1 && x.Id == id)
                                    join BI in await _bookItemRepo.GetList(x => x.IsActive == 1)
                                    on BT.BookItemId equals BI.Id
                                    join BM in await _bookMasterRepo.GetList(x => x.IsActive == 1)
                                    on BI.BookId equals BM.Id
                                    select new BookIssueDetailModel
                                    {
                                        Id = BT.Id,
                                        BookItemId = BT.BookItemId,
                                        IssueDate = BT.IssueDate,
                                        ExpectedReturnDate = BT.ExpectedReturnDate,
                                        BookItemName = BI.BookBarCode,
                                        BookName = BM.TitleName,
                                        UserType = BT.UserTypeId,
                                        UserId = BT.UserId
                                    }).ToList();
            var studentList = await _studentRepo.GetList(x => x.IsActive == 1);
            var employeeList = await _employeeRepo.GetList(x => x.IsActive == 1);

            bookIsuseDetails.ForEach(item =>
            {
                if (item.UserType == 0)
                {
                    item.IssueTo = studentList.ToList().Find(x => x.Id == item.UserId).Name + " (" + "Student" + ") ";
                }
                else
                {
                    item.IssueTo = employeeList.ToList().Find(x => x.Id == item.UserId).Name + " (" + "Employee" + ") ";
                }
            });

            return bookIsuseDetails;
        }

        #endregion

    }
}