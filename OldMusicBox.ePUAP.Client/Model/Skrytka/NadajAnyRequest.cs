using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Model.Headers;
using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Skrytka
{
    /// <summary>
    /// NadajAny request
    /// </summary>
    [XmlRoot("Dokument", Namespace = Namespaces.OBI_EXT)]
    public class NadajAnyRequest : IServiceRequest
    {
        public string SOAPAction
        {
            get
            {
                return null;
            }
        }

        [XmlIgnore]
        public string NazwaPliku { get; set; }
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
        public DocumentAnyType Document { get; set; }

        [XmlText]
        public string Zawartosc
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
                    new NazwaPlikuHeaderAttribute(this.NazwaPliku),
                    new DaneDodatkoweHeaderAttribute(this.DaneDodatkowe),
                    new CzyProbneHeaderAttribute(this.CzyProbne),
                    new AdresOdpowiedziHeaderAttribute(this.AdresOdpowiedzi),
                    new AdresSkrytkiHeaderAttribute(this.AdresSkrytki),
                    new IdentyfikatorPodmiotuHeaderAttribute(this.PodmiotNadawcy)
                };
            }
        }
    }

    public class DocumentAnyType
    {
        [XmlText]
        public string Zawartosc { get; set; }
    }
}
