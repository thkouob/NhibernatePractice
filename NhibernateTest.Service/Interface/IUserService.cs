using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhibernateTest.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        
        User GetSingle(int id);

        void Insert(User model);

        void Update(int id, User model);

        void Delete(int id);
    }
}
