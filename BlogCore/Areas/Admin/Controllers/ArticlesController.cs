using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticlesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticlesController(IUnitOfWork unitOfWork, 
                                  IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticleViewModel artiVm = new ArticleViewModel()
            {
                article = new Models.Article(),
                categoryList = _unitOfWork.categoryRepository.GetCategoryList()
            };
            return View(artiVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleViewModel articleVM)
        {
            if (ModelState.IsValid)
            {
                string mainPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(articleVM.article.Id == 0 && files.Count > 0)
                {
                    //New Article
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(mainPath, @"images\articles");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    articleVM.article.ImageUrl = @"\images\articles\" + fileName + extension;
                    articleVM.article.CreatedAt = DateTime.Now.ToString();

                    _unitOfWork.articleRepository.Add(articleVM.article);
                    _unitOfWork.Save();
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "You should add an image");
                }

            }
            articleVM.categoryList = _unitOfWork.categoryRepository.GetCategoryList();
            return View(articleVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticleViewModel artiVm = new ArticleViewModel()
            {
                article = new Models.Article(),
                categoryList = _unitOfWork.categoryRepository.GetCategoryList()
            };
            if (id != null)
            {
                artiVm.article = _unitOfWork.articleRepository.Get(id.GetValueOrDefault());
            }
            return View(artiVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleViewModel articleVM)
        {
            if (ModelState.IsValid)
            {
                string mainPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var articleFromBd = _unitOfWork.articleRepository.Get(articleVM.article.Id);

                if (files.Count > 0)
                {
                    //New Article
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(mainPath, @"images\articles");
                    var extension = Path.GetExtension(files[0].FileName);

                    var newExtension = Path.GetExtension(files[0].FileName);
                    var pathImage = Path.Combine(mainPath, articleFromBd.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(pathImage))
                    {
                        System.IO.File.Delete(pathImage);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    articleVM.article.ImageUrl = @"\images\articles\" + fileName + extension;
                    articleVM.article.CreatedAt = DateTime.Now.ToString();

                    _unitOfWork.articleRepository.Update(articleVM.article);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articleVM.article.ImageUrl = articleFromBd.ImageUrl;
                }

            }
            _unitOfWork.articleRepository.Update(articleVM.article);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articleFromDb = _unitOfWork.articleRepository.Get(id);
            string pathMainDirectory = _webHostEnvironment.WebRootPath;

            var pathImage = Path.Combine(pathMainDirectory, articleFromDb.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(pathImage))
            {
                System.IO.File.Delete(pathImage);
            }

            if (articleFromDb == null)
            {
                return Json(new { success = false, message = "Error deleting article" });
            }

            _unitOfWork.articleRepository.Remove(articleFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Article Deleted" });
        }


        #region CallsToAPI

        [HttpGet]
        public IActionResult GetAll() => Json(new { data = _unitOfWork.articleRepository.GetAll(includeProperties: "Category") });

        #endregion
    }
}
