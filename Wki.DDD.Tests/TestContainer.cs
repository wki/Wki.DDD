using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wki.DDD.EventBus;

namespace Wki.DDD.Tests
{
    /// <summary>
    /// this container offers more than IContainer requires.
    /// We use the container during all use-cases for tests.
    /// </summary>
    public class TestContainer : IContainer
    {
        //public IUnityContainer container { get; set; }

        //public TestContainer()
        //{
        //    container = new UnityContainer();
        //}

        //public void RegisterInstance<T>(T instance, string name = null)
        //{
        //    container.RegisterInstance<T>(instance);
        //}

        //public void RegisterAll<T>()
        //{

        //    container.RegisterTypes(
        //        AllClasses.FromAssembliesInBasePath()
        //            .Where(t =>
        //                   {
        //                       // Console.WriteLine(String.Join(", ", t.GetInterfaces().Select(i => i.Name)));
        //                       return t.GetInterfaces().Any(i => i.Name.StartsWith("ISubscribe"));
        //                   })
        //                ,
        //            WithMappings.FromAllInterfaces,
        //            (Type t) => "EventHandler: " + t.FullName
        //        );
        //}

        //public T Resolve<T>()
        //{
        //    return container.Resolve<T>();
        //}

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
            //return container.ResolveAll<T>();
        }

        //// for debugging purposes
        //public void PrintRegistrations()
        //{
        //    // to be defined.
        //}
    }
}
