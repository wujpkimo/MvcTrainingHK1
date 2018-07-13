using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerManager.Models
{   
	public  class 客戶清單Repository : EFRepository<客戶清單>, I客戶清單Repository
	{

	}

	public  interface I客戶清單Repository : IRepository<客戶清單>
	{

	}
}