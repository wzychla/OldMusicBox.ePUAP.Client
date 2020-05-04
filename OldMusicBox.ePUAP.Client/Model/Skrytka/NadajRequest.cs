using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Skrytka
{
    [XmlRoot("Dokument", Namespace = Namespaces.OBI)]
    public class NadajRequest : IServiceRequest
    {
        public string SOAPAction
        {
            get
            {
                return null;
            }
        }

        [XmlIgnore]
        public byte[] DaneDodatkowe { get; set; }
        [XmlIgnore]
        public bool CzyProbne { get; set; }

        [XmlIgnore]
        public string AdresOdpowiedzi { get; set; }
        [XmlIgnore]
        public string AdresSkrytki { get; set; }
        [XmlIgnore]
        public string PodmiotNadawcy { get; set; }

        [XmlIgnore]
        public DocumentType Document { get; set; }

        [XmlElement(ElementName = "nazwaPliku", Namespace = "")]
        public string NazwaPliku
        {
            get
            {
                return this.Document.NazwaPliku;
            }
            set
            {
                this.Document.NazwaPliku = value;
            }
        }

        [XmlElement(ElementName = "typPliku", Namespace = "")]
        public string TypPliku
        {
            get
            {
                return this.Document.TypPliku;
            }
            set
            {
                this.Document.TypPliku = value;
            }
        }

        [XmlElement(ElementName = "zawartosc", Namespace = "")]
        public byte[] Zawartosc
        {
            get
            {
                return this.Document.Zawartosc;
            }
            set
            {
                this.Document.Zawartosc = value;
            }
        }
        
        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return new HeaderAttribute[]
                {
                    new DaneDodatkoweHeaderAttribute(this.DaneDodatkowe),
                    new CzyProbneHeaderAttribute(this.CzyProbne),
                    new AdresOdpowiedziHeaderAttribute(this.AdresOdpowiedzi),
                    new AdresSkrytkiHeaderAttribute(this.AdresSkrytki),
                    new IdentyfikatorPodmiotuHeaderAttribute(this.PodmiotNadawcy)
                };
            }
        }
    }

    public class DocumentType
    {
        [XmlElement("nazwaPliku", Namespace = "")]
        public string NazwaPliku { get; set; }

        [XmlElement("typPliku", Namespace = "")]
        public string TypPliku { get; set; }

        [XmlElement("zawartosc", Namespace = "")]
        public byte[] Zawartosc { get; set; }
    }
}
