﻿using System;
using System.Linq;
using Wki.DDD.EventBus;
using NUnit.Framework;
using Wki.DDD.Domain;

namespace Wki.DDD.Tests.Domain
{
    public interface INotIEvent {}
    public interface IIsIEvent : IEvent {}

    public class LoggingEvent : DomainEvent, INotIEvent {}
    public class LoggingXEvent : LoggingEvent, IIsIEvent {}
    public class NonLoggingEvent : DomainEvent {}

    [TestFixture]
    public class DomainEventTest
    {
        [Test]
        public void DomainEvent_GetAllBaseTypes_ReturnsIEvent()
        {
            // Arrange
            var e = new DomainEvent();

            // Assert
            Assert.AreEqual("IEvent", GetAllTypes(e));
        }

        [Test]
        public void LoggingEvent_GetAllBaseTypes_ReturnsDomainEventIEvent()
        {
            // Arrange
            var e = new LoggingEvent();

            // Assert
            Assert.AreEqual("DomainEvent, IEvent", GetAllTypes(e));
        }

        [Test]
        public void NonLoggingEvent_GetAllBaseTypes_ReturnsDomainEventIEvent()
        {
            // Arrange
            var e = new NonLoggingEvent();

            // Assert
            Assert.AreEqual("DomainEvent, IEvent", GetAllTypes(e));
        }

        [Test]
        public void LoggingXEvent_GetAllBaseTypes_ReturnsMany()
        {
            // Arrange
            var e = new LoggingXEvent();

            // Assert
            Assert.AreEqual("DomainEvent, IEvent, IIsIEvent, LoggingEvent", GetAllTypes(e));
        }

        private string GetAllTypes(DomainEvent e)
        {
            return String.Join(", ", 
                e.GetAllBaseTypes()
                 .Select(t => t.Name)
                 .OrderBy(s => s)
            );
        }
    }
}
