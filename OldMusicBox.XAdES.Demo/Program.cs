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

namespace OldMusicBox.XAdES.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // arrange
                var xml  = File.ReadAllText("test.xml", Encoding.UTF8);
                var pdf     = File.ReadAllBytes("test.pdf");
                var cert    = Program.Certificate;
                if ( cert   == null )
                {
                    Console.WriteLine("No certificate selected.");
                    Environment.Exit(0);
                }

                var dokument = new Dokument();

                dokument.Opis.Data.Czas.Wartosc = DateTime.Now;

                dokument.Dane.Naglowek.Nazwa.Wartosc = "test.pdf";
                dokument.Dane.Data.Czas.Wartosc = DateTime.Now;

                dokument.Tresc.Zalaczniki = new Zalacznik[]
                {
                    new Zalacznik()
                    {
                        Format         = "application/pdf",
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
                namespaces.Add("wnio", ePUAP.Client.Constants.Namespaces.WNIO_PODPISANYDOKUMENT);
                namespaces.Add("meta", ePUAP.Client.Constants.Namespaces.WNIO_META);
                namespaces.Add("str",  ePUAP.Client.Constants.Namespaces.WNIO_STR);

                var document = dokument.ToXmlDocument(namespaces);
                //var document = new XmlDocument();
                //document.LoadXml(xml);

                var signed = new XAdESBESSigner().Sign(document, cert);
                if ( signed != null )
                {
                    // save signed document without BOM
                    File.WriteAllText(string.Format("test.{0}.xml", DateTime.Now.Ticks), document.OuterXml, new UTF8Encoding(false));
                }

                Console.WriteLine("signed.");

                // verification
                var signedXml            = new SignedXml(document);
                var messageSignatureNode = document.GetElementsByTagName("Signature")[0];

                signedXml.LoadXml((XmlElement)messageSignatureNode);

                // check the signature and return the result.
                var verification = signedXml.CheckSignature(cert, true);
                Console.WriteLine("Verification: {0}", verification);

                Console.ReadLine();
            }
            catch ( Exception ex )
            {
                while ( ex != null )
                {
                    Console.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }
            }
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
