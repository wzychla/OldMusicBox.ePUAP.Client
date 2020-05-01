using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldMusicBox.ePUAP.Client.Model.Pull;
using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Tests
{
    /// <summary>
    /// WS-Pull tests
    /// </summary>
    [TestClass]
    public class PullTests
    {
        [TestMethod]
        public void OczekujaceDokumentyRequest_Valid()
        {
            // arrange
            var podmiot         = "test";
            var nazwaSkrytki    = "default";
            var adresSkrytki    = "/test/default";

            var request = new OczekujaceDokumentyRequest()
            {
                Podmiot      = podmiot,
                NazwaSkrytki = nazwaSkrytki,
                AdresSkrytki = adresSkrytki
            };

            var requestFactory = new RequestFactory(new TestCertProvider().GetClientCertificate());

            // act
            string requestString = requestFactory.CreateRequest(request);

            // assert

            Assert.IsNotNull(requestString);
        }
    }
}
