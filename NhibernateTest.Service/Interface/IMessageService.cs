using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhibernateTest.Service
{
    public interface IMessageService : IBaseService<int, Message>
    {
        Message GetByUser(string userName);
    }
}
