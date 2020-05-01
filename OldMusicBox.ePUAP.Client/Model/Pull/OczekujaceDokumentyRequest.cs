using System;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Constants;

namespace OldMusicBox.ePUAP.Client.Model.Pull
{
    [XmlRoot("ZapytaniePullOczekujace", Namespace = Namespaces.OBI)]
    public class OczekujaceDokumentyRequest : IServiceRequest
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
                return "http://ws.epuap.gov.pl/pull/oczekujaceDokumenty";
            }
        }

        [XmlElement("podmiot", Namespace = "")]
        public string Podmiot { get; set; }

        [XmlElement("nazwaSkrytki", Namespace = "")]
        public string NazwaSkrytki { get; set; }

        [XmlElement("adresSkrytki", Namespace = "")]
        public string AdresSkrytki { get; set; }
    }
}
