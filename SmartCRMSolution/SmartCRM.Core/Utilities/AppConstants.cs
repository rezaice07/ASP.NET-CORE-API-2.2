using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCRM.Core.Utilities
{
    public class AppConstants
    {
        public static class UserRoleConstants
        {
            public const int Admin = 1;
            public const int Customer = 2;
        }

        public static class StatusConstants
        {
            public const int Pending = 1;
            public const int Active = 2;
            public const int Deleted = 3;
        }
    }
}
