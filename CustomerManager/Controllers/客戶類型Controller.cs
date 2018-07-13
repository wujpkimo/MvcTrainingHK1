using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManager.Models;

namespace CustomerManager.Controllers
{
    public class 客戶類型Controller : Controller
    {
        private 客戶管理Entities db = new 客戶管理Entities();

        // GET: 客戶類型
        public ActionResult Index()
        {
            return View(db.客戶類型.ToList());
        }

        // GET: 客戶類型/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶類型 客戶類型 = db.客戶類型.Find(id);
            if (客戶類型 == null)
            {
                return HttpNotFound();
            }
            return View(客戶類型);
        }

        // GET: 客戶類型/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶類型/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,客戶分類")] 客戶類型 客戶類型)
        {
            if (ModelState.IsValid)
            {
                db.客戶類型.Add(客戶類型);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶類型);
        }

        // GET: 客戶類型/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶類型 客戶類型 = db.客戶類型.Find(id);
            if (客戶類型 == null)
            {
                return HttpNotFound();
            }
            return View(客戶類型);
        }

        // POST: 客戶類型/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,客戶分類")] 客戶類型 客戶類型)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶類型).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶類型);
        }

        // GET: 客戶類型/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶類型 客戶類型 = db.客戶類型.Find(id);
            if (客戶類型 == null)
            {
                return HttpNotFound();
            }
            return View(客戶類型);
        }

        // POST: 客戶類型/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            客戶類型 客戶類型 = db.客戶類型.Find(id);
            db.客戶類型.Remove(客戶類型);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}