using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NhibernateTest
{
    //http://www.dotblogs.com.tw/lastsecret/archive/2011/12/15/62132.aspx
    public class NHibernateUtility
    {
        public static Configuration Configuration { get; private set; }

        public static ISessionFactory SessionFactory { get; private set; }

        public void Initialize()
        {
            Configuration = this.Configure();

            SessionFactory = Configuration.BuildSessionFactory();
        }

        private Configuration Configure()
        {
            var configuration = new Configuration();

            // 資料庫設定
            // 這裡的東西可以改用xml的方式設定，增加修改的彈性
            configuration.DataBaseIntegration(c =>
            {
                // 資料庫選用 SQLite
                c.Dialect<MsSql2012Dialect>();

                // 取用 .config 中的 "MyTestDB" 連線字串
                c.ConnectionStringName = "NhibernateMssqlDb";

                // Schema 變更時的處置方式
                c.SchemaAction = SchemaAutoAction.Create;

                // 交易隔離等級
                c.IsolationLevel = IsolationLevel.ReadCommitted;
            });

            // 取得Mapping
            // 取代舊有的 *.hbm.xml
            var mapping = GetMappings();
            //加入Mapping
            configuration.AddMapping(mapping);

            return configuration;
        }

        private HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();

            //加入Mapping
            //如果是多個 Type 可用 AddMappings
            mapper.AddMapping(typeof(ProductEntityMap));
            mapper.AddMapping(typeof(FileEntityMap));
            mapper.AddMapping(typeof(UserEntityMap));
            mapper.AddMapping(typeof(AdminEntityMap));
            mapper.AddMapping(typeof(ImageEntityMap));
            mapper.AddMapping(typeof(VideoEntityMap));
            mapper.AddMapping(typeof(CommentEntityMap));
            mapper.AddMapping(typeof(MessageEntityMap));

            HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }
    }
}