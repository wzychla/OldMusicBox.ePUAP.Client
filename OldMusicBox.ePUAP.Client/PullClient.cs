using OldMusicBox.ePUAP.Client.Model.Fault;
using OldMusicBox.ePUAP.Client.Model.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// WS-Pull client
    /// </summary>
    /// <remarks>
    /// Production uri:  https://ws.epuap.gov.pl/pk_external_ws/services/pull
    /// Integration uri: https://int.epuap.gov.pl/pk_external_ws/services/pull
    /// WSDL:            https://ws.epuap.gov.pl/pk_external_ws/services/pull/wsdl/pull.wsdl
    /// </remarks>
    public class PullClient : BaseClient
    {
        public PullClient(string serviceUri, X509Certificate2 signingCertificate) : base(serviceUri, signingCertificate)
        {

        }

        #region Oczekujące dokumentu

        public virtual OczekujaceDokumentyResponse OczekujaceDokumenty(
            string podmiot,
            string nazwaSkrytki,
            string adresSkrytki,
            out FaultModel fault
            )
        {
            // validation
            if (string.IsNullOrEmpty(podmiot))
                throw new ArgumentNullException("podmiot");
            if (string.IsNullOrEmpty(nazwaSkrytki))
                throw new ArgumentNullException("nazwaSkrytki");
            if (string.IsNullOrEmpty(adresSkrytki))
                throw new ArgumentNullException("adresSkrytki");

            var request = new OczekujaceDokumentyRequest()
            {
                Podmiot      = podmiot,
                NazwaSkrytki = nazwaSkrytki,
                AdresSkrytki = adresSkrytki
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<OczekujaceDokumentyRequest, OczekujaceDokumentyResponse, OczekujaceDokumentyResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
