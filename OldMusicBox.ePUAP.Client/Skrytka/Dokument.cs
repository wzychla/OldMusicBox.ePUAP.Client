using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Skrytka
{
    [XmlRoot("Dokument", Namespace = Namespaces.OBI)]
    public class Dokument : IServiceRequest
    {
        public string SOAPAction
        {
            get
            {
                return "http://ws.epuap.gov.pl/skrytka/nadaj";
            }
        }

        [XmlIgnore]
        public string AdresOdpowiedzi { get; set; }

        [XmlElement(ElementName = "nazwaPliku", Namespace = "")]
        public string NazwaPliku { get; set; }

        [XmlElement(ElementName = "typPliku", Namespace = "")]
        public string TypPliku { get; set; }

        [XmlElement(ElementName = "zawartosc", Namespace = "")]
        public byte[] Zawartosc { get; set; }

        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return new[]
                {
                    new AdresOdpowiedziHeaderAttribute(this.AdresOdpowiedzi)
                };
            }
        }
    }
}
