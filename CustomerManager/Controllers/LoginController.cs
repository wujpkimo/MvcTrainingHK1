using CustomerManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            //Session["Login"] = null;
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            //FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Login");
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
                    //Session["Login"] = "Customer";
                    LoginProcess("Customer", 客戶資料.客戶Id.ToString(), false); //表單驗證方法
                    return RedirectToAction("Edit", "Login", new { id = 客戶資料.客戶Id });
                }
                if (客戶資料 == null && login.帳號 == "Admin" && login.密碼 == "123qweasd")
                {
                    //Session["Login"] = "Admin";
                    LoginProcess("Admin", "Admin", false); //表單驗證方法
                    return RedirectToAction("Index", "客戶清單");
                }
            }
            return View();
        }

        private void LoginProcess(string level, string id, bool isRemeber)
        {
            var now = DateTime.Now;

            string roles = level;
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: id.ToString(), //這邊看個人，你想放使用者名稱也可以，自行更改
                issueDate: now,//現在時間
                expiration: now.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                isPersistent: isRemeber,//記住我 true or false
                userData: roles, //這邊可以放使用者名稱，而我這邊是放使用者的群組代號
                cookiePath: FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket); //把驗證的表單加密
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
        }

        // GET: 客戶資料/Edit/5
        [Authorize(Roles = "Customer")]
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