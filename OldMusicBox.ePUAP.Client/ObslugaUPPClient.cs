using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.ePUAP.Client.Model.ObslugaUPP;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// WS-obslugaUpp client
    /// 
    /// Służy do generowania i wysyłania UPP. W tej usłudze zostały wyodrębnione dwie operacje:
    /// dajUpp oraz dajUppPrzeslij.
    /// <remarks>
    /// Production uri:  https://ws.epuap.gov.pl/pk_external_ws/services/obslugaUpp
    /// Integration uri: https://ws-int.epuap.gov.pl/pk_external_ws/services/obslugaUpp
    /// WSDL:            https://ws.epuap.gov.pl/pk_external_ws/services/obslugaUpp/wsdl/obslugaUpp.wsdl
    /// </remarks>
    public class ObslugaUPPClient : BaseClient
    {
        public const string INTEGRATION_URI = "https://ws-int.epuap.gov.pl/pk_external_ws/services/obslugaUpp";
        public const string PRODUCTION_URI  = "https://ws.epuap.gov.pl/pk_external_ws/services/obslugaUpp";

        public ObslugaUPPClient( string serviceUri, X509Certificate2 signingCertificate ) : base( serviceUri, signingCertificate )
        {

        }

        #region Daj Upp

        /// <summary>
        /// Interfejs służy do wygenerowania UPP 
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu adresata</param>
        /// <param name="nadawca">Nazwa sprawdzanej skrytki</param>
        /// <param name="dokument">Informacje o dokumencie</param>
        public virtual DajUPPResponse DajUPP(
            string podmiot,
            UzytkownikType nadawca,
            DocumentType dokument,
            out FaultModel fault
            )
        {
            // validation
            if ( string.IsNullOrEmpty( podmiot ) )
                throw new ArgumentNullException( "podmiot" );
            if ( nadawca == null )
                throw new ArgumentNullException( "nadawca" );
            if ( dokument == null )
                throw new ArgumentNullException( "nadawca" );

            var request = new DajUPPRequest()
            {
                Podmiot      = podmiot,
                Nadawca      = nadawca,
                Dokument     = dokument
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<DajUPPRequest, DajUPPResponse, DajUPPResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion

        #region Daj Upp wyślij

        #endregion
    }
}
