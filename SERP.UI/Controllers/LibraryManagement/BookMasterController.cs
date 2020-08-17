using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarCodeGenerator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LibraryManagement;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using static System.Convert;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class BookMasterController : Controller
    {
        private readonly IGenericRepository<BookMasterModel, int> _IBookRepo;
        private readonly IGenericRepository<CourseMaster, int> _courseRepo;
        private readonly IGenericRepository<CategoryMaster, int> _categoryRepo;
        private readonly IGenericRepository<BookItemModel, int> _bookItemRepo;
        private readonly IGenericRepository<SubjectMaster, int> _subjectRepo;
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IGenericRepository<BookStatusModel, int> _bookStatusRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public BookMasterController(IGenericRepository<BookMasterModel, int> bookRepo, 
                                    IGenericRepository<CourseMaster, int> courseRepo,
                                    IGenericRepository<CategoryMaster, int> categoryRepo,
                                    IGenericRepository<BookItemModel, int> bookItemRepo,
                                    IHostingEnvironment hostingEnviroment,
                                    IGenericRepository<SubjectMaster, int> subjectRepo,
                                    IGenericRepository<BookStatusModel, int> bookStatusRepo,
                                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IBookRepo = bookRepo;
            _courseRepo = courseRepo;
            _categoryRepo = categoryRepo;
            _bookItemRepo = bookItemRepo;
            _hostingEnviroment = hostingEnviroment;
            _subjectRepo = subjectRepo;
            _bookStatusRepo = bookStatusRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }

        public async Task<IActionResult> CreateBook(int id)
        {
            try
            {
                var bookItems = await _bookItemRepo.GetList(x => x.IsActive == 1 && x.BookId == id);
                ViewBag.CourseList = await _courseRepo.GetList(x => x.IsActive == 1);
                ViewBag.CategorList = await _categoryRepo.GetList(x => x.IsActive == 1);
                ViewBag.BookStatus = await _bookStatusRepo.GetList(x => x.IsActive == 1);
                var bookModels = await _IBookRepo.GetSingle(x => x.Id == id);
                if (bookModels != null)
                    bookModels.TotalBookCount = ToInt32(bookItems?.Count());
                return PartialView("~/Views/LibraryManagement/_BookMasterPartial.cshtml", bookModels);
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
        public async Task<IActionResult> UpsertBook(BookMasterModel model, IFormFile ImagePath)
        {
            if (model.Id == 0)
            {
                string imagePath = string.Empty;
                
                if (ImagePath!=null && ImagePath.Length > 0)
                {
                    imagePath = (await UploadImage.UploadImageOnFolder(new List<IFormFile>() { ImagePath }, _hostingEnviroment)).First();
                }
                model.ImagePath = imagePath;
                var response = await _IBookRepo.CreateEntity(model);
                await _IBookRepo.CreateNewContext();
                var bookId = (await _IBookRepo.GetList(x => x.IsActive == 1)).Max(x => x.Id);
                if (response == Utilities.ResponseUtilities.ResponseStatus.AddedSuccessfully)
                {
                    ResponseStatus bookItemResponse = await InsertBookItems(model, bookId);
                    return Json(ResponseData.Instance.GenericResponse(bookItemResponse));
                }
            }
            else
            {
                string imagePath = model.ImagePath;
                if (ImagePath.Length > 0)
                {
                    imagePath = (await UploadImage.UploadImageOnFolder(new List<IFormFile>() { ImagePath }, _hostingEnviroment)).First();
                }
                var updateModel = CommanDeleteHelper.CommanUpdateCode<BookMasterModel>(model, 1);
                var updateResponse = await _IBookRepo.Update(model);
                if (updateResponse == ResponseStatus.UpdatedSuccessFully)
                {
                    var bookItemModels = await _bookItemRepo.GetList(x => x.IsActive == 1 && x.BookId == model.Id);
                    await _bookItemRepo.CreateNewContext();
                    bookItemModels.ToList().ForEach(item =>
                    {
                        item.IsActive = 0;
                        item.IsDeleted = 1;
                        item.BookStatus = 2;
                    });
                    var deleteResponse = await _bookItemRepo.Update(bookItemModels.ToArray());
                    await _bookItemRepo.CreateNewContext();
                    var response = await InsertBookItems(model, model.TotalBookCount);
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
            }
            return Json("Error in book creation please contact admin!");
        }

        private async Task<ResponseStatus> InsertBookItems(BookMasterModel model, int bookId)
        {
            List<BookItemModel> bookitems = new List<BookItemModel>();
            for (int i = 1; i <= model.TotalBookCount; i++)
            {
                BookItemModel bookItem = new BookItemModel();
                bookItem.BookId = bookId;
                bookItem.CreatedBy = 1;
                bookItem.CreatedDate = DateTime.Now.Date;
                bookItem.BINShelf = string.Empty;
                bookItem.BarCode = new BarCodeHelper().CreateBarCode(model.TitleName + i.ToString());
                bookItem.ISBNNumber = string.Empty;
                bookItem.BookBarCode = model.TitleName + i.ToString();
                bookItem.BookStatus = 2;
                bookitems.Add(bookItem);

            }
            var bookItemResponse = await _bookItemRepo.Add(bookitems.ToArray());
            return bookItemResponse;
        }

        public async Task<IActionResult> GetBookDetail()
        {
            try
            {
                var bookItems = await _bookItemRepo.GetList(x => x.IsActive == 1);
                var response = await _IBookRepo.GetList(x => x.IsActive == 1);
                response.ToList().ForEach(item =>
                {
                    item.TotalBookCount = bookItems.Where(x => x.BookId == item.Id && x.IsActive == 1).Count();
                });
                var model = (from BM in response
                             join CM in await _categoryRepo.GetList(x => x.IsActive == 1)
                             on BM.CategoryId equals CM.Id
                             join CSM in await _courseRepo.GetList(x => x.IsActive == 1)
                             on BM.CourseId equals CSM.Id
                             join SM in await _subjectRepo.GetList(x => x.IsActive == 1)
                             on BM.SubjectId equals SM.Id
                             select new BookMasterModelVm
                             {
                                 Id = BM.Id,
                                 BookName = BM.TitleName,
                                 AuthorName = BM.AuthorName,
                                 CategoryName = CM.Name,
                                 CourseName = CSM.Name,
                                 SubjectName = SM.SubjectName,
                                 PublisherName = BM.PublisherName,
                                 Language = BM.Language,
                                 Edition = BM.Edition.ToString(),
                                 Description = BM.Description,
                                 ImagePath = BM.ImagePath,
                                 CostPrice = BM.CostPrice,
                                 PurchaseDate = BM.PurchaseDate,
                                 TotalCount = BM.TotalBookCount
                             }).ToList();
                return PartialView("~/Views/LibraryManagement/_BookDetailPartial.cshtml", model);
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

        public async Task<IActionResult> GetBookItems(int bookId)
        {
            try
            {
                var response = await _bookItemRepo.GetList(x => x.IsActive == 1 && x.BookId == bookId);
                return PartialView("~/Views/LibraryManagement/_BookItemListPartial.cshtml", response);
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
        public async Task<IActionResult> UpsertBookItems()
        {
            string[] bookItemId = Request.Form["bookItemId"];
            var isbn = Request.Form["ISBN"];
            int[] itemIds = Array.ConvertAll(bookItemId, s => int.Parse(s));
            var bookIitems = await _bookItemRepo.GetList(x => x.Id.Equals(itemIds.ToList()));
            var bin = Request.Form["BIN"];
            for(int i=0; i<bookItemId.Count(); i++)
            {
                var  model = bookIitems.Where(x=>x.Id==ToInt32(bookItemId[i])).FirstOrDefault();
                model.ISBNNumber = isbn[i];
                model.BINShelf = bin[i];
            }
            return Json(ResponseData.Instance.GenericResponse(await _bookItemRepo.Update(bookIitems.ToArray())));
        }
    }
}