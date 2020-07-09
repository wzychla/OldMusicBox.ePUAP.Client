using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldMusicBox.ePUAP.Client.Model.Doreczyciel;
using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Tests
{
    /// <summary>
    /// WS-Skrytka tests
    /// </summary>
    [TestClass]
    public class DoreczycielTests
    {
        [TestMethod]
        public void DoreczRequest_Valid()
        {
            // arrange
            var czyProbne              = false;
            var adresSkrytki           = "/foo/default";
            var adresOdpowiedzi        = "/bar/default";
            var podmiotNadawcy         = "test";
            var terminDoreczenia       = DateTime.Now;
            var identyfikatorDokumentu = "abc";
            var identyfikatorSprawy    = "abc";

            var document = new DocumentType
            {
                NazwaPliku = "plik.txt",
                TypPliku   = "text/plain",
                Zawartosc  = Encoding.UTF8.GetBytes("abc")
            };

            var request = new DoreczRequest()
            {
                CzyProbne              = czyProbne,
                AdresOdpowiedzi        = adresOdpowiedzi,
                AdresSkrytki           = adresSkrytki,
                IdentyfikatorPodmiotu  = podmiotNadawcy,
                TerminDoreczenia       = terminDoreczenia,
                IdentyfikatorDokumentu = identyfikatorDokumentu,
                IdentyfikatorSprawy    = identyfikatorSprawy,
                Document               = document
            };

            var requestFactory = new RequestFactory(new TestCertProvider().GetClientCertificate());

            // act
            string requestString = requestFactory.CreateRequest(request);

            // assert

            Assert.IsNotNull(requestString);
        }
    }
}
