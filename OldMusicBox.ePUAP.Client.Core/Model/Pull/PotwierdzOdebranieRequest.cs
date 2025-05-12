using OldMusicBox.ePUAP.Client.Core.Request;
using OldMusicBox.ePUAP.Client.Core.Constants;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Pull
{
    /// <summary>
    /// PotwierdzOdebranie request
    /// </summary>
    [XmlRoot("ZapytaniePullPotwierdz", Namespace = Namespaces.OBI)]
    public class PotwierdzOdebranieRequest : IServiceRequest
    {
        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return null;
            }
        }

        public string SOAPAction
        {
            get
            {
                return null;
            }
        }

        [XmlElement("podmiot", Namespace = "")]
        public string Podmiot { get; set; }

        [XmlElement("nazwaSkrytki", Namespace = "")]
        public string NazwaSkrytki { get; set; }

        [XmlElement("adresSkrytki", Namespace = "")]
        public string AdresSkrytki { get; set; }

        [XmlElement("skrot", Namespace = "")]
        public byte[] Skrot { get; set; }
    }
}
