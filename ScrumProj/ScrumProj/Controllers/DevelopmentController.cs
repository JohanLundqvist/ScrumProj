using ScrumProj.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class DevelopmentController : Controller
    {
        // GET: Development
        [Authorize]
        public ActionResult DevelopmentWork(DevelopmentViewModel model)
        {       if(ModelState.IsValid)
            {

            }

                return View(model);
        }
    }
}