using OldMusicBox.ePUAP.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Request
{
    /// <summary>
    /// Generic WS-Security compliant request factory
    /// </summary>
    public class RequestFactory
    {
        public X509Certificate2 SigningCertificate { get; set; }

        public RequestFactory( X509Certificate2 signingCertificate )
        {
            if (signingCertificate == null ||
                 signingCertificate.PrivateKey == null
                )
            {
                throw new ArgumentNullException("signingCertificate");
            }

            this.SigningCertificate = signingCertificate;
        }

        /// <summary>
        /// Return bare string can can be sent to a ePUAP service
        /// </summary>
        public virtual string CreateRequest( IServiceRequest request )
        {
            if ( request == null )
            {
                throw new ArgumentNullException("request");
            }

            // create the SOAP envelope and internal IDs
            var envelope               = new Envelope();
            envelope.Body.Contents     = request;
            envelope.Header.Attributes = request.HeaderAttributes;

            return envelope.GetSignedXML(this.SigningCertificate);
        }
    }
}
