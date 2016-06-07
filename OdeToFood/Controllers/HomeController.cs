using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Configuration;

namespace OdeToFood.Controllers
{
    //[Authorize(Roles="admin, sales")]
    public class HomeController : Controller
    {
        IOdeToFoodDB _db;

        public HomeController()
        {
            _db = new OdeToFoodDB3();
        }

        public HomeController(IOdeToFoodDB db)
        {
            _db = db;
        }

        public ActionResult Autocomplete(string term)
        {
            var model = _db.Query<Restaurant>()
                .Where(r => r.Name.StartsWith(term))
                .Take(10)
                .Select(r => new { label = r.Name });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //[ChildActionOnly]
        //[OutputCache(Duration = 60)]
        //public ActionResult SayHello()
        //{
        //    return Content("Hello!!!");
        //}

        //[OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With")]
        public ActionResult Index(string searchTerm = null, int page = 1)
        {
            ViewBag.MailServer = ConfigurationManager.AppSettings["MailServer"];

            //var model = _db.Restaurants.ToList();

            //var model =
            //    from r in _db.Restaurants
            //    where r.Name.StartsWith(searchTerm)
            //    orderby r.Reviews.Average(re => re.Rating) descending
            //    select new RestaurantListViewModel
            //    {
            //        Id = r.Id,
            //        Name = r.Name,
            //        City = r.City,
            //        Country = r.Country,
            //        CountOfReviews = r.Reviews.Count()
            //    };

            var model = _db.Query<Restaurant>()
                .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm))
                .OrderByDescending(r => r.Reviews.Average(re => re.Rating))
                .Select(r => new RestaurantListViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    City = r.City,
                    Country = r.Country,
                    CountOfReviews = r.Reviews.Count()
                })
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Restaurants", model);
            }

            return View(model);
        }


        public ActionResult About()
        {
            var model = new AboutModel();
            model.Name = "Osmartin";
            model.Location = "La Chusmita, Matanzas";

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
