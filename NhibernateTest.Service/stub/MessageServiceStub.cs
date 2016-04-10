using NhibernateTest.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhibernateTest
{
    public class MessageServiceStub : IMessageService
    {

        public Message Get(int id)
        {
            return new Message()
            {
                Id = id,
                Content = "Message"
            };
        }

        public void Add(Message entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Message entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Message entity)
        {
            throw new NotImplementedException();
        }

        public Message GetByUser(string userName)
        {
            return new Message()
            {
                Creator = userName,
                Id = 1,
                Content = "Stub"
            };
        }
    }
}
