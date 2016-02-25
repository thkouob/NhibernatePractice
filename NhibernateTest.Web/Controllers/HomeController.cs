using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhibernateTest.Web.Controllers
{
    public class HomeController : Controller
    {
        private ProductService _prodcutService;
        
        public HomeController()
        {
            this._prodcutService = new ProductService();
        }
        // GET: Home
        public ActionResult Index()
        {
            var models = _prodcutService.GetAll();
            return Content("Ok");
        }
    }
}