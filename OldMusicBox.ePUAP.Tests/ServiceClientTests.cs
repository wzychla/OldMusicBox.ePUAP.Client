using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldMusicBox.ePUAP.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Tests
{
    /// <summary>
    /// Basic service client tests
    /// </summary>
    [TestClass]
    public class ServiceClientTests
    {
        [TestMethod]
        public void ConstructorThrowsIfEmptyUri()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    var cert = new TestCertProvider().GetClientCertificate();
                    var service = new ServiceClient(null, cert);
                });
        }

        [TestMethod]
        public void ConstructorThrowsIfEmptyCert()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    var service = new ServiceClient("http://foo.bar", null);
                });
        }
    }
}
