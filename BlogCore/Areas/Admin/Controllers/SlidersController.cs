using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlidersController(IUnitOfWork unitOfWork,
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string mainPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (slider.Id == 0 && files.Count > 0)
                {
                    //New Article
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(mainPath, @"images\sliders");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    slider.ImageUrl = @"\images\sliders\" + fileName + extension;
                    slider.CreatedAt = DateTime.Now.ToString();

                    _unitOfWork.sliderRepository.Add(slider);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "You should add an image");
                }

            }
            return View(slider);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var slider = new Slider();
            slider = _unitOfWork.sliderRepository.Get(id);

            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string mainPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var articleFromBd = _unitOfWork.sliderRepository.Get(slider.Id);

                if (files.Count > 0)
                {
                    //New Slider
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
                    slider.ImageUrl = @"\images\articles\" + fileName + extension;
                    slider.CreatedAt = DateTime.Now.ToString();

                    _unitOfWork.sliderRepository.Update(slider);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    slider.ImageUrl = articleFromBd.ImageUrl;
                }

            }
            _unitOfWork.sliderRepository.Update(slider);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sliderFromDb = _unitOfWork.sliderRepository.Get(id);
            string pathMainDirectory = _webHostEnvironment.WebRootPath;

            var pathImage = Path.Combine(pathMainDirectory, sliderFromDb.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(pathImage))
            {
                System.IO.File.Delete(pathImage);
            }

            if (sliderFromDb == null)
            {
                return Json(new { success = false, message = "Error deleting article" });
            }

            _unitOfWork.sliderRepository.Remove(sliderFromDb.Id);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Article Deleted" });
        }


        #region CallsToAPI

        [HttpGet]
        public IActionResult GetAll() => Json(new { data = _unitOfWork.sliderRepository.GetAll() });

        #endregion
    }
}
