
namespace NhibernateTest
{
    public class Product : BaseEntity<int>
    {
        public virtual ProductCategoryEnum Category
        { get; set; }

        public virtual string Name
        { get; set; }

        public virtual string Description
        { get; set; }

        public virtual int Sort
        { get; set; }

        public virtual string ProductDetail
        { get; set; }
    }
}
