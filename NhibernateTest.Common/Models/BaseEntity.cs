using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhibernateTest
{
    public abstract class BaseEntity
    {
        public virtual string Creator { get; set; }

        public virtual DateTime CreateTime { get; set; }

        public virtual string LastEditor { get; set; }

        public virtual DateTime LastTime { get; set; }

        public virtual EntityStatus EntityStatus { get; set; }
    }

    public abstract class BaseEntity<TId> : BaseEntity
    {
         public virtual TId Id { get; set; }
 
         public override bool Equals(object obj)
         {
             if (obj == null)
             {
                 return false;
             }
 
             //是否為子類
             var thisType = this.GetType();
             var targetType = obj.GetType();
             if (thisType.IsAssignableFrom(targetType) || targetType.IsAssignableFrom(thisType))
             {
                 return this.Id.Equals(this.Id);
             }
             else
             {
                 return false;
             }
         }
 
         public override int GetHashCode()
         {
             return this.ToString().GetHashCode();
         }
 
         public override string ToString()
         {
             return this.GetType().Name + ":" + this.Id;
         }
    }
}