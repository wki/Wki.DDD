﻿using FakeItEasy;
using System;
using NUnit.Framework;
using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;

namespace Wki.DDD.Tests.EventBus
{
    [TestFixture]
    public class HubTest
    {
        private IDispatcher dispatcher;
        private IHub hub;

        [SetUp]
        public void SetupHub()
        {
            dispatcher = A.Fake<IDispatcher>();
            hub = new Hub(dispatcher);
        }

        [Test]
        public void Hub_Initially_DoesNotCallDispatcher()
        {
            // Assert
            A.CallTo(() => dispatcher.Dispatch(A<IEvent>._))
             .MustNotHaveHappened();
        }

        [Test]
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
