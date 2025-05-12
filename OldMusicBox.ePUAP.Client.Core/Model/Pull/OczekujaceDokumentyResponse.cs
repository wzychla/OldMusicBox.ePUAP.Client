using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Pull
{
    /// <summary>
    /// OczekujaceDokumenty response
    /// </summary>
    [XmlRoot("OdpowiedzPullOczekujace", Namespace = Namespaces.OBI)]
    public class OczekujaceDokumentyResponse : IServiceResponse
    {
        [XmlElement("status", Namespace = "")]
        public StatusModel Status { get; set; }

        [XmlElement("oczekujace", Namespace = "")]
        public int Oczekujace { get; set; }
    }
}
