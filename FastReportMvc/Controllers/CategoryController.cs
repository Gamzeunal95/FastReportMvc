using Microsoft.AspNetCore.Mvc;

namespace FastReportMvc.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
