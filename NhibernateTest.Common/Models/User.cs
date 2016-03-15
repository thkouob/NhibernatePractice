
namespace NhibernateTest
{
    public class User : BaseEntity<int>
    {
        public virtual string Name
        { get; set; }

        public virtual string Account
        { get; set; }

        public virtual string Password
        { get; set; }

        public virtual string Salt
        { get; set; }

        public virtual bool IsSuperUser
        { get; set; }

        public virtual bool IsOnline
        { get; set; }

        public virtual string Email
        { get; set; }

        public virtual AddressInfo Address
        { get; set; }
    }
}