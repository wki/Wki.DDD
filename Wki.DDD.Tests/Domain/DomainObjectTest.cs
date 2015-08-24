﻿using NUnit.Framework;
using Wki.DDD.Domain;

namespace Wki.DDD.Tests.Domain
{
    public class BrokenDomainObject : DomainObject
    {
        protected override void Validate()
        {
            AddBrokenRule(new BusinessRule("nonsense"));
        }
    }

    [TestFixture]
    public class DomainObjectTest
    {
        [Test]
        public void DomainObject_Initially_HasNoBrokenRules()
        {
            // Arrange
            var domainObject = new DomainObject();

            // Assert
            Assert.AreEqual(0, domainObject.GetBrokenRules().Count);
        }

        [Test]
        public void DomainObject_WithoutBrokenRule_DoesNotThrowException()
        {
            // Arrange
            var domainObject = new DomainObject();
            
            // Act
            domainObject.ThrowExceptionIfInvalid();

            // Assert
            Assert.IsTrue(true);
        }

        [Test]
        public void BrokenDomainObject_Initially_HasABrokenRule()
        {
            // Arrange
            var brokenDomainObject = new BrokenDomainObject();

            // Assert
            Assert.AreEqual(1, brokenDomainObject.GetBrokenRules().Count);
        }

        [Test]
        [ExpectedException(typeof(ObjectIsInvalidException))]
        public void BrokenDomainObject_BrokenRule_ThrowsException()
        {
            // Arrange
            var brokenDomainObject = new BrokenDomainObject();
            
            // Act
            brokenDomainObject.ThrowExceptionIfInvalid();

            // Assert
            Assert.IsTrue(false);
        }
    }
}
