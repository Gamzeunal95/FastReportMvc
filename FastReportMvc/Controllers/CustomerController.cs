using FastReportMvc.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FastReportMvc.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index(IConfiguration configuration, NorthwindContext northwindContext)
        {


            return View();
        }
    }
}
