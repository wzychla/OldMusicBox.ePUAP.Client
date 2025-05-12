using Microsoft.Extensions.Logging;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using OldMusicBox.ePUAP.Client.Core.Model.GetTpUserInfo;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// The GetTpUserInfo service client
    /// Responsible for calling multiple services
    /// </summary>
    public class ServiceClient : BaseClient
    {
        public const string INTEGRATION_URI = "https://int.pz.gov.pl/pz-services/tpUserInfo";
        public const string PRODUCTION_URI  = "https://pz.gov.pl/pz-services/tpUserInfo";

        public ServiceClient( string serviceUri, X509Certificate2 signingCertificate, ILogger logger) : base(serviceUri, signingCertificate, logger)
        {

        }

        #region GetTpUserInfo

        /// <summary>
        /// GetTpUserInfo call
        /// </summary>
        /// <remarks>
        /// Either returns a valid response or a fault information
        /// </remarks>
        public virtual GetTpUserInfoResponse GetTpUserInfo(
            string sessionIndex, 
            out FaultModel fault )
        {
            if (string.IsNullOrEmpty(sessionIndex))
            {
                throw new ArgumentNullException("sessionIndex");
            }
            fault = null;

            // request
            var request =
                new GetTpUserInfoRequest()
                {
                    TgSid                = sessionIndex,
                    SystemOrganisationId = "0"
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<GetTpUserInfoRequest, GetTpUserInfoResponse, GetTpUserInfoResponseHandler>(
                this.ServiceUri, 
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
