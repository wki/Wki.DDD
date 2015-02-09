using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wki.DDD.Sandbox
{
    [TestClass]
    public class Contravariance
    {
        [TestMethod]
        public void Covariance_Compiles_OK()
        {
            Action<string> log;

            log = Send;
            log = Save;  // compiles OK. contravariance!

            Assert.IsTrue(true);
        }


        public void Send(string message) { }
        public void Save(object o) { }
    }
}
