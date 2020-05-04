using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Skrytka
{
    /// <summary>
    /// Nadaj response
    /// </summary>
    [XmlRoot("OdpowiedzSkrytki", Namespace = Namespaces.OBI)]
    public class NadajResponse : IServiceResponse
    {
        [XmlElement("status", Namespace = "")]
        public StatusModel Status { get; set; }

        [XmlElement("identyfikatorDokumentu", Namespace = "")]
        public int IdentyfikatorDokumentu { get; set; }

        [XmlElement("identyfikatorUpp", Namespace = "")]
        public int IdentyfikatorUPP { get; set; }

        [XmlElement("zalacznik", Namespace = "")]
        public DocumentType Zalacznik { get; set; }
    }
}
