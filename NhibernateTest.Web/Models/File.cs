using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace NhibernateTest
{
    public class File : BaseEntity<int>
    {
        public virtual string Name
        { get; set; }

        public virtual string DisplayName
        { get; set; }

        public virtual int Category
        { get; set; }

        public virtual int Sort
        { get; set; }
    }
}
