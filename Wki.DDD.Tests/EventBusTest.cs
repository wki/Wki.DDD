using FakeItEasy;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;
using Wki.DDD.Tests.Services;
using Wki.DDD.Domain;

namespace Wki.DDD.Tests
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
    }




    //interface XEvent {} // will not lead to calling an event handler
    //interface YEvent : IEvent {}
    //public class SomethingHappened    : IEvent, XEvent {}
    //public class GoodThingHappened    : SomethingHappened, YEvent {}
    //public class BadThingHappened     : SomethingHappened {}
    //public class AnotherThingHappened : IEvent {}

    //public class ServiceBase : IPublish
    //{
    //    public int nrEventsHandled;

    //    public ServiceBase()
    //    {
    //        nrEventsHandled = 0;
    //    }
    //}

    /*
    interface IAService {}
    interface IBService {}
    interface ICService {}

    public class AService : ServiceBase, IAService, ISubscribe<GoodThingHappened>
    {
        public AService()
        {
        }

        public void Handle(GoodThingHappened @event)
        {
            nrEventsHandled++;
        }
    }

    public class BService : ServiceBase, IBService, ISubscribe<BadThingHappened>
    {
        public BService()
        {
        }

        public void Handle(BadThingHappened @event)
        {
            nrEventsHandled++;
        }
    }

    public class CService : ServiceBase, ICService, ISubscribe<IEvent>
    {
        public CService()
        {
        }

        public void Handle(IEvent @event)
        {
            nrEventsHandled++;
        }
    }

    
    [TestClass]
    public class EventBusTest
    {
        private TestContainer container { get; set; } 
        
        private void printRegistrations()
        {
            //Console.WriteLine("Registrations:");
            //foreach (var handler in container.Kernel.GetAssignableHandlers(typeof(object)))
            //{
            //    foreach (var service_name in handler.ComponentModel.Services)
            //    {
            //        Console.WriteLine("Service: {0} Name: {1}, Implemented By {2}",
            //            service_name,
            //            handler.ComponentModel.Name,
            //            handler.ComponentModel.Implementation);
            //    }
            //}
        }

        [TestInitialize]
        public void InitializeContainer()
        {
            container = new TestContainer();
        }

        [TestMethod]
        public void Type_Interfaces()
        {
            // arrange
            var goodThingHappened = new GoodThingHappened();
 
            // act
            var type_names = goodThingHappened
                .GetType()
                .GetInterfaces()
                .OrderBy(x => x.Name)
                .Select(x => x.Name);

            // assert
            Assert.AreEqual(
                String.Join("+", type_names),
                "IEvent+XEvent+YEvent", 
                "We get all inherited interfaces"
            );
        }

        [TestMethod]
        public void Interface_Subclasses()
        {
            Assert.IsTrue(
                typeof(IEvent).IsAssignableFrom(typeof(SomethingHappened)),
                "a derived class or interface can get assigned to a base interface"
            );
        }

        // order of execution unknown. Therefore will fail
        //[TestMethod]
        //public void Hub_Uninitialized()
        //{
        //    Assert.IsNull(Hub.Current, "Current unset");
        //}

        [TestMethod]
        public void Hub_Instantiation()
        {
            // arrange
            var bus = new Hub(container);

            // assert
            Assert.AreEqual(Hub.Current, bus, "Current reflects new bus");
        }

        [TestMethod]
        public void Publish_Unsubscribed_Event()
        {
            // arrange
            var bus = new Hub(container);

            // act
            bus.Publish(new GoodThingHappened());
            
            // assert
            Assert.IsTrue(true, "Publishing an unsubscribed Event");
        }

        [TestMethod]
        public void Publish_Subscribed_Event()
        {
            // arrange
            var aService = new AService();
            container.RegisterInstance<ISubscribe<GoodThingHappened>>(aService);
            container.PrintRegistrations();
            var bus = new Hub(container);

            // act
            bus.Publish(new GoodThingHappened());

            // assert
            Assert.AreEqual(1, aService.nrEventsHandled, "subscribed event handled");
        }

        [TestMethod]
        public void Publish_Multiple_Subscribed_Events()
        {
            // arrange
            var aService = new AService();
            var bService = new BService();
            container.RegisterInstance<ISubscribe<GoodThingHappened>>(aService);
            container.RegisterInstance<ISubscribe<BadThingHappened>>(bService);
            container. PrintRegistrations();
            var bus = new Hub(container);

            // act
            bus.Publish(new GoodThingHappened());
            bus.Publish(new BadThingHappened());
            bus.Publish(new BadThingHappened());

            // assert
            Assert.AreEqual(1, aService.nrEventsHandled, "subscribed event A handled");
            Assert.AreEqual(2, bService.nrEventsHandled, "subscribed event B handled");
        }

        [TestMethod]
        public void Publish_Multiply_Subscribed_Event()
        {
            // arrange
            var a1Service = new AService();
            var a2Service = new AService();
            container.RegisterInstance<ISubscribe<GoodThingHappened>>(a1Service, "a1");
            container.RegisterInstance<ISubscribe<GoodThingHappened>>(a2Service, "a2");
            container.PrintRegistrations();
            var bus = new Hub(container);

            // act
            bus.Publish(new GoodThingHappened());
            bus.Publish(new GoodThingHappened());
            bus.Publish(new BadThingHappened());

            // assert
            Assert.AreEqual(2, a1Service.nrEventsHandled, "subscribed event handled by A1");
            Assert.AreEqual(2, a2Service.nrEventsHandled, "subscribed event handled by A2");
        }

        [TestMethod]
        public void Publish_Generic_Subscribed_Event()
        {
            // arrange
            container.RegisterAll<ServiceBase>();

            AService aService = (AService)container.Resolve<IAService>();
            BService bService = (BService)container.Resolve<IBService>();
            CService cService = (CService)container.Resolve<ICService>();

            printRegistrations();
            var bus = new Hub(container);

            // act
            bus.Publish(new GoodThingHappened()); // caucht by a+c
            bus.Publish(new BadThingHappened());  // caught by b+c
            bus.Publish(new BadThingHappened());  // caught by b+c

            // assert
            Assert.AreEqual(1, aService.nrEventsHandled, "subscribed event A handled");
            Assert.AreEqual(2, bService.nrEventsHandled, "subscribed event B handled");
            Assert.AreEqual(3, cService.nrEventsHandled, "subscribed event C handled");
        }
    }
    */
}
