using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;

namespace NhibernateTest
{
    public class ProductService
    {
        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> model;
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                model = session.Query<Product>().ToList();
            }
            return model;
        }

        public Product GetSingle(int id)
        {
            Product model;
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                model = session.Get<Product>(id);
            }
            return model;
        }

        public void Insert(Product model)
        {
            model.CreateTime = DateTime.Now;

            //如同時處理好幾張表，可加入交易避免例外發生時產生髒資料
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            using (var trans = session.BeginTransaction())
            {
                try
                {
                    session.Save(model);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // Log...
                    trans.Rollback();
                }
            }
        }

        public void Update(int id, Product model)
        {
             var soucre = GetSingle(id);

             if (soucre != null)
             {
                 soucre.Name = model.Name;

                 //如同時處理好幾張表，可加入交易避免例外發生時產生髒資料
                 using (var session = NHibernateUtility.SessionFactory.OpenSession())
                 using (var trans = session.BeginTransaction())
                 {
                     try
                     {
                         session.SaveOrUpdate(soucre);
                         trans.Commit();
                     }
                     catch (Exception ex)
                     {
                         // Log...
                         trans.Rollback();
                     }
                 }
             }
        }

        public void Delete(int id)
        {
            var model = GetSingle(id);

            if (model != null)
            {
                using (var session = NHibernateUtility.SessionFactory.OpenSession())
                using (var trans = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete(model);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Log...
                        trans.Rollback();
                    }
                }
            }
        }
    }
}