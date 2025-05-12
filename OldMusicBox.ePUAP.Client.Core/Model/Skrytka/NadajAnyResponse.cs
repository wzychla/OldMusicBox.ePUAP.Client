using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Skrytka
{
    /// <summary>
    /// NadajAny response
    /// </summary>
    [XmlRoot("OdpowiedzSkrytki", Namespace = Namespaces.OBI)]
    public class NadajAnyResponse : IServiceResponse
    {
        [XmlElement("status", Namespace = "")]
        public StatusModel Status { get; set; }

        [XmlElement("statusOdbiorcy", Namespace = "")]
        public StatusModel StatusOdbiorcy { get; set; }

        [XmlElement("identyfikatorDokumentu", Namespace = "")]
        public int IdentyfikatorDokumentu { get; set; }

        [XmlElement("identyfikatorUpp", Namespace = "")]
        public int IdentyfikatorUPP { get; set; }

        [XmlElement("zalacznik", Namespace = "")]
        public DocumentType Zalacznik { get; set; }
    }
}
