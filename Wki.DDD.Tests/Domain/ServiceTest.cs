using Wki.DDD.Domain;
using Wki.DDD.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wki.DDD.Tests.Domain
{
    public class SomethingHappened : DomainEvent {}
    public class ErrorOccured : DomainEvent {}

    public class SampleService 
        : IService, ISubscribe<SomethingHappened>
    {
        public int NrEvents = 0;
        
        public void Handle(SomethingHappened somethingHappened)
        {
            NrEvents++;
        }
    }

    public class SecondService
        : IService, ISubscribe<SomethingHappened>, ISubscribe<ErrorOccured>
    {
        public int NrEvents = 0;
        
        public void Handle(SomethingHappened somethingHappened)
        {
            NrEvents++;
        }

        public void Handle(ErrorOccured errorOccured) { }
    }

    [TestClass]
    public class ServiceTest
    {
        [TestInitialize]
        public void SetupContainer()
        {
            
        }

        [TestMethod]
        public void SampleService_Initially_HasNotHandledEvents()
        {
            // Arrange
            var service = new SampleService();

            // Assert
            Assert.AreEqual(0, service.NrEvents);

        }
        //[TestMethod]
        //public void TestServiceInstantiation()
        //{
        //    var s = new SampleService();

        //    Assert.AreEqual(s.NrEvents, 0);
        //}

        //[TestMethod]
        //public void TestHandling()
        //{
        //    var s = new SampleService();

        //    s.Handle(new SomethingHappened());
        //    s.Handle(new SomethingHappened());

        //    Assert.AreEqual(s.NrEvents, 2);
        //}

        //[TestMethod]
        //public void TestEventRegistration()
        //{
        //    var s = new SampleService();

        //    DomainEvents.Register<SomethingHappened>(s.Handle);

        //    Assert.AreEqual(s.NrEvents, 0);
        //}

        //[TestMethod]
        //public void TestEventHandling()
        //{
        //    var s = new SampleService();

        //    DomainEvents.Register<SomethingHappened>(s.Handle);
        //    DomainEvents.Raise(new SomethingHappened());

        //    Assert.AreEqual(s.NrEvents, 1);
        //}

        //[TestMethod]
        //public void TestContainerEventHandling()
        //{
        //    var s = new SampleService();
        //    // var c = new UnityContainer();


        //    // windsor version. no .FromAllAssemblies() existing. difficult.

        //    var container = new WindsorContainer();

        //    container.Register(Classes
        //        .FromThisAssembly()
        //        // .BasedOn<IHandle<DomainEvent>>()
        //        .BasedOn<IService>()
        //        .WithService.AllInterfaces()
        //        .Configure(c => c.Named("EventHandler:" + c.Implementation.FullName))
        //    );

        //    printRegistrations(container);


        //    /* unity:
        //    c.RegisterTypes(
        //        AllClasses
        //            .FromLoadedAssemblies(false, false)
        //            .Where(t => typeof(IService).IsAssignableFrom(t)),
        //        //WithMappings.FromAllInterfaces, // .Where(t => t.Name.StartsWith("IHandle")),
        //        t => t.GetInterfaces().Where(x => x.Name.StartsWith("IHandle")),
        //        WithName.TypeName);

            

        //    Console.WriteLine("Container has {0} Registrations", c.Registrations.Count());
        //    foreach (ContainerRegistration item in c.Registrations)
        //    {
        //        //Console.WriteLine("Registered Type: {0} Name: {1} Mapped To: {2}", item.RegisteredType, item.Name, item.MappedToType);
        //        Console.WriteLine(item.GetMappingAsString());
        //    }
        //    */

        //    var resolved = container.ResolveAll<IHandle<SomethingHappened>>();
        //    // var resolved = c.ResolveAll<IHandle<DomainEvent>>();

        //    // Assert.AreEqual(2, resolved.Count());

        //    Assert.AreEqual("x", resolved.Select(t => t.ToString()).Aggregate<object>((current, next) => current + ", " + next));

        //    // DomainEvents.Raise(new SomethingHappened());

        //    // Assert.AreEqual(1, s.NrEvents);
        //}
    }
}
