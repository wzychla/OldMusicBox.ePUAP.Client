using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// Holds values of SSO endpoints
    /// </summary>
    public class SSOClient
    {
        /// <summary>
        /// Integration SSO
        /// </summary>
        public const string INTEGRATION_SSO      = "https://int.pz.gov.pl/dt/SingleSignOnService";
        /// <summary>
        /// Integration SLO
        /// </summary>
        public const string INTEGRATION_SLO      = "https://int.pz.gov.pl/dt/SingleLogoutService";
        /// <summary>
        /// Integration Arifact
        /// </summary>
        public const string INTEGRATION_ARTIFACT = "https://int.pz.gov.pl/dt-services/idpArtifactResolutionService";

        /// <summary>
        /// Production SSO
        /// </summary>
        public const string PRODUCTION_SSO      = "https://pz.gov.pl/dt/SingleSignOnService";
        /// <summary>
        /// Production SLO
        /// </summary>
        public const string PRODUCTION_SLO      = "https://pz.gov.pl/dt/SingleLogoutService";
        /// <summary>
        /// Production artifact
        /// </summary>
        public const string PRODUCTION_ARTIFACT = "https://pz.gov.pl/dt-services/idpArtifactResolutionService";
    }
}
