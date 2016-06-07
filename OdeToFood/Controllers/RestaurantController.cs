using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Models;

namespace OdeToFood.Controllers
{
    public class RestaurantController : Controller
    {
        private OdeToFoodDB3 db = new OdeToFoodDB3();

        //
        // GET: /Restaurant/

        public ActionResult Index()
        {
            var model =
                from r in db.Restaurants
                orderby r.Reviews.Average(re => re.Rating) descending
                select r;

            return View(model);
        }

        ////
        //// GET: /Restaurant/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Restaurant restaurant = db.Restaurants.Find(id);
        //    if (restaurant == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(restaurant);
        //}

        //
        // GET: /Restaurant/Create
        [Authorize(Roles = "admin")] 
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Restaurant/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurant);
        }

        //
        // GET: /Restaurant/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        //
        // POST: /Restaurant/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        //
        // GET: /Restaurant/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        //
        // POST: /Restaurant/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}