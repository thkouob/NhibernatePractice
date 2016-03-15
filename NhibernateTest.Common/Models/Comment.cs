
namespace NhibernateTest
{
    public class Comment : BaseEntity<int>
    {

        public virtual string Content
        { get; set; }

        public virtual Message Message
        { get; set; }
    }
}
