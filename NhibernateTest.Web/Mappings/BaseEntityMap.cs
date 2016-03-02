using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhibernateTest
{
    public abstract class BaseEntityMap<TId, TEntity> : ClassMapping<TEntity>
        where TEntity : BaseEntity<TId>
    {
        public BaseEntityMap()
        {
            this.Id(x => x.Id, map => map.Generator(Generators.Identity));
            this.Property(x => x.Creator, map =>
            {
                map.Length(200);
                map.Access(Accessor.ReadOnly);
            });
            this.Property(x => x.CreateTime, map => map.NotNullable(true));
            this.Property(x => x.LasEditor, map => map.Formula("Creator + 'Test'"));
            this.Property(x => x.LastTime);
            this.Property(x => x.EntityStatus, map => map.Column("Valid"));

        }
    }

    public class ProductEntityMap : BaseEntityMap<int, Product>
    {
        public ProductEntityMap()
        {
            Schema("dbo");
            this.Property(x => x.Category, map =>
            {
                map.NotNullable(true);
                map.Type<NHibernate.Type.EnumType<ProductCategoryEnum>>();
            });
            this.Property(x => x.Name, map => map.NotNullable(true));
            this.Property(x => x.Description, map => map.NotNullable(true));
            this.Property(x => x.Sort, map => map.NotNullable(true));
            //this.Propertya(x => x.ProductDetail, map =>
            //{
            //    map.Type<NHibernate.Type.XmlDocType>();
            //    map.NotNullable(true);
            //});
            //this.Discriminator(x =>
            //{
            //    x.Column("");
            //    x.Type<NHibernate.Type.XmlDocType>();
            //});
            //this.DiscriminatorValue();
            this.Lazy(true);
        }
    }

    public class FileEntityMap : BaseEntityMap<int, File>
    {
        public FileEntityMap()
        {
            this.Discriminator(x => 
            { 
                x.Column("FileType"); 
                x.Type<NHibernate.Type.Int32Type>(); 
            });

            this.DiscriminatorValue(0);
            this.Property(x => x.Name, map => map.NotNullable(true));
            this.Property(x => x.DisplayName, map => map.NotNullable(true));
            this.Property(x => x.Category, map => map.NotNullable(true));
            this.Property(x => x.Sort, map => map.NotNullable(true));
        }
    }

    public class UserEntityMap : BaseEntityMap<int, User>
    {
        public UserEntityMap()
        {

            this.Property(x => x.Email);
            this.Property(x => x.Password);

            this.Component(x => x.Address, x =>
            {
                x.Property(p => p.Country, p => { p.Column("Country"); });
                x.Property(p => p.ZipCode, p => { p.Column("ZipCode"); });
                x.Property(p => p.Address, p => { p.Column("Address"); });
            });
        }
    }

    public class AdminEntityMap : JoinedSubclassMapping<Admin>
    {
        public AdminEntityMap()
        {
            this.Property(x => x.Phone);
            this.Map(x => x.Setting, x =>
            {
                x.Table("");
                x.Key(k => k.Column(""));
            },
            x =>
            {
                x.Element(e => e.Column("`Key`"));
            },
            x =>
            {
                x.Element(e => e.Column("`Value`"));
            });
        }
    }

    public class ImageEntityMap : SubclassMapping<Image>
    {
        public ImageEntityMap()
        {
            this.DiscriminatorValue(1);
            this.Property(x => x.Width);
            this.Property(x => x.Height);
        }
    }

    public class VideoEntityMap : SubclassMapping<Video>
    {
        public VideoEntityMap()
        {
            this.DiscriminatorValue(2);
            this.Property(x => x.Length);
        }
    }

    public class MessageEntityMap : BaseEntityMap<int, Message>
    {
        public MessageEntityMap()
        {
            this.Property(x => x.Content, x => x.Length(2000));
            this.Property(x => x.Type, x => x.Type<NHibernate.Type.EnumType<MessageType>>());
        }
    }

    public class CommentEntityMap : BaseEntityMap<int, Comment>
    {
        public CommentEntityMap()
        {
            this.Property(x => x.Content, x => x.Length(2000));
        }
    }
}