
namespace NhibernateTest
{
    public class Image : NhibernateTest.File
    {

        public virtual int Width
        {
            get;
            set;
        }

        public virtual int Height
        {
            get;
            set;
        }
    }
}
