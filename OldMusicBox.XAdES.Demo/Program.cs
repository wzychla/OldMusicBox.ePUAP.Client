using OldMusicBox.ePUAP.Client.Model.Dokumenty;
using OldMusicBox.ePUAP.Client.Model.XML;
using OldMusicBox.ePUAP.Client.XAdES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace OldMusicBox.XAdES.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // arrange
                var xml = File.ReadAllText("test.xml", Encoding.UTF8);
                var pdf = File.ReadAllBytes("test.pdf");
                var cert = Program.Certificate;
                if (cert == null)
                {
                    Console.WriteLine("No certificate selected.");
                    Environment.Exit(0);
                }

                var dokument = new Dokument();

                dokument.Opis.Data.Czas.Wartosc = DateTime.Now.ToString("o");

                dokument.Dane.Data.Czas.Wartosc = DateTime.Now.ToString("yyyy-MM-dd");

                dokument.Dane.Adresaci.Podmiot.Osoba          = new Osoba();
                dokument.Dane.Adresaci.Podmiot.Osoba.Nazwisko = "Kowalski";
                dokument.Dane.Adresaci.Podmiot.Osoba.Imie     = "Jan";

                dokument.Dane.Nadawcy.Podmiot.Instytucja                   = new Instytucja();
                dokument.Dane.Nadawcy.Podmiot.Instytucja.NazwaInstytucji   = "Urząd miasta Widliszki Wielkie";
                dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Miejscowosc = "Widliszki Wielkie";
                dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Ulica       = "Kwiatowa";
                dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Budynek     = "1-8";
                dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Poczta      = "11-110";

                dokument.Tresc.MiejscowoscDokumentu               = "Widliszki Wielkie";
                dokument.Tresc.Tytul                              = "Zawiadomienie w sprawie 1234/2019";
                dokument.Tresc.RodzajWnioskuRozszerzony.JakisInny = "inne pismo";
                dokument.Tresc.RodzajWnioskuRozszerzony.Rodzaj    = "zawiadomienie";
                dokument.Tresc.Informacje                         = new Informacja[]
                {
                    new Informacja()
                    {
                        Wartosc = "Ala ma kota"
                    },
                    new Informacja()
                    {
                        Wartosc = "Basia ma wózek widłowy"
                    }
                };
                dokument.Tresc.Zalaczniki                         = new Zalacznik[]
                {
                    new Zalacznik()
                    {
                        Format         = "application/octet-stream",
                        NazwaPliku     = "test.pdf",
                        DaneZalacznika = new DaneZalacznika()
                        {
                            Zawartosc = pdf
                        }
                    }
                };
                // act

                var namespaces = new XmlSerializerNamespaces();
                //namespaces.Add("", ePUAP.Client.Constants.Namespaces.WNIO_PODPISANYDOKUMENT);
                namespaces.Add("wnio", ePUAP.Client.Constants.Namespaces.CRD_WNIO);
                namespaces.Add("meta", ePUAP.Client.Constants.Namespaces.CRD_META);
                namespaces.Add("str", ePUAP.Client.Constants.Namespaces.CRD_STR);
                namespaces.Add("adr", ePUAP.Client.Constants.Namespaces.CRD_ADR);
                namespaces.Add("oso", ePUAP.Client.Constants.Namespaces.CRD_OSO);
                namespaces.Add("inst", ePUAP.Client.Constants.Namespaces.CRD_INST);

                // wnio:Dokument
                var document = dokument.ToXmlDocument(namespaces);
                var pi = document.CreateProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"http://crd.gov.pl/wzor/2013/12/12/1410/styl.xsl\"");
                document.InsertAfter(pi, document.FirstChild);
                //var document = new XmlDocument();
                //document.LoadXml(xml);

                var signed = new XAdESBESSigner().Sign(document, cert);
                var signedName = string.Format("test.{0}.xml", DateTime.Now.Ticks);
                if (signed != null)
                {
                    // save signed document without BOM
                    File.WriteAllText(signedName, document.OuterXml, new UTF8Encoding(false));
                }

                Console.WriteLine("signed.");

                // verification
                var signedXml = new SignedXml(document);
                var messageSignatureNode = document.GetElementsByTagName("Signature")[0];

                signedXml.LoadXml((XmlElement)messageSignatureNode);

                // check the signature and return the result.
                var verification = signedXml.CheckSignature(cert, true);
                Console.WriteLine("Verification: {0}", verification);

                // transformation
                using (var xmlReader = new StreamReader(signedName, new UTF8Encoding(false)))
                {
                    var xPathDoc = new XPathDocument(xmlReader);
                    var xslTrans = new XslCompiledTransform();
                    xslTrans.Load("http://crd.gov.pl/wzor/2013/12/12/1410/styl.xsl");
                    using (var writer = new XmlTextWriter(string.Format("result.{0}.html", DateTime.Now.Ticks), null))
                    {
                        xslTrans.Transform(xPathDoc, null, writer);
                        writer.Flush();
                    }
                }
            }
            catch ( Exception ex )
            {
                while ( ex != null )
                {
                    Console.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }
            }

            Console.ReadLine();
        }

        private static X509Certificate2 _certificate;

        /// <summary>
        /// Signing certificate
        /// </summary>
        public static X509Certificate2 Certificate
        {
            get
            {
                if (_certificate == null)
                {
                    X509Store store = new X509Store("My", StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                    var certificates = X509Certificate2UI.SelectFromCollection(store.Certificates, "Certificate", "Select a certificate", X509SelectionFlag.SingleSelection);

                    if (certificates.Count > 0)
                    {
                        _certificate = certificates[0];
                    }
                }

                return _certificate;
            }
        }
    }
}
