using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace NhibernateTest.Test
{
    [TestClass]
    public class UnitTestCfgXml
    {
        [TestMethod]
        public void TestConfigMappingByCode()
        {
            var config = new NHibernate.Cfg.Configuration();
            config.Configure();
            config.DataBaseIntegration(db =>
            {
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            });

            config.AddDeserializedMapping(InternalHelper.GetAllMapper(), "Models");

            var factory = config.BuildSessionFactory();

        }
    }
}
