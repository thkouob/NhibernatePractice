using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;

namespace NhibernateTest
{
    public class Admin : User
    {

        public virtual string Phone
        {
            get;
            set;
        }

        public virtual IDictionary<string, string> Setting { get; set; }
    }
}
