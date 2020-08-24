using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldMusicBox.ePUAP.Client.Model.Dokumenty;
using OldMusicBox.ePUAP.Client.Model.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Tests
{
    /// <summary>
    /// Testy modelu dokumentu
    /// </summary>
    [TestClass]
    public class DokumentTests
    {
        [TestMethod]
        public void SimpleWnioDokument()
        {
            // arrange
            var dokument = new Dokument();

            dokument.Opis.Data.Czas.Wartosc = "2020-01-01";

            dokument.Dane.Naglowek.Nazwa.Wartosc = "test.pdf";
            dokument.Dane.Data.Czas.Wartosc      = "2020-01-01";

            dokument.Tresc.Zalaczniki = new Zalacznik[]
            {
                new Zalacznik()
                {
                    Format         = "application/pdf",
                    DaneZalacznika = new DaneZalacznika()
                    {                        
                        Zawartosc = File.ReadAllBytes("test.pdf")
                    }
                }
            };

            // act
            var xml = dokument.ToXmlDocument();

            // assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.OuterXml.Length > 0);
        }

    }
}
