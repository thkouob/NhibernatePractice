using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhibernateTest.Service
{
    public interface ICommentService : IBaseService<int, Comment>
    {
    }
}
