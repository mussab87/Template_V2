using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Template.Models
{
    public class Soldier
    {
        [Required(ErrorMessage = " الرقم العسكري حقل إجباري")]
        [Display(Name = "الرقم العسكري*")]
        public int EmployeeNo { get; set; }


        [Required(ErrorMessage = " الهوية الوطنية حقل إجباري")]
        [Display(Name = "الهوية الوطنية*")]
        [JsonPropertyName("emp-id")]
        public int EmpId { get; set; }


        [Display(Name = "الاسم*")]
        public string EmployeeName { get; set; }


        [Display(Name = "الرتبة*")]
        public string EmployeeRank { get; set; }


        [Display(Name = "الوحدة*")]
        public string UnitName { get; set; }


        [Display(Name = "المسمى الوظيفي*")]
        public string JobName { get; set; }


        [Display(Name = "المنطقة*")]
        public string Region { get; set; }

        [Display(Name = "رقم الجوال*")]
        public int MobileNum { get; set; }
    }
}
