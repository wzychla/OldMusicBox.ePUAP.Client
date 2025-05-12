﻿using Microsoft.Extensions.Logging;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using OldMusicBox.ePUAP.Client.Core.Model.Skrytka;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// WS-Skrytka client
    /// 
    /// Służy do przesyłania (przedkładania) dokumentów na skrytkę. 
    /// </summary>
    /// <remarks>
    /// Production uri:  https://ws.epuap.gov.pl/pk_external_ws/services/skrytka
    /// Integration uri: https://ws-int.epuap.gov.pl/pk_external_ws/services/skrytka
    /// WSDL:            https://ws.epuap.gov.pl/pk_external_ws/services/skrytka/wsdl/skrytka.wsdl
    /// </remarks>
    public class SkrytkaClient : BaseClient
    {
        public const string INTEGRATION_URI = "https://ws-int.epuap.gov.pl/pk_external_ws/services/skrytka";
        public const string PRODUCTION_URI  = "https://ws.epuap.gov.pl/pk_external_ws/services/skrytka";

        public SkrytkaClient(string serviceUri, X509Certificate2 signingCertificate, ILogger logger) : base(serviceUri, signingCertificate, logger)
        {

        }

        #region Nadaj

        /// <summary>
        /// Interfejs służy do nadawania (przedkladania) dokumentów XML na skrytkę
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu w kontekście ktorego nadawany jest dokument</param>
        /// <param name="adresSkrytki">Adres skrytki odbiorcy</param>
        /// <param name="adresOdpowiedzi">Adres skrytki nadawcy na ktory mają być przesyłane odpowiedzi w sprawie</param>
        /// <param name="czyProbne">Określa czy to jest nadanie próbne, jedynie w celu sprawdzenia poprawności dokumentu i adresu; przy nadawaniu probnym dokument nie jest przekazywany do odbiorcy ani nie jest wystawiane UPP</param>
        /// <param name="daneDodatkowe">Dodatkowe dane w formacie XML</param>
        /// <param name="dokument">Przesyłany dokument wraz z ewentualnymi załącznikami</param>
        public virtual NadajResponse Nadaj(
            string podmiot, 
            string adresSkrytki, 
            string adresOdpowiedzi, 
            bool   czyProbne, 
            byte[] daneDodatkowe, 
            DocumentType dokument,
            out FaultModel fault
            )
        {
            // validation
            if (string.IsNullOrEmpty(podmiot))
                throw new ArgumentNullException("podmiot");
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
                PodmiotNadawcy  = podmiot,
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

        #region NadajAny

        /// <summary>
        /// Interfejs służy do nadawania (przedkladania) dokumentów dowolnego typu na skrytkę
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu w kontekście ktorego nadawany jest dokument</param>
        /// <param name="adresSkrytki">Adres skrytki odbiorcy</param>
        /// <param name="adresOdpowiedzi">Adres skrytki nadawcy na ktory mają być przesyłane odpowiedzi w sprawie</param>
        /// <param name="czyProbne">Określa czy to jest nadanie próbne, jedynie w celu sprawdzenia poprawności dokumentu i adresu; przy nadawaniu probnym dokument nie jest przekazywany do odbiorcy ani nie jest wystawiane UPP</param>
        /// <param name="daneDodatkowe">Dodatkowe dane w formacie XML</param>
        /// <param name="dokument">Przesyłany dokument wraz z ewentualnymi załącznikami</param>
        public virtual NadajAnyResponse NadajAny(
            string identyfikatorPodmiotu,
            string adresSkrytki,
            string adresOdpowiedzi,
            bool   czyProbne,
            byte[] daneDodatkowe,
            string nazwaPliku,
            DocumentAnyType dokument,
            out FaultModel fault
            )
        {
            // validation
            if (string.IsNullOrEmpty(identyfikatorPodmiotu))
                throw new ArgumentNullException("identyfikatorPodmiotu");
            if (string.IsNullOrEmpty(adresSkrytki))
                throw new ArgumentNullException("adresSkrytki");
            if (string.IsNullOrEmpty(adresOdpowiedzi))
                throw new ArgumentNullException("adresOdpowiedzi");
            if (string.IsNullOrEmpty(nazwaPliku))
                throw new ArgumentException("nazwaPliku");
            if (dokument == null)
                throw new ArgumentException("dokument");
            if (dokument.Zawartosc == null)
                throw new ArgumentException("dokument");

            var request = new NadajAnyRequest()
            {
                DaneDodatkowe   = daneDodatkowe,
                CzyProbne       = czyProbne,
                AdresOdpowiedzi = adresOdpowiedzi,
                AdresSkrytki    = adresSkrytki,
                PodmiotNadawcy  = identyfikatorPodmiotu,
                NazwaPliku      = nazwaPliku,
                Document        = dokument
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<NadajAnyRequest, NadajAnyResponse, NadajAnyResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
