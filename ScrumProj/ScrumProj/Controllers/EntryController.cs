using ScrumProj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class EntryController : Controller
    {
        
        // GET: Entry
        public ActionResult Index()
        {
            return View();
        }

    }
}