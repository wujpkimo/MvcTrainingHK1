using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using CustomerManager.Models;
using Newtonsoft.Json;
using PagedList;

namespace CustomerManager.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        private int pageSize = 10;

        public 客戶資料Controller()
        {
            客戶資料repo = RepositoryHelper.Get客戶資料Repository();
            客戶類型repo = RepositoryHelper.Get客戶類型Repository(客戶資料repo.UnitOfWork);
        }

        // GET: 客戶資料
        public ActionResult Index(int page = 1
            , string sortOrder = ""
            , string custName = "", string custNo = "", string category = "")
        {
            //by 條件顯示資料
            var 客戶資料 = 客戶資料repo.SerachByCondition(custName, custNo, category);

            #region ViewBag配置

            ViewBag.custName = custName;
            ViewBag.custNo = custNo;
            ViewBag.category = category;
            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類");

            #endregion ViewBag配置

            #region 排序

            ViewBag.客戶名稱排序規則 = string.IsNullOrEmpty(sortOrder) ? "客戶名稱反序" : "";
            ViewBag.統一編號排序規則 = sortOrder == "統一編號" ? "統一編號反序" : "統一編號";
            ViewBag.電話排序規則 = sortOrder == "電話" ? "電話反序" : "電話";
            ViewBag.傳真排序規則 = sortOrder == "傳真" ? "傳真反序" : "傳真";
            ViewBag.地址排序規則 = sortOrder == "地址" ? "地址反序" : "地址";
            ViewBag.Email排序規則 = sortOrder == "Email" ? "Email反序" : "Email";
            ViewBag.CreateDate排序規則 = sortOrder == "CreateDate" ? "CreateDate反序" : "CreateDate";
            ViewBag.客戶分類排序規則 = sortOrder == "客戶分類" ? "客戶分類反序" : "客戶分類";

            switch (sortOrder)
            {
                case "客戶名稱反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.客戶名稱);
                    break;

                case "統一編號":
                    客戶資料 = 客戶資料.OrderBy(s => s.統一編號);
                    break;

                case "統一編號反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.統一編號);
                    break;

                case "電話":
                    客戶資料 = 客戶資料.OrderBy(s => s.電話);
                    break;

                case "電話反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.電話);
                    break;

                case "傳真":
                    客戶資料 = 客戶資料.OrderBy(s => s.傳真);
                    break;

                case "傳真反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.傳真);
                    break;

                case "地址":
                    客戶資料 = 客戶資料.OrderBy(s => s.地址);
                    break;

                case "地址反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.地址);
                    break;

                case "Email":
                    客戶資料 = 客戶資料.OrderBy(s => s.Email);
                    break;

                case "Email反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.Email);
                    break;

                case "CreateDate":
                    客戶資料 = 客戶資料.OrderBy(s => s.CreateDate);
                    break;

                case "CreateDate反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.CreateDate);
                    break;

                case "客戶分類":
                    客戶資料 = 客戶資料.OrderBy(s => s.客戶分類);
                    break;

                case "客戶分類反序":
                    客戶資料 = 客戶資料.OrderByDescending(s => s.客戶分類);
                    break;

                default:  // 依客戶名稱排序
                    客戶資料 = 客戶資料.OrderBy(s => s.客戶名稱);
                    break;
            }

            #endregion 排序

            #region 分頁

            int currentPage = page < 1 ? 1 : page;
            //var result = 客戶資料.OrderBy(x => x.客戶Id).ToPagedList(page, pageSize);

            var result = 客戶資料.ToPagedList(page, pageSize);

            #endregion 分頁

            return View(result);
        }

        public JsonResult Get客戶分類()
        {
            var data = 客戶類型repo.All();
            return (JsonResult)data;
        }

        public ActionResult Search(string custName, string custNo, string category, int page = 1)
        {
            var 客戶資料 = 客戶資料repo.SerachByCondition(custName, custNo, category);

            int currentPage = page < 1 ? 1 : page;
            var result = 客戶資料.OrderBy(x => x.客戶Id).ToPagedList(page, pageSize);

            return View("Index", result);
        }

        // GET: 客戶資料/Details/5
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

        public ActionResult Export客戶資料(string custName = "", string custNo = "", string category = "")
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                //取得資料
                var data = 客戶資料repo
                    .SerachByCondition(custName, custNo, category)
                    .Select(c => new { c.客戶分類, c.客戶名稱, c.統一編號, c.電話, c.傳真, c.地址, c.Email });
                //建立sheet
                var ws = wb.Worksheets.Add("Data", 1);

                //修改標題列Style
                var header = ws.Range("A1:G1");
                header.Style.Fill.BackgroundColor = XLColor.Green;
                header.Style.Font.FontColor = XLColor.Yellow;
                header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                //將標題插入到shee中
                int colIdx = 1;
                foreach (PropertyInfo colName in data.ElementType.GetProperties())
                {
                    ws.Cell(1, colIdx++).Value = colName.Name;
                }
                //將資料插入到shee中
                ws.Cell(2, 1).InsertData(data);

                //自動設定欄寬
                for (int i = 1; i <= ws.ColumnCount(); i++)
                {
                    ws.Column(i).AdjustToContents();
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "List.xlsx");
                }
            }
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