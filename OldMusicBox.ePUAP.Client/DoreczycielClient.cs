using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.ePUAP.Client.Model.Doreczyciel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// WS-Doręczyciel client
    /// 
    /// Służy do wykonania zlecenia doręczenia dokumentu ze zwrotnym potwierdzeniem odbioru. 
    /// </summary>
    /// <remarks>
    /// Production uri:  https://ws.epuap.gov.pl/pk_external_ws/services/doreczyciel
    /// Integration uri: https://int.epuap.gov.pl/pk_external_ws/services/doreczyciel
    /// WSDL:            https://ws.epuap.gov.pl/pk_external_ws/services/doreczyciel/wsdl/doreczyciel.wsdl
    /// </remarks>
    public class DoreczycielClient : BaseClient
    {
        public const string INTEGRATION_URI = "https://int.epuap.gov.pl/pk_external_ws/services/doreczyciel";
        public const string PRODUCTION_URI  = "https://ws.epuap.gov.pl/pk_external_ws/services/doreczyciel";

        public DoreczycielClient(string serviceUri, X509Certificate2 signingCertificate) : base(serviceUri, signingCertificate)
        {

        }

        #region Dorecz

        /// <summary>
        /// Interfejs służy do nadawania (przedkladania) dokumentów XML na skrytkę
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu w kontekście ktorego zlecane jest doręczenie</param>
        /// <param name="adresSkrytki">Adres skrytki odbiorcy</param>
        /// <param name="adresOdpowiedzi">Adres skrytki nadawcy na ktory mają być przesyłane odpowiedzi w sprawie</param>
        /// <param name="terminDoreczenia">Termin doręczenia</param>
        /// <param name="czyProbne">Określa czy to jest nadanie próbne, jedynie w celu sprawdzenia poprawności dokumentu i adresu; przy nadawaniu probnym dokument nie jest przekazywany do odbiorcy</param>
        /// <param name="identyfikatorDokumentu">Identyfikator dokumentu w systemie zewnętrznym, będzie umieszczony w UPD</param>
        /// <param name="identyfikatorSprawy">Identyfikator sprawy w systemie zewnętrznym, będzie umieszczony w UPD</param>
        /// <param name="daneDodatkowe">Dodatkowe dane w formacie XML</param>
        /// <param name="upd">Parametr opcjonalny, zawierający gotowe UPD, stworzone przez nadawcę (jeżeli nie zostanie podany, to UPD zostanie wygenerowane automatycznie)</param>
        /// <param name="dokument">Informacje o dokumencie</param>
        public virtual DoreczResponse Dorecz(
            string   podmiot, 
            string   adresSkrytki, 
            string   adresOdpowiedzi, 
            DateTime terminDoreczenia, 
            bool     czyProbne, 
            string   identyfikatorDokumentu, 
            string   identyfikatorSprawy, 
            byte[]   daneDodatkowe, 
            DocumentType UPD, 
            DocumentType dokument,
            out FaultModel fault
            )
        {
            // validation
            if (string.IsNullOrEmpty(podmiot))
                throw new ArgumentNullException("podmiot");
            if (string.IsNullOrEmpty(adresSkrytki))
                throw new ArgumentNullException("adresSkrytki");
            if (string.IsNullOrEmpty(adresOdpowiedzi))
                throw new ArgumentNullException("adresOdpowiedzi");

            if (dokument == null)
                throw new ArgumentException("dokument");

            if (string.IsNullOrEmpty(dokument.NazwaPliku))
                throw new ArgumentException("dokument");
            if (string.IsNullOrEmpty(dokument.TypPliku))
                throw new ArgumentException("dokument");
            if (dokument.Zawartosc == null)
                throw new ArgumentException("dokument");

            // jak u licha przekazać tu UPD? w dokumentacji jest to pominięte!
            var request = new DoreczRequest()
            {
                IdentyfikatorPodmiotu  = podmiot,
                AdresOdpowiedzi        = adresOdpowiedzi,
                AdresSkrytki           = adresSkrytki,
                TerminDoreczenia       = terminDoreczenia,
                CzyProbne              = czyProbne,
                IdentyfikatorDokumentu = identyfikatorDokumentu,
                IdentyfikatorSprawy    = identyfikatorSprawy,
                DaneDodatkowe          = daneDodatkowe,    
                            
                Document               = dokument
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<DoreczRequest, DoreczResponse, DoreczResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion

        #region Odbierz
        #endregion
    }
}
