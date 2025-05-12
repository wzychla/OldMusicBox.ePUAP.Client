using Microsoft.Extensions.Logging;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using OldMusicBox.ePUAP.Client.Core.Model.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// WS-Pull client
    /// 
    /// Służy do pobierania dokumentów z kolejki skrytki z ustawionym trybem PULL. 
    /// </summary>
    /// <remarks>
    /// Production uri:  https://ws.epuap.gov.pl/pk_external_ws/services/pull
    /// Integration uri: https://ws-int.epuap.gov.pl/pk_external_ws/services/pull
    /// WSDL:            https://ws.epuap.gov.pl/pk_external_ws/services/pull/wsdl/pull.wsdl
    /// </remarks>
    public class PullClient : BaseClient
    {
        public const string INTEGRATION_URI = "https://ws-int.epuap.gov.pl/pk_external_ws/services/pull";
        public const string PRODUCTION_URI  = "https://ws.epuap.gov.pl/pk_external_ws/services/pull";

        public PullClient(string serviceUri, X509Certificate2 signingCertificate, ILogger logger) : base(serviceUri, signingCertificate, logger)
        {

        }

        #region Oczekujące dokumenty

        /// <summary>
        /// Interfejs służy do uzyskania informacji o liczbie dokumentów oczekujących na pobranie dla wskazanego podmiotu i adresu skrytki. 
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu</param>
        /// <param name="nazwaSkrytki">Nazwa sprawdzanej skrytki</param>
        /// <param name="adresSkrytki">Adres sprawdzanej skrytki</param>
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

        #region Pobierz następny

        /// <summary>
        /// Interfejs służy do pobierania kolejnego dokumentu oczekującego w kolejce do pobrania w trybie PULL z wskazanego podmiotu i adresu skrytki.
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu</param>
        /// <param name="nazwaSkrytki">Nazwa sprawdzanej skrytki</param>
        /// <param name="adresSkrytki">Adres sprawdzanej skrytki</param>
        public virtual PobierzNastepnyResponse PobierzNastepny(
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

            var request = new PobierzNastepnyRequest()
            {
                Podmiot      = podmiot,
                NazwaSkrytki = nazwaSkrytki,
                AdresSkrytki = adresSkrytki
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<PobierzNastepnyRequest, PobierzNastepnyResponse, PobierzNastepnyResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion

        #region Potwierdź odebranie

        /// <summary>
        /// Interfejs służy do potwierdzania prawidłowego odbioru dokumentu przez system zewnętrzny oraz usuwania dokumentu z kolejki oczekujących na pobranie.
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu</param>
        /// <param name="nazwaSkrytki">Nazwa sprawdzanej skrytki</param>
        /// <param name="adresSkrytki">Adres sprawdzanej skrytki</param>
        /// <param name="skrot">Obliczony skrot SHA-1 odebranego dokumentu</param>
        public virtual PotwierdzOdebranieResponse PotwierdzOdebranie(
            string podmiot,
            string nazwaSkrytki,
            string adresSkrytki,
            byte[] skrot,
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
            if (skrot == null)
                throw new ArgumentNullException("skrot");

            var request = new PotwierdzOdebranieRequest()
            {
                Podmiot      = podmiot,
                NazwaSkrytki = nazwaSkrytki,
                AdresSkrytki = adresSkrytki,
                Skrot        = skrot
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<PotwierdzOdebranieRequest, PotwierdzOdebranieResponse, PotwierdzOdebranieResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
