using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using NhibernateTest.Service;

namespace NhibernateTest
{
    public class UserService : IUserService
    {
        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> model;
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                model = session.Query<User>().ToList();
            }
            return model;
        }

        public User GetSingle(int id)
        {
            User model;
            using (var session = NHibernateUtility.SessionFactory.OpenSession())
            {
                model = session.Get<User>(id);
            }
            return model;
        }

        public void Insert(User model)
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

        public void Update(int id, User model)
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