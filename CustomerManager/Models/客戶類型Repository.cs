using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerManager.Models
{   
	public  class 客戶類型Repository : EFRepository<客戶類型>, I客戶類型Repository
	{

	}

	public  interface I客戶類型Repository : IRepository<客戶類型>
	{

	}
}