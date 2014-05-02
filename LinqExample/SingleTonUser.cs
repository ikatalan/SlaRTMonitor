using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqExample
{
    static class SingletoneUser
    {
        private static string userName;
        private static string userPass;
        private static int role; // 0 - Admin 1 - Viewer

        public static string UserPass
        {
            get
            {
                return userPass;
            }
            set
            {
                userPass = value;
            }
        }

        public static string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        public static int Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
            }
        }
    }

 }

