using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhibernateTest
{
    public enum EntityStatus
    {
        /// <summary>
        /// 0
        /// </summary>
        Enabled = 0,
        /// <summary>
        /// 1
        /// </summary>
        Disabled = 1,
        /// <summary>
        /// 2
        /// </summary>
        Deleted = 2
    }
}