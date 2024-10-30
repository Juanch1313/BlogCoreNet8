using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid) 
            {
                _unitOfWork.categoryRepository.Add(category);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = new Category();
            category = _unitOfWork.categoryRepository.Get(id);

            if(category  == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.categoryRepository.Update(category);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.categoryRepository.Get(id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error deleting caregory"});
            }

            _unitOfWork.categoryRepository.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Category Deleted" });
        }

        #region CallsToAPI

        [HttpGet]
        public IActionResult GetAll() => Json(new { data = _unitOfWork.categoryRepository.GetAll() });

        #endregion
    }
}
