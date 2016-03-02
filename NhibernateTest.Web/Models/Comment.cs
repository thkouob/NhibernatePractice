using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace NhibernateTest
{
    public class Comment : BaseEntity<int>
    {

        public virtual string Content
        { get; set; }

        public virtual Message Message
        { get; set; }
    }
}
