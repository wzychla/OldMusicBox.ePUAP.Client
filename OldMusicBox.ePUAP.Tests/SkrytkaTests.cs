using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldMusicBox.ePUAP.Client.Request;
using OldMusicBox.ePUAP.Client.Skrytka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Tests
{
    [TestClass]
    public class SkrytkaTests
    {
        [TestMethod]
        public void NadajRequest_Valid()
        {
            // arrange
            var adresSkrytki    = "/foo/default";
            var adresOdpowiedzi = "/bar/default";
            var document        = new NadajRequest.DocumentType
            {
                NazwaPliku = "plik.txt",
                TypPliku   = "text/plain",
                Zawartosc  = Encoding.UTF8.GetBytes("abc")
            };

            NadajRequest request = new NadajRequest()
            {
                AdresOdpowiedzi = adresOdpowiedzi,
                AdresSkrytki    = adresSkrytki,
                Document        = document
            };

            var requestFactory = new RequestFactory(new TestCertProvider().GetClientCertificate());

            // act
            string requestString = requestFactory.CreateRequest(request);

            // assert

            Assert.IsNotNull(requestString);
        }
    }
}
