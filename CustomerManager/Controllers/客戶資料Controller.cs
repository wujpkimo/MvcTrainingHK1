using CustomerManager.Handler;
using CustomerManager.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CustomerManager.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        private int pageSize = 10;
        public string order;

        public 客戶資料Controller()
        {
            客戶資料repo = RepositoryHelper.Get客戶資料Repository();
            客戶類型repo = RepositoryHelper.Get客戶類型Repository(客戶資料repo.UnitOfWork);
            客戶聯絡人repo = RepositoryHelper.Get客戶聯絡人Repository();
        }

        // GET: 客戶資料
        [TimingActionFilter]
        public ActionResult Index(客戶資料VM VM)
        {
            #region 排序

            if (!string.IsNullOrEmpty(VM.Order))
                VM.Order = VM.Order == "asc" ? "desc" : "asc";
            var 客戶資料 = 客戶資料repo.SerachSortByCondition(VM);

            #endregion 排序

            #region 分頁

            int currentPage = VM.Page < 1 ? 1 : VM.Page;
            var result = 客戶資料.ToPagedList(VM.Page, pageSize);

            #endregion 分頁

            #region ViewBag配置

            ViewBag.VM = VM;
            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類");

            #endregion ViewBag配置

            return View(result);
        }

        public JsonResult Get客戶分類()
        {
            var data = 客戶類型repo.All()
                .Select(c => c.客戶分類);
            var json = JsonConvert.SerializeObject(data);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: 客戶資料/Details/5
        [TimingActionFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = 客戶資料repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        [ChildActionOnly]
        public ActionResult 連絡人List(int id)
        {
            var 客戶聯絡人 = 客戶聯絡人repo
                .FindByCust(id);
            return PartialView(客戶聯絡人);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類");
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "客戶Id,客戶分類,客戶名稱,統一編號,電話,傳真,地址,Email,CreateDate,Active")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                客戶資料.CreateDate = DateTime.UtcNow;
                客戶資料.Active = true;
                客戶資料repo.Add(客戶資料);
                客戶資料repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);
            //ViewData["客戶分類"] = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = 客戶資料repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);
            return View("Edit", 客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "客戶Id,客戶分類,客戶名稱,統一編號,電話,傳真,地址,Email,CreateDate,Active")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = 客戶資料repo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = 客戶資料repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = 客戶資料repo.Find(id);
            客戶資料repo.Delete(客戶資料);
            客戶資料repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Export客戶資料(客戶資料VM 客戶資料SSVM)
        {
            //取得資料
            var data = 客戶資料repo
                .SerachByCondition(客戶資料SSVM.CustName, 客戶資料SSVM.CustNo, 客戶資料SSVM.Category)
                .Select(c => new { c.客戶分類, c.客戶名稱, c.統一編號, c.電話, c.傳真, c.地址, c.Email });

            return Export(data as IQueryable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                客戶資料repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}