using FakeItEasy;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;
using Wki.DDD.Tests.Services;
using Wki.DDD.Domain;

namespace Wki.DDD.Tests.EventBus
{
    [TestClass]
    public class EventBusTest
    {
        CatchAllService catchAllService;
        ErrorService errorService;
        SomethingService somethingService;
        SomethingErrorService somethingErrorService;
        
        IContainer container;
        IHub hub;

        [TestInitialize]
        public void SetupContainer()
        {
            catchAllService = new CatchAllService();
            errorService = new ErrorService();
            somethingService = new SomethingService();
            somethingErrorService = new SomethingErrorService();

            container = A.Fake<IContainer>();

            // TODO: also test for DomainEvent. Must get added in Hub also

            A.CallTo(() => container.ResolveAll<ISubscribe<IEvent>>())
                .Returns(new ISubscribe<IEvent>[] { catchAllService });

            A.CallTo(() => container.ResolveAll<ISubscribe<ErrorOccured>>())
                .Returns(new ISubscribe<ErrorOccured>[] { errorService, somethingErrorService });

            A.CallTo(() => container.ResolveAll<ISubscribe<SomethingHappened>>())
                .Returns(new ISubscribe<SomethingHappened>[] { somethingService, somethingErrorService });

            hub = new Hub(container);
        }

        [TestMethod]
        public void Hub_NoEventsSent_NothingIsCaptured()
        {
            // Assert
            Assert.AreEqual(0, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(0, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(0, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void Hub_NotCapturedSent_OnlyCatchAllCaptures()
        {
            // Act
            hub.Publish(new NotCaptured());

            // Assert
            Assert.AreEqual(1, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(0, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(0, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void Hub_ErrorSent_ErrorAndCatchAllCaptures()
        {
            // Act
            hub.Publish(new ErrorOccured());

            // Assert
            Assert.AreEqual(1, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(100, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(100, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void Hub_ErrorSentTwice_ErrorAndCatchAllCaptures()
        {
            // Act
            hub.Publish(new ErrorOccured());
            hub.Publish(new ErrorOccured());

            // Assert
            Assert.AreEqual(2, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(200, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(200, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void Hub_MiscMessagesSent_ErrorAndCatchAllCaptures()
        {
            // Act
            hub.Publish(new ErrorOccured());
            hub.Publish(new SomethingHappened());
            hub.Publish(new SomethingHappened());

            // Assert
            Assert.AreEqual(3, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(100, errorService.nrEventsHandled, "error");
            Assert.AreEqual(2, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(102, somethingErrorService.nrEventsHandled, "somethingError");
        }
    }
}
