using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DataLayer.Template.ViewModels
{
    public class AccountViewModel
    {                
        [Required(ErrorMessage = "إسم المستخدم حقل إجباري")]
        //[Remote(action: "IsEmailInUse", controller: "Account")]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Required(ErrorMessage = "كلمة المرور حقل إجباري")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور ")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور حقل إجباري")]
        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير مطابقة")]
        public string ConfirmPassword { get; set; }

        public List<IdentityRole> Roles { get; set; }

    }
}
