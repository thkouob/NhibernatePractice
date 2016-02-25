using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace NhibernateTest
{
    public class Image : NhibernateTest.File
    {

        public virtual int Width
        {
            get;
            set;
        }

        public virtual int Height
        {
            get;
            set;
        }
    }
}
