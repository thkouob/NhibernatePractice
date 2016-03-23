using System;
using System.Linq;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Bytecode;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;


namespace NhibernateTest.Test
{
    [TestClass]
    public class UnitTestNhibernate
    {
        private static NHibernate.ISessionFactory Init()
        {
            var config = new Configuration();
            config.Proxy(proxy => proxy.ProxyFactoryFactory<DefaultProxyFactoryFactory>());
            config.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.ConnectionString = @"Data Source=|DataDirectory|\Test.db";
                //db.Driver<SQLite20Driver>();
                db.SchemaAction = SchemaAutoAction.Create;
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            });

            config.AddDeserializedMapping(InternalHelper.GetAllMapper(), "Models");

            return config.BuildSessionFactory();
        }

        [TestMethod]
        public void Initialization()
        {
            Init();
        }

        [TestMethod]
        public void Test_Message()
        {
            var factory = Init();
            Console.WriteLine("Create Message Info");
            using (var session = factory.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    try
                    {
                        var message = new Message()
                                   {
                                       Content = "Leoli"
                                   };
                        session.Save(message);
                        session.Save(new Comment() { Message = message, Content = "Hi" });
                        session.Save(new Comment() { Message = message, Content = "Nhibernate" });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        trans.Rollback();
                    }
                }
            }

            Console.WriteLine("Select Message Info");
            using (var session = factory.OpenSession())
            {
                var message = session.Get<Message>(1);

                Assert.AreEqual("Leoli", message.Content);

                Console.WriteLine("Lazy Load");

                Assert.AreEqual(2, message.Comments.Count);
            }
        }
    }
}
