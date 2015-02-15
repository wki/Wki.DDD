using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wki.DDD.Specifications;
using Wki.DDD.Domain;
using System.Linq.Expressions;

namespace Wki.DDD.Tests.Specifications
{
    public class TestEntity
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }

    [TestClass]
    public class SpecificationTest
    {
        private TestEntity entity;

        [TestInitialize]
        public void Initialize()
        {
            entity = new TestEntity { Name = "Unnamed", Number = 42 };
        }

        #region failing cases
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Specification_WithoutPredicate_ThrowsException()
        {
            // Arrange
            var s = new Specification<TestEntity>(null);

            // Act
            s.IsSatisfiedBy(null);

            // Assert
            Assert.IsNull(s); // will fail if we do not throw
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Specification_SatisfiedByNull_ThrowsException()
        {
            // Arrange
            var s = new Specification<TestEntity>(e => e.Name == "Hello");

            // Act
            s.IsSatisfiedBy(null);

            // Assert
            Assert.IsNull(s); // will fail if we do not throw.
        }
        #endregion

        #region simple conditions
        [TestMethod]
        public void Specification_FalseCondition_ReturnsFalse()
        {
            // Arrange
            var s = new Specification<TestEntity>(e => e.Name == "Hello");

            // Act
            var result = s.IsSatisfiedBy(entity);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Specification_TrueCondition_ReturnsTrue()
        {
            // Arrange
            var s = new Specification<TestEntity>(e => e.Name == "Unnamed");

            // Act
            var result = s.IsSatisfiedBy(entity);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion

        #region AND
        [TestMethod]
        public void Specification_TrueAndFalse_ReturnsFalse()
        {
            // Arrange
            var s = new Specification<TestEntity>(e => e.Name == "Unnamed")
                .And(new Specification<TestEntity>(e => e.Number == 13));
            
            // Act
            var result = s.IsSatisfiedBy(entity);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Specification_TrueAndTrue_ReturnsTrue()
        {
            // Arrange
            var s = new Specification<TestEntity>(e => e.Name == "Unnamed")
                .And(e => e.Number == 42);

            // Act
            var result = s.IsSatisfiedBy(entity);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion
    }
}
