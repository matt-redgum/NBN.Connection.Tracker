using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBN.Connection.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeModel();
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +    Request.ApplicationPath.TrimEnd('/') + "/";
            model.SiteRoot = baseUrl;
            return View(model);
        }
        

    }

    public class HomeModel
    {
        public string SiteRoot { get; set; }
    }

}