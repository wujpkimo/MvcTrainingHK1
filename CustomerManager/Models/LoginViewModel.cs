using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerManager.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(12, MinimumLength = 5, ErrorMessage = "帳號長度需在5-12碼之間")]
        public string 帳號 { get; set; }

        [DataType(DataType.Password)]
        public string 密碼 { get; set; }
    }
}