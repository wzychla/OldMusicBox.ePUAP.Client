using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Doreczyciel
{
    /// <summary>
    /// Dorecz response
    /// </summary>
    [XmlRoot("OdpowiedzDoreczyciela", Namespace = Namespaces.OBI)]
    public class DoreczResponse : IServiceResponse
    {
        [XmlElement("status", Namespace = "")]
        public StatusModel Status { get; set; }

        [XmlElement("identyfikatorDokumentu", Namespace = "")]
        public int IdentyfikatorDokumentu { get; set; }

        [XmlElement("identyfikatorZlecenia", Namespace = "")]
        public int IdentyfikatorZlecenia { get; set; }
    }
}
