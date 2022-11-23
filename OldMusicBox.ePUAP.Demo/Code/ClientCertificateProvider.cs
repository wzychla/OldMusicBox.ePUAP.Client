using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Hosting;

namespace OldMusicBox.ePUAP.Demo
{
    /// <summary>
    /// X509 certificate provider for the client.
    /// Client uses the cert to sign SAML2 requests 
    /// sent to the server.
    /// 
    /// The ePUAP certificate has to be obtained following
    /// the procedure described at ePUAP website.
    /// </summary>
    public class ClientCertificateProvider
    {
        private static X509Certificate2 _clientCertificate;

        public X509Certificate2 GetClientCertificate()
        {
            var p12location = ConfigurationManager.AppSettings["p12location"];
            var p12password = ConfigurationManager.AppSettings["p12password"];

            if (_clientCertificate == null)
            {
                var path = p12location.Contains(":") ? p12location : HostingEnvironment.MapPath(p12location);
                using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    _clientCertificate = new X509Certificate2(bytes, p12password, X509KeyStorageFlags.Exportable);
                }
            }

            return _clientCertificate;
        }

        /*
        private static X509Certificate2 _idpCertificate;

        public X509Certificate2 GetIdPCertificate()
        {
            if (_idpCertificate == null)
            {
                var path = HostingEnvironment.MapPath("~/ClientCertificate/IDP.cer");
                using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    _idpCertificate = new X509Certificate2(bytes);
                }
            }

            return _idpCertificate;
        }
        */
    }

}