using CustomerManager.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CustomerManager.Controllers
{
    public class 客戶類型Controller : BaseController
    {
        // GET: 客戶類型
        public ActionResult Index()
        {
            return View(客戶類型repo.All().ToList());
        }

        // GET: 客戶類型/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶類型 客戶類型 = 客戶類型repo.Find(id.Value);
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
                客戶類型repo.Add(客戶類型);
                客戶類型repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶類型);
        }

        // GET: 客戶類型/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶類型 客戶類型 = 客戶類型repo.Find(id.Value);
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
                var db = 客戶類型repo.UnitOfWork.Context;
                db.Entry(客戶類型).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶類型);
        }

        // GET: 客戶類型/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶類型 客戶類型 = 客戶類型repo.Find(id.Value);
            if (客戶類型 == null)
            {
                return HttpNotFound();
            }
            return View(客戶類型);
        }

        // POST: 客戶類型/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            客戶類型 客戶類型 = 客戶類型repo.Find(id.Value);
            客戶類型repo.Delete(客戶類型);
            客戶類型repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                客戶類型repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}