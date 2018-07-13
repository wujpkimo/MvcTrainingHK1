using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerManager.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
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

        public IQueryable<客戶資料> SerachByCondition(string custName, string custNo,string category)
        {
            var 客戶資料 = this.All();
            //if (!string.IsNullOrEmpty(custName) && !string.IsNullOrEmpty(custNo) && !string.IsNullOrEmpty(category))
            //{
            //    客戶資料 = 客戶資料
            //        .Where(p => p.客戶名稱.Contains(custName) 
            //        && p.統一編號.Contains(custNo) && p.客戶分類==category);
            //}
            //else if (!string.IsNullOrEmpty(custName))
            //{
            //    客戶資料 = 客戶資料.Where(p => p.客戶名稱.Contains(custName));
            //}
            //else if (!string.IsNullOrEmpty(custNo))
            //{
            //    客戶資料 = 客戶資料.Where(p => p.統一編號.Contains(custNo));
            //}
            //else
            //    客戶資料 = this.All();
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