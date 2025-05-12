using Microsoft.Extensions.Logging;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using OldMusicBox.ePUAP.Client.Core.Model.TrustedProfileInfoForPESEL;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// TpUserObjectsInfo Client
    /// </summary>
    public class TpUserObjectsInfoClient : BaseClient
    {
        public const string INTEGRATION_URI = "https://int.pz.gov.pl/pz-services/tpUserObjectsInfoService";
        public const string PRODUCTION_URI  = "https://pz.gov.pl/pz-services/tpUserObjectsInfoService";

        public TpUserObjectsInfoClient(string serviceUri, X509Certificate2 signingCertificate, ILogger logger) : base(serviceUri, signingCertificate, logger)
        {

        }

        #region TrustedProfileInfoForPESEL

        /// <summary>
        /// TrustedProfileInfoForPESEL call
        /// </summary>
        /// <remarks>
        /// Either returns a valid response or a fault information
        /// </remarks>
        public virtual TrustedProfileInfoForPESELResponse TrustedProfileInfoForPESEL(
            string PESEL,
            ProfileInfoEnum profileInfo,
            out FaultModel fault)
        {
            if (string.IsNullOrEmpty(PESEL))
            {
                throw new ArgumentNullException("PESEL");
            }
            fault = null;

            // request
            var request =
                new TrustedProfileInfoForPESELRequest()
                {
                    PESEL       = PESEL,
                    ProfileInfo = profileInfo
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<TrustedProfileInfoForPESELRequest, TrustedProfileInfoForPESELResponse, TrustedProfileInfoForPESELResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
