using OldMusicBox.ePUAP.Client.Model.Fault;
using OldMusicBox.ePUAP.Client.Skrytka;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// WS-Skrytka client
    /// </summary>
    /// <remarks>
    /// Production uri:  https://ws.epuap.gov.pl/pk_external_ws/services/skrytka
    /// Integration uri: https://int.epuap.gov.pl/pk_external_ws/services/skrytka
    /// WSDL:            https://ws.epuap.gov.pl/pk_external_ws/services/skrytka/wsdl/skrytka.wsdl
    /// </remarks>
    public class SkrytkaClient : BaseClient
    {
        public SkrytkaClient(string serviceUri, X509Certificate2 signingCertificate) : base(serviceUri, signingCertificate)
        {

        }

        #region Nadaj

        public virtual NadajResponse Nadaj(
            string serviceUrl,
            string identyfikatorPodmiotu, 
            string adresSkrytki, 
            string adresOdpowiedzi, 
            bool   czyProbne, 
            byte[] daneDodatkowe, 
            NadajRequest.DocumentType dokument,
            out FaultModel fault
            )
        {
            // validation
            if (string.IsNullOrEmpty(identyfikatorPodmiotu))
                throw new ArgumentNullException("IdentyfikatorPodmiotu");
            if ( string.IsNullOrEmpty(adresSkrytki))
                throw new ArgumentNullException("AdresSkrytki");
            if (string.IsNullOrEmpty(adresOdpowiedzi))
                throw new ArgumentNullException("AdresOdpowiedzi");
            if (dokument == null )
                throw new ArgumentNullException("Dokument");
            if (string.IsNullOrEmpty(dokument.NazwaPliku))
                throw new ArgumentException("Dokument");
            if (string.IsNullOrEmpty(dokument.TypPliku))
                throw new ArgumentException("Dokument");
            if (dokument.Zawartosc == null)
                throw new ArgumentException("Dokument");

            var request = new NadajRequest()
            {
                AdresOdpowiedzi = adresOdpowiedzi,
                AdresSkrytki    = adresSkrytki,
                Document        = dokument
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<NadajRequest, NadajResponse, NadajResponseHandler>(
                serviceUrl,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
