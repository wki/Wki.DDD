using FakeItEasy;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;

namespace Wki.DDD.Tests.EventBus
{
    [TestClass]
    public class HubTest
    {
        private IDispatcher dispatcher;
        private IHub hub;

        [TestInitialize]
        public void SetupHub()
        {
            dispatcher = A.Fake<IDispatcher>();
            hub = new Hub(dispatcher);
        }

        [TestMethod]
        public void Hub_Initially_DoesNotCallDispatcher()
        {
            // Assert
            A.CallTo(() => dispatcher.Dispatch(A<IEvent>._))
             .MustNotHaveHappened();
        }

        [TestMethod]
        public void Hub_Publish_CallsDispatcher()
        {
            // Act
            hub.Publish(new ErrorOccured());

            // Assert
            A.CallTo(() => dispatcher.Dispatch(A<ErrorOccured>._))
             .MustHaveHappened();
        }
    }
}
