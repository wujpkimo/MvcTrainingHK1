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
    public class 客戶清單Controller : Controller
    {
        private Entities db = new Entities();

        // GET: 客戶清單
        public ActionResult Index()
        {
            return View(db.客戶清單.ToList());
        }

        // GET: 客戶清單/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶清單 客戶清單 = db.客戶清單.Find(id);
            if (客戶清單 == null)
            {
                return HttpNotFound();
            }
            return View(客戶清單);
        }

        // GET: 客戶清單/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶清單/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "客戶Id,客戶名稱,聯絡人數量,銀行帳戶數量")] 客戶清單 客戶清單)
        {
            if (ModelState.IsValid)
            {
                db.客戶清單.Add(客戶清單);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶清單);
        }

        // GET: 客戶清單/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶清單 客戶清單 = db.客戶清單.Find(id);
            if (客戶清單 == null)
            {
                return HttpNotFound();
            }
            return View(客戶清單);
        }

        // POST: 客戶清單/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "客戶Id,客戶名稱,聯絡人數量,銀行帳戶數量")] 客戶清單 客戶清單)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶清單).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶清單);
        }

        // GET: 客戶清單/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶清單 客戶清單 = db.客戶清單.Find(id);
            if (客戶清單 == null)
            {
                return HttpNotFound();
            }
            return View(客戶清單);
        }

        // POST: 客戶清單/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶清單 客戶清單 = db.客戶清單.Find(id);
            db.客戶清單.Remove(客戶清單);
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
