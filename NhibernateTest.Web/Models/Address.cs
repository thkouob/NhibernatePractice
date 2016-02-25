using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace NhibernateTest
{
    public class AddressInfo
    {

        public virtual int Address
        { get; set; }

        public virtual int Country
        { get; set; }

        public virtual string ZipCode
        {
            get;
            set;
        }
    }
}
