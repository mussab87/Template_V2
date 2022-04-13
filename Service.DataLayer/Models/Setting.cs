using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Service.DataLayer.Models
{
public class Setting
    {
        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "عدد محاولات غلق حساب مستخدم*")]
        public int AccessFailedCount { get; set; }

        [Required(ErrorMessage = " حقل اجباري")]
        [Display(Name = "طول كلمة المرور*")]
        public int? UserPasswordLength { get; set; }

        [Required(ErrorMessage = "  حقل إجباري")]
        [Display(Name = "احتواء كلمة المرور على ارقام*")]
        public bool? PassRequireDigit { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "احتواء كلمة المرور على حروف صغيرة*")]
        public bool? PassRequireLowercase { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "احتواء كلمة المرور على حروف كبيرة*")]
        public bool? PassRequireUppercase { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "احتواء كلمة المرور على رموز *")]
        public bool? PassRequireNonAlphanumeric { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "وقت انتهاء الجلسة للمستخدم*")]
        public int? UserSessionTimeOut { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "مدة اغلاق الحساب في حالة عدم الدخول*")]
        public int? MaxLimitToLockUser { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "عنوان السياسات *")]
        public string UserConfirmPolicyTitle { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "سياسات استخدام النظام*")]
        public string UserConfirmPolicy { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "اسم النظام*")]
        public string ApplicationName { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "صورة اللوقو للنظام*")]
        public string ApplicationLogo { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "رابط استرجاع بيانات الموظف*")]
        public string EmpDataApiUrl { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "اظهار سياسات النظام*")]
        public string EnableConfirmPolicy { get; set; }

        [Required(ErrorMessage = " حقل إجباري")]
        [Display(Name = "نظام حساس*")]
        public string EnableRightClick { get; set; }
    }
}
