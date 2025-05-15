using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core
{
    public class IntegrationServiceUriProvider : IServiceUriProvider
    {
        public string DoreczycielUri => DoreczycielClient.INTEGRATION_URI;
        public string FileRepoServiceUri => FileRepoServiceClient.INTEGRATION_URI;
        public string GetTpUserInfoUri => ServiceClient.INTEGRATION_URI;
        public string ObslugaUPPUri => ObslugaUPPClient.INTEGRATION_URI;
        public string PullUri => PullClient.INTEGRATION_URI;
        public string SkrytkaUri => SkrytkaClient.INTEGRATION_URI;
        public string TpSigningUri => TpSigningClient.INTEGRATION_URI;
        public string TpSigning5Uri => TpSigning5Client.INTEGRATION_URI;
        public string TpUserObjectsInfoUri => TpUserObjectsInfoClient.INTEGRATION_URI;
        public string ZarzadzanieDokumentamiUri => ZarzadzanieDokumentamiClient.INTEGRATION_URI;
    }

    public class ProductionServiceUriProvider : IServiceUriProvider
    {
        public string DoreczycielUri => DoreczycielClient.PRODUCTION_URI;
        public string FileRepoServiceUri => FileRepoServiceClient.PRODUCTION_URI;
        public string GetTpUserInfoUri => ServiceClient.PRODUCTION_URI;
        public string ObslugaUPPUri => ObslugaUPPClient.PRODUCTION_URI;
        public string PullUri => PullClient.PRODUCTION_URI;
        public string SkrytkaUri => SkrytkaClient.PRODUCTION_URI;
        public string TpSigningUri => TpSigningClient.PRODUCTION_URI;
        public string TpSigning5Uri => TpSigning5Client.PRODUCTION_URI;
        public string TpUserObjectsInfoUri => TpUserObjectsInfoClient.PRODUCTION_URI;
        public string ZarzadzanieDokumentamiUri => ZarzadzanieDokumentamiClient.PRODUCTION_URI;
    }
}
