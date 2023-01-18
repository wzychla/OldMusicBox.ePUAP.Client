using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.ObslugaUPP
{
    /// <summary>
    /// Daj UPP response
    /// </summary>
    [XmlRoot( "OdpowiedzDajUpp", Namespace = Namespaces.OBI )]
    public class DajUPPResponse : IServiceResponse
    {
        [XmlElement( "status", Namespace = "" )]
        public StatusModel Status { get; set; }

        [XmlElement( "upp", Namespace = "" )]
        public DocumentType UPP { get; set; }
    }
}
