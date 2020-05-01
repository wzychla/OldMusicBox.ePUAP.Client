using OldMusicBox.ePUAP.Client.Model.Fault;
using OldMusicBox.ePUAP.Client.Model.Skrytka;
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
                throw new ArgumentNullException("identyfikatorPodmiotu");
            if ( string.IsNullOrEmpty(adresSkrytki))
                throw new ArgumentNullException("adresSkrytki");
            if (string.IsNullOrEmpty(adresOdpowiedzi))
                throw new ArgumentNullException("adresOdpowiedzi");
            if (dokument == null )
                throw new ArgumentException("dokument");
            if (string.IsNullOrEmpty(dokument.NazwaPliku))
                throw new ArgumentException("dokument");
            if (string.IsNullOrEmpty(dokument.TypPliku))
                throw new ArgumentException("dokument");
            if (dokument.Zawartosc == null)
                throw new ArgumentException("dokument");

            var request = new NadajRequest()
            {
                DaneDodatkowe   = daneDodatkowe,
                CzyProbne       = czyProbne,
                AdresOdpowiedzi = adresOdpowiedzi,
                AdresSkrytki    = adresSkrytki,
                PodmiotNadawcy  = identyfikatorPodmiotu,
                Document        = dokument
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<NadajRequest, NadajResponse, NadajResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
