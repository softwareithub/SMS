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
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class BookIssueController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<BookMasterModel, int> _bookMasterRepo;
        private readonly IGenericRepository<BookItemModel, int> _bookItemRepo;
        private readonly IGenericRepository<BookTransaction, int> _bookTransactionRepo;
        private readonly IGenericRepository<LibrarySetting, int> _librarySettingRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;


        public BookIssueController(IGenericRepository<StudentMaster, int> studentRepo, 
                                   IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, 
                                   IGenericRepository<BookMasterModel, int> bookMasterRepo, 
                                   IGenericRepository<BookItemModel, int> bookItemRepo, 
                                   IGenericRepository<BookTransaction, int> bookTransactionRepo,
                                   IGenericRepository<LibrarySetting, int> librarySettingRepo,
                                   IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _studentMasterRepo = studentRepo;
            _employeeRepo = employeeRepo;
            _bookItemRepo = bookItemRepo;
            _bookMasterRepo = bookMasterRepo;
            _bookTransactionRepo = bookTransactionRepo;
            _librarySettingRepo = librarySettingRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateBookViewBag();
                await PopulateStudentViewBag();
                await PopulateEmployeeViewBag();

                return PartialView("~/Views/LibraryManagement/BookIssuePartial.cshtml");
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
        public async Task<IActionResult> IssueBook(BookTransaction model)
        {
            try
            {
                model.BookStatusId = 1;
                model.UserTypeId = Request.Form["IssueTo"].ToString() == "stu" ? 0 : 1;

                var bookitem = await _bookItemRepo.GetSingle(x => x.Id == model.BookItemId);
                bookitem.BookStatus = 1;
                await _bookItemRepo.CreateNewContext();
                var updateResponse = await _bookItemRepo.Update(bookitem);

                if (model.Id == 0)
                {
                    var response = await _bookTransactionRepo.CreateEntity(model);
                    return Json("Book successfully issue.");
                }
                else
                {
                    var updateModel = CommanDeleteHelper.CommanDeleteCode<BookTransaction>(model, 1);
                    var response = await _bookTransactionRepo.Update(updateModel);
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

        public async Task<IActionResult> GetBookItems(int bookId)
        {
            try
            {
                var bookItemList = await _bookItemRepo.GetList(x => x.IsActive == 1 && x.BookStatus == 2 && x.BookId == bookId);
                return Json(bookItemList);
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

        public async Task<IActionResult> BookIssueDetail()
        {
            try
            {
                var bookIsuseDetails = (from BT in await _bookTransactionRepo.GetList(x => x.IsActive == 1)
                                        join BI in await _bookItemRepo.GetList(x => x.IsActive == 1)
                                        on BT.BookItemId equals BI.Id
                                        join BM in await _bookMasterRepo.GetList(x => x.IsActive == 1)
                                        on BI.BookId equals BM.Id
                                        where BT.ActualReturnDate.Year == 1
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
                var studentList = await _studentMasterRepo.GetList(x => x.IsActive == 1);
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

                return PartialView("~/Views/LibraryManagement/_bookIssueDetailPartial.cshtml", bookIsuseDetails);
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

        public async Task<IActionResult> GetIssueInformation(string userType, string Date)
        {
            try
            {
                var UserType = userType == "stu" ? "student" : "staff";
                var responseModel = await _librarySettingRepo.GetSingle(x => x.UserType.Trim().ToLower() == UserType.Trim().ToLower());
                var newDate = System.Convert.ToDateTime(Date).AddDays(responseModel.IssueDays);
                return Json(newDate.ToShortDateString());
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

        #region PrivateMember
        private async Task PopulateBookViewBag()
        {
            var bookList = await _bookMasterRepo.GetList(x => x.IsActive == 1);

            List<BookModel> bookDetails = new List<BookModel>();
            bookList.ToList().ForEach(item =>
            {
                BookModel bookModel = new BookModel()
                {
                    Id = item.Id,
                    BookName = item.AuthorName + "(" + item.PublisherName + ")" + "(" + item.TitleName + ")"
                };
                bookDetails.Add(bookModel);
            });
            ViewBag.BookList = bookDetails;
        }

        private async Task PopulateStudentViewBag()
        {
            var studentList = await _studentMasterRepo.GetList(x => x.IsActive == 1);
            ViewBag.StudentList = studentList;
        }

        private async Task PopulateEmployeeViewBag()
        {
            var employeeList = await _employeeRepo.GetList(x => x.IsActive == 1);
            ViewBag.employeeList = employeeList;
        }

        public async Task<IActionResult> GetEmployeeDetails()
        {
            try
            {
                return Json(await _employeeRepo.GetList(x => x.IsActive == 1));
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

        public async Task<IActionResult> GetStudentDetails()
        {
            try
            {
                return Json(await _studentMasterRepo.GetList(x => x.IsActive == 1));
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

        #endregion
    }
}