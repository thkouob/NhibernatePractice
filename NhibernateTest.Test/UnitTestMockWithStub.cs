using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NhibernateTest.Service;
using Moq;

namespace NhibernateTest.Test
{
    [TestClass]
    public class UnitTestMockWithStub
    {
        [TestInitialize]
        public void TestInit()
        {
            //之後可抽換實作
            ServiceFactory.MessageService = new MessageServiceStub();

            var mockService = new Mock<ICommentService>();
            mockService.Setup(x => x.Get(It.IsAny<int>()))
                       .Returns(new Comment() { Content = "Hi,Leo" });
            ServiceFactory.CommentService = mockService.Object;
        }

        [TestMethod]
        public void TestStubMessage()
        {
            var message = ServiceFactory.MessageService.Get(1);
            Assert.AreEqual(message.Content, "Message");
        }

        [TestMethod]
        public void TestMockMessage()
        {
            var comment = ServiceFactory.CommentService.Get(1);
            Assert.AreEqual(comment.Content, "Hi,Leo");
        }

        [TestMethod]
        public void TestStubMessage_GetByUser()
        {
            var message = ServiceFactory.MessageService.GetByUser("leoli");
            Assert.AreEqual(message.Creator, "leoli");
        }
    }
}