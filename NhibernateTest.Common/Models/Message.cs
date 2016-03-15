using System.Collections.Generic;

namespace NhibernateTest
{
    public class Message : BaseEntity<int>
    {
        public virtual string Content
        { get; set; }

        public virtual MessageType Type
        { get; set; }

        public virtual List<File> Files
        { get; set; }

        public virtual Comment[] Comments
        { get; set; }
    }
}
