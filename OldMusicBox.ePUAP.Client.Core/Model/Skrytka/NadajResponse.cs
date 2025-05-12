using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Skrytka
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

        /// <summary>
        /// Jeżeli skrzynka docelowa ma znacznik generowania UPP, to w tym miejscu system dołączy dokument UPP
        /// dla nadawcy.
        /// </summary>
        /// <remarks>
        /// Fakt anegdotyczny: w pewnych okolicznościach system może nie dodać UPP tutaj, tylko
        /// po pewnym czasie asynchronicznie umieścić UPP w skrzynce nadawcy.
        /// Niepotwierdzone empirycznie.
        /// </remarks>
        [XmlElement("zalacznik", Namespace = "")]
        public DocumentType Zalacznik { get; set; }
    }
}
