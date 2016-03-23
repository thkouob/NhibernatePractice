using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhibernateTest.Service
{
    public interface IBaseService<TId, TEntity> where TEntity : class
    {
        TEntity Get(TId id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
