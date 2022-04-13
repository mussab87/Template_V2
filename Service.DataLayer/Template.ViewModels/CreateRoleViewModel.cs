﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DataLayer.Template.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "إسم الصلاحية حقل إجباري")]
        [Display(Name = "إسم الصلاحية")]
        //إسم الصلاحية حقل إجباري
        public string RoleName { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}
