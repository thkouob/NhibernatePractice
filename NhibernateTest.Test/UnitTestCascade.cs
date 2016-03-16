using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Bytecode;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace NhibernateTest.Test
{
    [TestClass]
    public class UnitTestCascade
    {
        private static NHibernate.ISessionFactory Initialize(Action<ModelMapper> action)
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
            action(mapper);
            var maps = mapper.CompileMappingForAllExplicitlyAddedEntities();
            config.AddDeserializedMapping(maps, "Models");

            return config.BuildSessionFactory();
        }

        [TestMethod]
        public void Test_ManyToOne_General()
        {
            //什麼都設定
            var factory = Initialize(mapper =>
            {
                mapper.Class<Message>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.Bag(b => b.Comments,
                        b => { b.Key(k => k.Column("MessageId")); },
                        b => { b.OneToMany(); });
                });

                mapper.Class<Comment>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.ManyToOne(o => o.Message,
                                o => { o.Column("MessageId"); });
                });
            });

            //共四條Sql
            using (var session = factory.OpenSession())
            {
                var message = new Message() { Content = "Message1" };
                session.Save(message);

                var comment1 = new Comment() { Message = message, Content = "Comment1" };
                session.Save(comment1); //必需要手動加入Session

                var comment2 = new Comment() { Message = message, Content = "Comment2" };
                session.Save(comment2); //這樣已經有入一次
                message.Comments.Add(comment2); //這樣又Update一次，有一次Sql

                session.Flush();
            }
        }

        [TestMethod]
        public void Test_ManyToOne_Insert()
        {
            //設定ManyToOne的Insert(false)
            var factory = Initialize(mapper =>
            {
                mapper.Class<Message>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.Bag(b => b.Comments,
                        b => { b.Key(k => k.Column("MessageId")); },
                        b => { b.OneToMany(); });
                });

                mapper.Class<Comment>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.ManyToOne(o => o.Message,
                                o =>
                                {
                                    o.Column("MessageId");
                                    o.Insert(false); /*停用Insert*/
                                });
                });
            });

            //四條Sql
            using (var session = factory.OpenSession())
            {
                var message = new Message() { Content = "Message1" };
                session.Save(message);

                var comment1 = new Comment() { Message = message, Content = "Comment1" }; //Message不會寫入,會變成孤兒
                session.Save(comment1);

                var comment2 = new Comment() { Content = "Comment2" };
                session.Save(comment2);
                message.Comments.Add(comment2); //當設定Insert(false)必需要這樣寫
                //Comment2會下二條Sql 一條Insert，一條Update

                session.Flush();
            }
        }

        [TestMethod]
        public void Test_Collection_Inverse()
        {
            //設定Collection的Inverse(true)
            var factory = Initialize(mapper =>
            {
                mapper.Class<Message>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.Bag(b => b.Comments,
                        b =>
                        {
                            b.Key(k => k.Column("MessageId"));
                            b.Inverse(true); /*啟用Inverse*/
                        },
                        b => { b.OneToMany(); });
                });

                mapper.Class<Comment>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.ManyToOne(o => o.Message,
                                o => { o.Column("MessageId"); });
                });
            });

            //三條Sql
            using (var session = factory.OpenSession())
            {
                var message = new Message() { Content = "Message1" };
                session.Save(message);

                var comment1 = new Comment() { Message = message, Content = "Comment1" };
                session.Save(comment1);

                var comment2 = new Comment() { Content = "Comment2" };
                session.Save(comment2);
                message.Comments.Add(comment2);
                /*因為Inverse(true)，不會產生Update的Sql(Comment不會更新messageId)
                  Insert時也沒有將message屬性指定對象(EX:167 Line)，comment2無法與Message關聯對應
                */
                session.Flush();
            }
        }

        [TestMethod]
        public void Test_Collection_Cascade_Insert()
        {
            //設定Collection的Cascade(Cascade.All)，全部連動(當操作Insert的時候)
            var factory = Initialize(mapper =>
            {
                mapper.Class<Message>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.Bag(b => b.Comments,
                        b =>
                        {
                            b.Key(k => k.Column("MessageId"));
                            b.Cascade(Cascade.All); /*啟用Cascade*/
                        },
                        b => { b.OneToMany(); });
                });

                mapper.Class<Comment>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(p => p.Content);

                    m.ManyToOne(o => o.Message,
                                o => { o.Column("MessageId"); });
                });
            });

            //四條Sql
            using (var session = factory.OpenSession())
            {
                var message = new Message() { Content = "Message1" };
                session.Save(message);

                var comment1 = new Comment() { Message = message, Content = "Comment1" };
                session.Save(comment1); //會由message找關連，將項目加入session

                var comment2 = new Comment() { Content = "Comment2" };
                //session.Save(comment2); //因為加了Cascade，可以不用讓子項目呼叫session.Save(Entity);
                message.Comments.Add(comment2); //會由message找關連，將項目加入session，會產生二個Sql Insert與Update

                session.Flush();
            }
        }

        [TestMethod]
        public void Test_Collection_Cascade_Update()
        {
            //設定Collection的Cascade(Cascade.All)，全部連動(當操作Update的時候)
            var factory = Initialize(mapper =>
            {
                mapper.Class<Message>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(x => x.Content);

                    //可不設定屬性，而用index的Column參db欄位
                    //m.Property(x => x.Sort);

                    //使用 idx(預設欄位名稱)欄立記錄順序
                    m.List(b => b.Comments,
                           b =>
                           {
                               b.Key(k => k.Column("MessageId"));
                               b.Cascade(Cascade.All); /*啟用Cascade*/
                               //http://notherdev.blogspot.tw/2012/02/mapping-by-code-list-array-idbag.html
                               b.Index(idx =>
                               {
                                   //起始值預設為0
                                   idx.Base(1);
                                   idx.Column("sort");
                               });
                           },
                           b => { b.OneToMany(); });
                });

                mapper.Class<Comment>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(x => x.Content);

                    m.ManyToOne(o => o.Message,
                                o =>
                                {
                                    o.Column("MessageId");
                                    o.Update(false);
                                });
                });
            });

            using (var session = factory.OpenSession())
            {
                var message = new Message() { Content = "Message1" };
                session.Save(message);

                message.Comments.Add(new Comment() { Message = message, Content = "Comment1" });

                message.Comments.Add(new Comment() { Message = message, Content = "Comment2" });
                session.Flush();
            }

            Console.WriteLine("======================");
            using (var session = factory.OpenSession())
            {
                var message = session.Get<Message>(1);

                message.Comments.Insert(0, new Comment() { Message = message, Content = "Comment3" });

                session.Flush();
            }

            Console.WriteLine("======================");
            using (var session = factory.OpenSession())
            {
                var message = session.Get<Message>(1);
                //Check List Sort
                foreach (var item in message.Comments)
                {
                    Console.WriteLine(item.Content);
                }
                session.Flush();
            }
        }

        [TestMethod]
        public void Test_Collection_Cascade_Delete()
        {
            //設定Collection的Cascade(Cascade.All)，全部連動(當操作Delete的時候)
            var factory = Initialize(mapper =>
            {
                mapper.Class<Message>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(x => x.Content);

                    m.List(b => b.Comments,
                           b => { b.Key(k => k.Column("MessageId")); b.Cascade(Cascade.All); /*啟用Cascade*/ },
                           b => { b.OneToMany(); });
                });

                mapper.Class<Comment>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(x => x.Content);

                    m.ManyToOne(o => o.Message,
                                o =>
                                {
                                    o.Column("MessageId");
                                    o.Update(false);
                                });
                });
            });

            using (var session = factory.OpenSession())
            {
                var message = new Message() { Content = "Message1" };
                session.Save(message);

                message.Comments.Add(new Comment() { Message = message, Content = "Comment1" });

                message.Comments.Add(new Comment() { Message = message, Content = "Comment2" });
                session.Flush();
            }

            //有6條Sql
            Console.WriteLine("======================");
            using (var session = factory.OpenSession())
            {
                //有多少子項目多少Sql
                session.Delete(session.Get<Message>(1)); //一定要Persistent

                session.Flush();
            }
        }

        [TestMethod]
        public void Test_Collection_OnDelete_Cascade()
        {
            //設定產生Database的Cascade
            var factory = Initialize(mapper =>
            {
                mapper.Class<Message>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(x => x.Content);

                    m.Bag(b => b.Comments,
                          b =>
                          {
                              b.Key(k =>
                              {
                                  k.Column("MessageId");
                                  k.OnDelete(OnDeleteAction.Cascade);
                              });
                              b.Inverse(true);
                          }, b => { b.OneToMany(); });
                });

                mapper.Class<Comment>(m =>
                {
                    m.Id(x => x.Id, x => { x.Generator(Generators.Identity); });

                    m.Property(x => x.Content);

                    m.ManyToOne(o => o.Message,
                                o =>
                                {
                                    o.Column("MessageId");
                                    o.Update(false);
                                });
                });

            });

            using (var session = factory.OpenSession())
            {
                var message = new Message() { Content = "Message1" };
                session.Save(message);

                session.Save(new Comment() { Message = message, Content = "Comment1" });
                session.Save(new Comment() { Message = message, Content = "Comment2" });
                session.Flush();
            }

            Console.WriteLine("======================");
            using (var session = factory.OpenSession())
            {
                //只有產生1條Sql
                session.Delete(new Message() { Id = 1 }); // 可以使用Transient

                session.Flush();
            }
        }
    }
}
