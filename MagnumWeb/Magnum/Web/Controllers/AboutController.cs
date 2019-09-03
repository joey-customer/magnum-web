using Microsoft.AspNetCore.Mvc;

namespace Magnum.Web.Controllers
{
    public class AboutController : BaseController
    {
        [HttpGet("About")]
        public IActionResult About()
        {
            return View();
        }
    }
}
