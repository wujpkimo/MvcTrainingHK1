using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManager.Handler;
using CustomerManager.Models;

namespace CustomerManager.Controllers
{
    public class 客戶聯絡人Controller : BaseController
    {
        public 客戶聯絡人Controller()
        {
            客戶聯絡人repo = RepositoryHelper.Get客戶聯絡人Repository();
            客戶資料repo = RepositoryHelper.Get客戶資料Repository(客戶聯絡人repo.UnitOfWork);
        }

        // GET: 客戶聯絡人
        [TimingActionFilter]
        public ActionResult Index(int page = 1
            , string sortOrder = ""
            , string currentFilter = ""
            , string 職稱 = "", string custName = "")
        {
            if (Session == null)
                return RedirectToAction("Login", "Login");
            else if (Session["Login"] as string != "Admin")
                return RedirectToAction("Login", "Login");

            if (string.IsNullOrEmpty(currentFilter))
                currentFilter = 職稱;
            var 客戶聯絡人 = 客戶聯絡人repo.SerachByCondition(currentFilter, custName);

            #region ViewBag配置

            ViewBag.職稱 = new SelectList(客戶聯絡人repo.All()
                .Select(c => new { c.職稱 }).Distinct(), "職稱", "職稱");
            ViewBag.CurrentFilter = currentFilter;

            #endregion ViewBag配置

            #region 排序

            ViewBag.客戶分類排序規則 = string.IsNullOrEmpty(sortOrder) ? "客戶分類反序" : "";
            ViewBag.客戶名稱排序規則 = sortOrder == "客戶名稱" ? "客戶名稱反序" : "客戶名稱";
            ViewBag.職稱排序規則 = sortOrder == "職稱" ? "職稱反序" : "職稱";
            ViewBag.姓名排序規則 = sortOrder == "姓名" ? "姓名反序" : "姓名";
            ViewBag.手機排序規則 = sortOrder == "手機" ? "手機反序" : "手機";
            ViewBag.電話排序規則 = sortOrder == "電話" ? "電話反序" : "電話";
            ViewBag.Email排序規則 = sortOrder == "Email" ? "Email反序" : "Email";

            switch (sortOrder)
            {
                case "客戶分類反序":
                    客戶聯絡人 = 客戶聯絡人.OrderByDescending(s => s.客戶資料.客戶分類);
                    break;

                case "客戶名稱":
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.客戶資料.客戶名稱);
                    break;

                case "客戶名稱反序":
                    客戶聯絡人 = 客戶聯絡人.OrderByDescending(s => s.客戶資料.客戶名稱);
                    break;

                case "職稱":
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.職稱);
                    break;

                case "職稱反序":
                    客戶聯絡人 = 客戶聯絡人.OrderByDescending(s => s.職稱);
                    break;

                case "姓名":
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.姓名);
                    break;

                case "姓名反序":
                    客戶聯絡人 = 客戶聯絡人.OrderByDescending(s => s.姓名);
                    break;

                case "電話":
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.電話);
                    break;

                case "電話反序":
                    客戶聯絡人 = 客戶聯絡人.OrderByDescending(s => s.電話);
                    break;

                case "手機":
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.手機);
                    break;

                case "手機反序":
                    客戶聯絡人 = 客戶聯絡人.OrderByDescending(s => s.手機);
                    break;

                case "Email":
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.Email);
                    break;

                case "Email反序":
                    客戶聯絡人 = 客戶聯絡人.OrderByDescending(s => s.Email);
                    break;

                case "客戶分類":
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.客戶資料.客戶分類);
                    break;

                default:  // 依客戶名稱排序
                    客戶聯絡人 = 客戶聯絡人.OrderBy(s => s.客戶資料.客戶分類);
                    break;
            }

            #endregion 排序

            return View(客戶聯絡人.ToList());
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = 客戶聯絡人repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,Active")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                客戶聯絡人.Active = true;
                客戶聯絡人repo.Add(客戶聯絡人);
                客戶聯絡人repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = 客戶聯絡人repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,Active")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = 客戶聯絡人repo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(客戶資料repo.All(), "客戶Id", "客戶分類", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        /// <summary>
        /// 批次修改資料
        /// </summary>
        /// <param name="data">ClientBathVM</param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View =
            "Error_DbEntityValidationException")]
        public ActionResult BatchUpdate(客戶聯絡人Batch[] data)
        {
            //data[0].ClientId
            if (ModelState.IsValid)
            {
                foreach (var vm in data)
                {
                    var client = 客戶聯絡人repo.Find(vm.Id);
                    client.職稱 = vm.職稱;
                    client.電話 = vm.電話;
                    client.手機 = vm.手機;
                }

                客戶聯絡人repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewData.Model = 客戶聯絡人repo.All();
            return View("Index");
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = 客戶聯絡人repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = 客戶聯絡人repo.Find(id);
            客戶聯絡人repo.Delete(客戶聯絡人);
            客戶聯絡人repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                客戶聯絡人repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}