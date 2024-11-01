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



        #region CallsToAPI

        [HttpGet]
        public IActionResult GetAll() => Json(new { data = _unitOfWork.articleRepository.GetAll(includeProperties: "Category") });

        #endregion
    }
}
