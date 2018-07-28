namespace CustomerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using System.Linq;

    [MetadataType(typeof(客戶資料MetaData))]
    public partial class 客戶資料 : IValidatableObject
    {

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶資料Repository 客戶資料repo = RepositoryHelper.Get客戶資料Repository();
            客戶資料 客戶資料 = 客戶資料repo
                .Where(p => p.帳號 == this.帳號 && p.客戶Id != this.客戶Id)
                .FirstOrDefault();        

            if (this.帳號!=null && 客戶資料!=null)
            {
                yield return new ValidationResult("帳號重複！", new string[] { "帳號" });
            }
        }
    }
    
    public partial class 客戶資料MetaData
    {
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 客戶分類 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶名稱 { get; set; }
        
        [StringLength(8, ErrorMessage="欄位長度不得大於 8 個字元")]
        [Required]
        [RegularExpression(@"\d{8}$", ErrorMessage ="請輸入正確的統一編號")]
        public string 統一編號 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 電話 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 傳真 { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string 地址 { get; set; }
        
        [EmailAddress]
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        public string Email { get; set; }

        [StringLength(12,MinimumLength = 5, ErrorMessage = "帳號長度需在5-12碼之間")]
        public string 帳號 { get; set; }

        [DataType(DataType.Password)]
        //[StringLength(12, MinimumLength = 8, ErrorMessage = "密碼長度需在8-12碼之間")]
        public string 密碼 { get; set; }

        //[Required]
        //[HiddenInput(DisplayValue = false)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}")]
        public System.DateTime CreateDate { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public bool Active { get; set; }
    
        public virtual 客戶類型 客戶類型 { get; set; }
        public virtual ICollection<客戶銀行資訊> 客戶銀行資訊 { get; set; }
        public virtual ICollection<客戶聯絡人> 客戶聯絡人 { get; set; }
    }
}
