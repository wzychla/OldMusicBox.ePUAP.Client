using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.ObslugaUPP
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
