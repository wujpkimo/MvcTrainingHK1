namespace CustomerManager.Models
{
    public class 客戶資料VM
    {
        public string CustName { get; set; }
        public string CustNo { get; set; }
        public string Category { get; set; }

        public string SortString { get; set; } = "客戶名稱";
        public string Order { get; set; } = "asc";

        public int Page { get; set; } = 1;

        public 客戶資料VM SetSortString(string SortString)
        {
            this.SortString = SortString;
            return this;
        }
    }
}