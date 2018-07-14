using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
	
namespace CustomerManager.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortString">欄位名稱</param>
        /// <param name="order">asc or desc</param>
        /// <returns></returns>
        public IQueryable<客戶資料> Sort(string sortString, string order = "")
        {
            var orderExpression = string.Format("{0} {1}", sortString, order);
            var data = this.All().OrderBy(orderExpression);
            return data;
        }

        //複寫讀取全部資料
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => p.Active != false);
        }
        //複寫刪除資料
        public override void Delete(客戶資料 entity)
        {
            entity.Active = false;
        }

        

        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.客戶Id == id);
        }

        public IQueryable<客戶資料> SerachSortByCondition(string custName, string custNo, string category, string sortString, string order)
        {
            var orderExpression = string.Format("{0} {1}", sortString, order);
            var data = this.All().OrderBy(orderExpression);

            if (!string.IsNullOrEmpty(custName))
            {
                data = data.Where(p => p.客戶名稱.Contains(custName));
            }
            if (!string.IsNullOrEmpty(custNo))
            {
                data = data.Where(p => p.統一編號.Contains(custNo));
            }
            if (!string.IsNullOrEmpty(category))
            {
                data = data.Where(p => p.客戶分類 == category);
            }

            return data;
        }

        public IQueryable<客戶資料> SerachByCondition(string custName, string custNo,string category)
        {
            var 客戶資料 = this.All();

            if (!string.IsNullOrEmpty(custName))
            {
                客戶資料 = 客戶資料.Where(p => p.客戶名稱.Contains(custName));
            }
            if (!string.IsNullOrEmpty(custNo))
            {
                客戶資料 = 客戶資料.Where(p => p.統一編號.Contains(custNo));
            }
            if (!string.IsNullOrEmpty(category))
            {
                客戶資料 = 客戶資料.Where(p => p.客戶分類 == category);
            }

            return 客戶資料;
        }
    }

    public  interface I客戶資料Repository : IRepository<客戶資料>
	{

    }
}