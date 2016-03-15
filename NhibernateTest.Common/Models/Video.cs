
namespace NhibernateTest
{
    public class Video : NhibernateTest.File
    {

        public virtual int Length
        {
            get;
            set;
        }
    }
}
