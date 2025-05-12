using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Pull
{
    /// <summary>
    /// PotwierdzOdebranie response
    /// </summary>
    [XmlRoot("OdpowiedzPullPotwierdz", Namespace = Namespaces.OBI)]
    public class PotwierdzOdebranieResponse : IServiceResponse
    {
        [XmlElement("status", Namespace = "")]
        public StatusModel Status { get; set; }
    }
}
