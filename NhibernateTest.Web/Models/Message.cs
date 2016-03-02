﻿using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace NhibernateTest
{
    public class Message : BaseEntity<int>
    {
        public virtual string Content
        { get; set; }

        public virtual MessageType Type
        { get; set; }

        public virtual File[] Files
        { get; set; }

        public virtual Comment[] Comments
        { get; set; }
    }
}
