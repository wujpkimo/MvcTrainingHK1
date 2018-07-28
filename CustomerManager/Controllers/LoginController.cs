using CustomerManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CustomerManager.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController()
        {
            客戶資料repo = RepositoryHelper.Get客戶資料Repository();
            客戶類型repo = RepositoryHelper.Get客戶類型Repository(客戶資料repo.UnitOfWork);
        }

        // GET: Login
        public ActionResult Login()
        {
            Session["Login"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                string password = PasswordProcess.GetHashPassword(login.密碼);
                客戶資料 客戶資料 = 客戶資料repo
                    .Where(p => p.帳號 == login.帳號 && p.密碼 == password)
                    .FirstOrDefault();
                if (客戶資料 != null)
                {
                    Session["Login"] = "Customer";
                    return RedirectToAction("Edit", "Login", new { id = 客戶資料.客戶Id });
                }
                if (客戶資料 == null && login.帳號 == "Admin" && login.密碼 == "123qweasd")
                {
                    Session["Login"] = "Admin";
                    return RedirectToAction("Index", "客戶清單");
                }
            }
            return View();
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
            客戶資料.密碼 = "";
            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);
            return View("Edit", 客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "客戶Id,客戶分類,客戶名稱,統一編號,電話,傳真,地址,Email,帳號,密碼,CreateDate,Active")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                if (客戶資料.密碼 == null)
                {
                    客戶資料.密碼 = 客戶資料repo
                        .Where(p => p.客戶Id == 客戶資料.客戶Id)
                        .Select(p => p.密碼)
                        .FirstOrDefault();
                }
                else
                {
                    客戶資料.密碼 = PasswordProcess.GetHashPassword(客戶資料.密碼);
                }
                var db = 客戶資料repo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "修改成功！";
            }
            ViewBag.客戶分類 = new SelectList(客戶類型repo.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);
            return View("Edit", 客戶資料);
        }
    }
}