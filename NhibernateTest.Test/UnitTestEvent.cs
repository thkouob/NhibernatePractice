using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Event;
using NhibernateTest.Service;
using NHibernate.Dialect;
using NHibernate.Cfg.MappingSchema;


namespace NhibernateTest.Test
{
    [TestClass]
    public class UnitTestEvent
    {
        [TestMethod]
        public void TestEvent()
        {
            var config = new NHibernate.Cfg.Configuration();
            config.Configure();
            config.DataBaseIntegration(db =>
            {
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            });

            config.EventListeners.SaveEventListeners = new ISaveOrUpdateEventListener[] 
            { 
                new TestSaveOrUpdateEventListener()
            };

            config.AddDeserializedMapping(InternalHelper.GetAllMapper(), "Models");

            var factory = config.BuildSessionFactory();

            using (var session = factory.OpenSession())
            {
                session.Save(new Message() { Content = "Message1" });
                session.Flush();
            }

            using (var session = factory.OpenSession())
            {
                var message = session.Get<Message>(1);
                Assert.IsNotNull(message.Creator);
                Assert.AreEqual(message.LastEditor, "Leoli_SaveOrUpdate_Event");
            }
        }

        [TestMethod]
        public void TestEvent2()
        {
            var config = new NHibernate.Cfg.Configuration();
            config.Configure();
            config.DataBaseIntegration(db =>
            {
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            });

            config.SetListener(ListenerType.PreUpdate, new TestUpdateEventListener());

            config.AddDeserializedMapping(InternalHelper.GetAllMapper(), "Models");

            var factory = config.BuildSessionFactory();

            using (var session = factory.OpenSession())
            {
                session.Save(new Message() { Content = "Message1", Creator = "Leoli_EventTest" });
                session.Flush();
            }

            using (var session = factory.OpenSession())
            {
                var message = session.Get<Message>(1);
                message.Content = "Message_Leoli2";
                session.Save(message);
                session.Flush();
                Assert.AreEqual(message.LastEditor, "Leoli_Update_Event");
            }
        }
    }
}
