using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Pull
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
