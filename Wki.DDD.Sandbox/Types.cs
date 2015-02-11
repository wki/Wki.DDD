using Wki.DDD.EventBus;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wki.DDD.Domain;

namespace Wki.DDD.Sandbox
{
    public class BaseEvent : DomainEvent { };
    public class DerivedEvent : BaseEvent { };

    [TestClass]
    public class Types
    {
        [TestMethod]
        public void Object_BaseType_IsNull()
        {
            // Arrange
            var objectType = typeof(Object);

            // Assert
            Assert.IsNull(objectType.BaseType);
        }

        [TestMethod]
        public void BaseEvent_BaseType_IsDerivedEvent()
        {
            // Arrange
            var derivedType = typeof(DerivedEvent);
            var baseType = typeof(BaseEvent);

            // Assert
            Assert.AreEqual(derivedType.BaseType, baseType);
        }
    }
}
