using BlogCore.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(IUnitOfWork unitOfWork,
                                  IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Get all users
            return View(_unitOfWork.userRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Block(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unitOfWork.userRepository.BlockUser(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Unlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unitOfWork.userRepository.UnlockUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
