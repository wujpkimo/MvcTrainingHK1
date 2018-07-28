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
using CustomerManager.Handler;
using CustomerManager.Models;

namespace CustomerManager.Controllers
{
    public class 客戶清單Controller : BaseController
    {
        private int pageSize = 10;

        public 客戶清單Controller()
        {
            客戶清單repo = RepositoryHelper.Get客戶清單Repository();
        }

        // GET: 客戶清單
        [TimingActionFilter]
        public ActionResult Index(int page = 1)
        {
            if (Session == null)
                return RedirectToAction("Login", "Login");
            else if (Session["Login"] as string != "Admin")
                return RedirectToAction("Login", "Login");

            return View(客戶清單repo.All());
        }

        public ActionResult Export客戶清單()
        {
            //取得資料
            var data = 客戶清單repo
                .All()
                .Select(c => new { c.客戶名稱, c.聯絡人數量, c.銀行帳戶數量 });
            return Export(data as IQueryable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                客戶清單repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}