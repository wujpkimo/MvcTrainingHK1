using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManager.Handler;
using CustomerManager.Models;

namespace CustomerManager.Controllers
{
    public class 客戶銀行資訊Controller : BaseController
    {
        public 客戶銀行資訊Controller()
        {
            客戶銀行資訊repo = RepositoryHelper.Get客戶銀行資訊Repository();
            客戶資料repo = RepositoryHelper.Get客戶資料Repository(客戶銀行資訊repo.UnitOfWork);
        }

        // GET: 客戶銀行資訊
        [TimingActionFilter]
        public ActionResult Index(int page = 1
            , string sortOrder = ""
            , string currentFilter = ""
            , string custName = "")
        {
            if (Session == null)
                return RedirectToAction("Login", "Login");
            else if (Session["Login"] as string != "Admin")
                return RedirectToAction("Login", "Login");

            //var 客戶銀行資訊 = 客戶銀行資訊repo.All();
            var 客戶銀行資訊 = 客戶銀行資訊repo.SerachByCondition(custName);

            //var 客戶銀行資訊 = 客戶銀行資訊repo.All().Include(p => p.客戶資料);
            return View(客戶銀行資訊.ToList());
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = 客戶銀行資訊repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,Active")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                客戶銀行資訊.Active = true;
                客戶銀行資訊repo.Add(客戶銀行資訊);
                客戶銀行資訊repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = 客戶銀行資訊repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶分類", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,Active")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = 客戶銀行資訊repo.UnitOfWork.Context;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = 客戶銀行資訊repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = 客戶銀行資訊repo.Find(id);
            客戶銀行資訊repo.Delete(客戶銀行資訊);
            客戶銀行資訊repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                客戶銀行資訊repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}