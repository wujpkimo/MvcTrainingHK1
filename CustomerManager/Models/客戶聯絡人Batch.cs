using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerManager.Models
{
    public class 客戶聯絡人Batch
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }

        [RegularExpression(@"\d{4}-\d{6}", ErrorMessage = "請輸入正確的手機號碼格式ex.0918-123456")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    }
}