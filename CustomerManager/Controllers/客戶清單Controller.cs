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
        public ActionResult Index(int page = 1)
        {
            return View(客戶清單repo.All());
        }

        public ActionResult Export客戶清單()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                //取得資料
                var data = 客戶清單repo
                    .All()
                    .Select(c => new { c.客戶名稱, c.聯絡人數量, c.銀行帳戶數量 });
                //建立sheet
                var ws = wb.Worksheets.Add("Data", 1);

                //修改標題列Style
                var header = ws.Range("A1:C1");
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
                客戶清單repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}