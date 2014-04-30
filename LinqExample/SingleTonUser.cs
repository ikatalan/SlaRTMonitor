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



        //// A private instance of the class
        //private static SingletoneUser instance;
        //private System.Collections.Generic.List<string> Users;

        //// The constructor for this user is private, so that cannot be instantiated

        //private SingletoneUser()
        //{
        //    try
        //    {
        //        Users = new System.Collections.Generic.List<string>();
        //    }
        //    catch { }
        //}

        //// This private static property, will check if the static instance of the user
        //// is instantiated if it is then it will return the static instance, if not it will
        //// create a new instance.
        //public static SingletoneUser Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new SingletoneUser();
        //        }
        //        return instance;
        //    }
        //}

        //public void AddUser(string User)
        //{
        //    Users.Add(User);
        //}

        //public bool HasUser(string User)
        //{
        //    return Users.Contains(User);
        //}
    }

 }

