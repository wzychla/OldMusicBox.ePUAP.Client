using OldMusicBox.ePUAP.Client.XAdES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
                var cert = Program.Certificate;
                if ( cert == null )
                {
                    Console.WriteLine("No certificate selected.");
                    Environment.Exit(0);
                }

                // act
                var document = new XmlDocument();
                document.LoadXml(xml);

                var signed = new XAdESBESSigner().Sign(document, cert);
                if ( signed != null )
                {
                    // save signed document without BOM
                    File.WriteAllText(string.Format("test.{0}.xml", DateTime.Now.Ticks), document.OuterXml, new UTF8Encoding(false));
                }

                Console.WriteLine("finished");
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
