using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;
using Wki.DDD.Tests.Services;

namespace Wki.DDD.Tests.EventBus
{
    [TestClass]
    public class ImmediateDispatcherTest
    {
        CatchAllService catchAllService;
        ErrorService errorService;
        SomethingService somethingService;
        SomethingErrorService somethingErrorService;
        
        IContainer container;
        ImmediateDispatcher dispatcher;

        [TestInitialize]
        public void SetupDispatcher()
        {
            catchAllService = new CatchAllService();
            errorService = new ErrorService();
            somethingService = new SomethingService();
            somethingErrorService = new SomethingErrorService();

            container = A.Fake<IContainer>();

            A.CallTo(() => container.ResolveAll<ISubscribe<IEvent>>())
                .Returns(new ISubscribe<IEvent>[] { catchAllService });

            A.CallTo(() => container.ResolveAll<ISubscribe<ErrorOccured>>())
                .Returns(new ISubscribe<ErrorOccured>[] { errorService, somethingErrorService });

            A.CallTo(() => container.ResolveAll<ISubscribe<SomethingHappened>>())
                .Returns(new ISubscribe<SomethingHappened>[] { somethingService, somethingErrorService });

            dispatcher = new ImmediateDispatcher(container);
        }

        [TestMethod]
        public void ImmediateDispatcher_NoEventsSent_NothingIsCaptured()
        {
            // Assert
            Assert.AreEqual(0, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(0, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(0, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void ImmediateDispatcher_NotCapturedSent_OnlyCatchAllCaptures()
        {
            // Act
            dispatcher.Dispatch(new NotCaptured());

            // Assert
            Assert.AreEqual(1, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(0, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(0, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void ImmediateDispatcher_ErrorSent_ErrorAndCatchAllCaptures()
        {
            // Act
            dispatcher.Dispatch(new ErrorOccured());

            // Assert
            Assert.AreEqual(1, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(100, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(100, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void ImmediateDispatcher_ErrorSentTwice_ErrorAndCatchAllCaptures()
        {
            // Act
            dispatcher.Dispatch(new ErrorOccured());
            dispatcher.Dispatch(new ErrorOccured());

            // Assert
            Assert.AreEqual(2, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(200, errorService.nrEventsHandled, "error");
            Assert.AreEqual(0, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(200, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void ImmediateDispatcher_MiscMessagesSent_ErrorAndCatchAllCaptures()
        {
            // Act
            dispatcher.Dispatch(new ErrorOccured());
            dispatcher.Dispatch(new SomethingHappened());
            dispatcher.Dispatch(new SomethingHappened());

            // Assert
            Assert.AreEqual(3, catchAllService.nrEventsHandled, "catchAll");
            Assert.AreEqual(100, errorService.nrEventsHandled, "error");
            Assert.AreEqual(2, somethingService.nrEventsHandled, "something");
            Assert.AreEqual(102, somethingErrorService.nrEventsHandled, "somethingError");
        }

        [TestMethod]
        public void ImmediateDispatcher_1000xMiscMessagesSent_DispatchEventBenchmark()
        {
            // Arrange
            var errorOccured = new ErrorOccured();
            var somethingHappened = new SomethingHappened();

            // Act
            for (var i = 0; i < 1000; i++)
            {
                dispatcher.DispatchEvent(errorOccured);
                dispatcher.DispatchEvent(somethingHappened);
                dispatcher.DispatchEvent(somethingHappened);
            }

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ImmediateDispatcher_1000xMiscMessagesSent_DispatchBaseTypesBenchmark()
        {
            // Arrange
            var errorOccured = new ErrorOccured();
            var somethingHappened = new SomethingHappened();

            // Act
            for (var i = 0; i < 1000; i++)
            {
                dispatcher.DispatchBaseTypes(errorOccured);
                dispatcher.DispatchBaseTypes(somethingHappened);
                dispatcher.DispatchBaseTypes(somethingHappened);
            }

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ImmediateDispatcher_1000xMiscMessagesSent_DispatchBenchmark()
        {
            // Arrange
            var errorOccured = new ErrorOccured();
            var somethingHappened = new SomethingHappened();

            // Act
            for (var i = 0; i < 1000; i++)
            {
                dispatcher.Dispatch(errorOccured);
                dispatcher.Dispatch(somethingHappened);
                dispatcher.Dispatch(somethingHappened);
            }

            // Assert
            Assert.IsTrue(true);
        }
    }
}
