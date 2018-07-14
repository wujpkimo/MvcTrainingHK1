using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerManager.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        //public IQueryable<客戶銀行資訊> Sort(string sortString,string order="")
        //{
        //    var data = this.All().OrderBy(sortString);
        //    return data;
        //}
        //複寫讀取全部資料
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(p => p.Active != false);
        }
        //複寫刪除資料
        public override void Delete(客戶銀行資訊 entity)
        {
            entity.Active = false;
        }


        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
        public IQueryable<客戶銀行資訊> SerachByCondition(string custName = "")
        {
            var 客戶銀行資訊 = this.All();

            if (!string.IsNullOrEmpty(custName))
            {
                客戶銀行資訊 = 客戶銀行資訊.Where(p => p.客戶資料.客戶名稱 == custName);
            }
            return 客戶銀行資訊;
        }
    }

    public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}