using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core
{
    public interface IServiceUriProvider
    {
        string DoreczycielUri { get; }
        string FileRepoServiceUri { get; }
        string GetTpUserInfoUri { get; }
        string ObslugaUPPUri { get; }
        string PullUri { get; }
        string SkrytkaUri { get; }
        string TpSigningUri { get; }
        string TpSigning5Uri { get; }
        string TpUserObjectsInfoUri { get; }
        string ZarzadzanieDokumentamiUri { get; }
    }
}
