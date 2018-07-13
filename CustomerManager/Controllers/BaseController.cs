using ClosedXML.Excel;
using CustomerManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CustomerManager.Controllers
{
    public abstract class BaseController : Controller
    {
        protected 客戶管理Entities db = new 客戶管理Entities();

        public 客戶資料Repository 客戶資料repo;
        public 客戶聯絡人Repository 客戶聯絡人repo;
        public 客戶銀行資訊Repository 客戶銀行資訊repo;
        public 客戶類型Repository 客戶類型repo;

        protected override void HandleUnknownAction(string actionName)
        {
            RedirectToAction("Index").ExecuteResult(this.ControllerContext);
            //base.HandleUnknownAction(actionName);
        }

        public ActionResult Export(IQueryable data)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                //建立sheet
                var ws = wb.Worksheets.Add("Data", 1);

                //將標題插入到sheet中
                int colIdx = 1;
                foreach (PropertyInfo colName in data.ElementType.GetProperties())
                {
                    ws.Cell(1, colIdx++).Value = colName.Name;
                }
                //將資料插入到shee中
                ws.Cell(2, 1).InsertData(data);

                //修改標題列Style
                var header = ws.Range(firstCell: ws.Cell(1, 1), lastCell: (ws.Cell(1, ws.ColumnCount())));
                header.Style.Fill.BackgroundColor = XLColor.Green;
                header.Style.Font.FontColor = XLColor.Yellow;
                header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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
    }
}