namespace CustomerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶類型MetaData))]
    public partial class 客戶類型 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.客戶資料.Count>0)
            {
                yield return new ValidationResult("此客戶分類已有客戶資料,不可異動！", new string[] { "客戶分類" });
            }
        }
    }

    public partial class 客戶類型MetaData
    {

        [Required]
        public int id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶分類 { get; set; }
    
        public virtual ICollection<客戶資料> 客戶資料 { get; set; }


    }
}
