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
