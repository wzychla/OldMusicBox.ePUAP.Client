using System;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Headers;

namespace OldMusicBox.ePUAP.Client.Model.Doreczyciel
{
    /// <summary>
    /// Dorecz request
    /// </summary>
    [XmlRoot("Dokument", Namespace = Namespaces.OBI)]
    public class DoreczRequest : IServiceRequest
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
        public string IdentyfikatorSprawy { get; set; }
        [XmlIgnore]
        public string IdentyfikatorDokumentu { get; set; }
        [XmlIgnore]
        public string IdentyfikatorPodmiotu { get; set; }

        [XmlIgnore]
        public bool CzyProbne { get; set; }

        [XmlIgnore]
        public string AdresOdpowiedzi { get; set; }
        [XmlIgnore]
        public string AdresSkrytki { get; set; }
        [XmlIgnore]
        public DateTime TerminDoreczenia { get; set; }

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
                    new IdentyfikatorSprawyHeaderAttribute(this.IdentyfikatorSprawy),
                    new IdentyfikatorDokumentuHeaderAttribute(this.IdentyfikatorDokumentu),
                    new CzyProbneHeaderAttribute(this.CzyProbne),
                    new TerminDoreczeniaHeaderAttribute(this.TerminDoreczenia),
                    new AdresOdpowiedziHeaderAttribute(this.AdresOdpowiedzi),
                    new AdresSkrytkiHeaderAttribute(this.AdresSkrytki),
                    new IdentyfikatorPodmiotuHeaderAttribute(this.AdresSkrytki),
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
