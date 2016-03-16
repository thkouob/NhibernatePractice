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

            var mapper = new ModelMapper();
            mapper.AddMapping(typeof(UserEntityMap));
            mapper.AddMapping(typeof(AdminEntityMap));
            mapper.AddMapping(typeof(FileEntityMap));
            mapper.AddMapping(typeof(ImageEntityMap));
            mapper.AddMapping(typeof(VideoEntityMap));
            mapper.AddMapping(typeof(MessageEntityMap));
            mapper.AddMapping(typeof(CommentEntityMap));

            var maps = mapper.CompileMappingForAllExplicitlyAddedEntities();
            config.AddDeserializedMapping(maps, "Models");

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
                var message = new Message()
                {
                    Content = "Leoli"
                };
                session.Save(message);
                session.Save(new Comment() { Message = message, Content = "Hi" });
                session.Save(new Comment() { Message = message, Content = "Nhibernate" });
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
