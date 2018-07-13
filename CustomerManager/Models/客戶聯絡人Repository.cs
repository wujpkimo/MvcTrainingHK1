using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerManager.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        //複寫讀取全部資料
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => p.Active != false);
        }
        //複寫刪除資料
        public override void Delete(客戶聯絡人 entity)
        {
            entity.Active = false;
        }


        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
        public IQueryable<客戶聯絡人> SerachByCondition(string 職稱)
        {
            var 客戶聯絡人 = this.All();
            if (!string.IsNullOrEmpty(職稱))
            {
                客戶聯絡人 = 客戶聯絡人.Where(p => p.職稱==職稱);
            }
            return 客戶聯絡人;
        }
    }

    public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}