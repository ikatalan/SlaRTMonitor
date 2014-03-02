using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqExample
{
    class SingleTonUser
    {

            // A private instance of the class

            private static SingleTonUser instance;
            private System.Collections.Generic.List<string> Users;

            // The constructor for this user is private, so that cannot be instantiated

            private SingleTonUser()
            {
            try
            {
            Users = new System.Collections.Generic.List<string>();
            }
            catch { }
            }

    // This private static property, will check if the static instance of the user
    // is instantiated if it is then it will return the static instance, if not it will
    // create a new instance.



            public static SingleTonUser Instance
            {
            get
            {
            if (instance == null)
            instance = new SingleTonUser();
            return instance;
            }
            }

            public void AddUser(string User)
            {
            Users.Add(User);
            }

            public bool HasUser(string User)
            {
                return Users.Contains(User);
            }
        }



 }

