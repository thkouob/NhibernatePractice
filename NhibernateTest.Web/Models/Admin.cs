using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace NhibernateTest
{
    public class Admin : User
    {

        public virtual string Phone
        {
            get;
            set;
        }
    }
}
