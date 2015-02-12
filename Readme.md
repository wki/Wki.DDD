# Wki.DDD

A very simplistic collection of base classes allowing to easily create
projects based on the tactical patterns of DDD.

One possible way to construct a full DDD application is to create a 
solution with these projects (Assuming "MyDomain" is your app's name):

 * "MyDomain" containing all Domain objects and Interfaces for all
   repositories to be implemented. If your domain is huge, of course
   many projects will improve your design.

 * "MyDomain.Init" contains an interface implementing a Dependency-Container
   implementation and handles initiating your domain's class registrations
   in the container of your choice.

 * "MyDomain.Repositories.Xxx" contains one implementation for your
   repositories for one storage engine. Inside your real application you
   only refer to one of your repositories. Registration will automatically
   happen by "MyDomain.Init" at application start time. Creating an
   In-Memory repository will help you code integration-tests.

 * "MyDomain.Web" might be an ASP.NET MVC app referring to your domain. 
   More applications are easily possible by referring the above projects.


## Initialization

Create a class that implements Wki.DDD.EventBus.IContainer:

    using Wki.DDD.EventBus;
    namespace StatisticsCollector
    {
      public class StatisticsUnityContainer: IContainer
	  {
	  ...
	  }
    }

Create a class for initializing your Container. For Unity the class
might consist of the following snippets:

    public static void Initialize(IUnityContainer unity)
    {
      var container = new StatisticsUnityContainer(unity);
      new Hub(container);
    
      unity
          .RegisterTypes(
              AllClasses.FromAssemblies(...)
                  .Where(t => typeof(IFactory).IsAssignableFrom(t)
                           || typeof(IRepository).IsAssignableFrom(t)
                           || typeof(IService).IsAssignableFrom(t)),
              WithMappings.FromMatchingInterface,
              WithName.Default
          )
	      
          .RegisterTypes(
              AllClasses.FromAssemblies(...)
                  .Where(t => t.GetInterfaces().Any(i => i.Name.StartsWith("ISubscribe"))),
              WithMappings.FromAllInterfaces,
              t => "EventHandler: " + t.FullName
          );
    }

Abstracting the initialization away allows simple reuse.

## Domain

Build your domain by structuring your DDD-specific classes into Subdomain-
like folders containing namespaces or individual projects. Just as jou like.

There are some specialities:

 * Entity and Aggregate classes may omit Domain Events by calling:

       void DoSomething()
	   {
        Publish(new SomethingHappened { ... });
	   }

 * Services (Implementors of IService) may register for Events

       public class XxxService : IService, ISubscribe<SomethingHappened>
	   {
	     void Handle(SomethingHappened @event)
	     {
	     ...
	     }
	   }
 
 * All constructors require all Interfaces they want to get instantiated
   objects at run time. Our Initialization will ensure the objects will
   be present when needed.

Events currently are dispatched immediately when calling Publish().
A more variable dispatching method is in planning to allow long-running
event processing to prevent blocking.
