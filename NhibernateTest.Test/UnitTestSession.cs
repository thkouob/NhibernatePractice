using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Linq;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;

namespace NhibernateTest.Test
{
    [TestClass]
    public class UnitTestSession
    {
        [TestInitialize]
        public void Init() 
        {
            var utility = new NHibernateUtility();
            utility.Initialize();
        }

        [TestMethod]
        public void TestMethod_Transaction()
        {
            using (var session = NHibernateUtility.SessionFactory.OpenSession()) 
            {
                using (var trans = session.BeginTransaction()) 
                {
                    try 
                    {
                        Message m = new Message()
                        {
                            Content = "Hi,Leo"
                        };
                        session.Save(m);

                        Comment c = new Comment()
                        {
                            Message = m,
                            Content = "Say hi"
                        };
                        session.Save(c);
                        session.Flush();
                        trans.Commit();
                        Assert.IsTrue(true);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Assert.Fail();
                    }
                    
                }
            }
        }

        [TestMethod]
        public void TestMethod_Merge() 
        {
            Message message;
            using (var session = NHibernateUtility.SessionFactory.OpenSession()) 
            {
                message = new Message()
                {
                    Content = "Hi, leoli2"
                };
                session.Save(message);
                session.Flush();
            }

            using(var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message.Content = "merge...";
                var mergeMessage = session.Merge(message);
                session.Flush();

                ////Content => Merge.....
                Assert.AreEqual(mergeMessage.Content, message.Content);  ////新實例與舊實例會merge
                Assert.AreNotSame(mergeMessage, message);

                session.Flush();
            }

            using (var session = NHibernateUtility.SessionFactory.OpenSession()) 
            {
                message.Id = 3;
                message.Content = "xxxxxxxx===========";
                message = session.Merge(message); ////沒有id是3
                Assert.AreEqual(2, message.Id);  ////新增一筆資料,因此自動編號為2
                session.Flush();
            }
        }

        [TestMethod]
        public void TestMethod_Replicate()
        {
            Message message;
            using (var session = NHibernateUtility.SessionFactory.OpenSession()) 
            {
                message = new Message()
                {
                    Content = "Hi, Leoli",
                    Type = MessageType.Self
                };
                session.Save(message);
                session.Flush();
            }

            using (var session = NHibernateUtility.SessionFactory.OpenSession()) 
            {
                message.Content = "Leoli2";
                session.Replicate(message, NHibernate.ReplicationMode.Ignore);
                Assert.AreEqual("Hi, Leoli", session.Get<Message>(message.Id).Content);
                session.Flush();
            }

            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message.Content = "Leoli3";
                session.Replicate(message, NHibernate.ReplicationMode.Overwrite);
                Assert.AreEqual("Leoli3", session.Get<Message>(message.Id).Content);
                session.Flush();
            }

            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message.Id = 3;
                message.Content = "Leoli4";
                session.Replicate(message, NHibernate.ReplicationMode.Ignore);////沒有id是3
                Assert.AreEqual(2, message.Id); ////新增一筆資料,因此自動編號為2
                session.Flush();
            }
        }

        [TestMethod]
        public void TestMethod_Refresh()
        {
            Message message1;
            Message message2;
            using(var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message1 = new Message()
                {
                    Content = "one",
                    Type = MessageType.Self
                };
                session.Save(message1);
                session.Flush();
            }

            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message2 = new Message()
                {
                    Content = "two",
                    Id = message1.Id
                };
                session.Refresh(message2);
                session.Flush();
            }
        }

        [TestMethod]
        public void TestMethod_Contains()
        {
            Message message1;
            Message message2;
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message1 = new Message()
                {
                    Content = "one",
                    Type = MessageType.Self
                };
                session.Save(message1);
                session.Flush();
            }

            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message2 = new Message()
                {
                    Id = message1.Id
                };
                Assert.IsFalse(session.Contains(message2));
                session.Flush();
            } 
        }

        [TestMethod]
        public void TestMethod_GetAndLoad()
        {
            Message message;
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                message = new Message()
                {
                    Content = "one",
                    Type = MessageType.Self
                };
                session.Save(message);
                session.Flush();
            }

            Console.WriteLine("Get");
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                session.Get<Message>(message.Id);
                try 
                {
                    message = session.Get<Message>(message.Id + 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Get fail");
                }
            }

            Console.WriteLine("Load");
            using (var session = NHibernateUtility.SessionFactory.OpenSession()) 
            {
                message = session.Load<Message>(message.Id);
                Assert.IsNotNull(message);

                message = session.Load<Message>(message.Id + 1);
                
            }
            //Assert.IsNull(message);
        }

        [TestMethod]
        public void TestMethod_Query()
        {
            using (var session = NHibernateUtility.SessionFactory.OpenSession()) 
            {
                IEnumerable<Message> message;
                var day = DateTime.Today.AddDays(-7);
                
                message = session.Query<Message>().Where(x => x.CreateTime >= DateTime.Today.AddDays(-7)).ToList();
                message = session.QueryOver<Message>().Where(x => x.CreateTime >= DateTime.Today.AddDays(-7)).List();
                
                IQuery hQuery = session.CreateQuery("Select m From Message m Where m.CreateTime >= ?");
                hQuery.SetParameter(0, day);
                message = hQuery.List<Message>();

                ISQLQuery sqlQuery = session.CreateSQLQuery("Select * From Message m Where m.CreateTime >= ?");
                sqlQuery.SetParameter(0, day);
                message = sqlQuery.List<Message>();

                ICriteria criterQuery = session.CreateCriteria<Message>();
                criterQuery.Add(Expression.Ge("CreateTime", day));
                message = criterQuery.List<Message>();
            }
        }
    }
}
