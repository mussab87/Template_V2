﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Service.DataLayer
{
    public class AppConstants
    {
        #region App Roles
        public class Role
        {
            public class Names
            {
                public const string Admin_Role_Name = "Admin";
                public const string User_Role_Name = "User";
            }
        }
        #endregion

        #region App Account
        public class Account
        {
            public class Login
            {
                public const int AccessFailedCount = 3;
                public const int UserPasswordLength = 5;
                public const int UserSessionTimeOut = 30; //mins
                public const int MaxLimitToLockUser = 90; //3 months
                public const string UserConfirmPolicyTitle = "العنوان للسياسات";
                public const string UserConfirmPolicy = "جميع السياسات التي يجب ان يوافق عليها المستخدم قبل الدخول للنظام";
                public const string Admin_Password = "Pa$$w0rd";
            }
        }
        #endregion

        #region App Layout
        public class LayOut
        {
            public class Config
            {               
                public const string ApplicationName = "اسم النظام";
                public const string ApplicationLogo = "/img/logo.png"; 
                //public const string UserConfirmPolicy = "جميع السياسات التي يجب ان يوافق عليها المستخدم قبل الدخول للنظام";
            }
        }
        #endregion

        #region App Api
        public class ApiLink
        {
            public class ApiUrl
            {
                public const string EmpDataApiUrl = "http://10.1.21.38/";
            }
        }
        #endregion
    }
}