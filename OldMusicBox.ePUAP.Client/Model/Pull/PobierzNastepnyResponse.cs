using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Pull
{
    /// <summary>
    /// PobierzNastepny response
    /// </summary>
    [XmlRoot("OdpowiedzPullPobierz", Namespace = Namespaces.OBI)]
    public class PobierzNastepnyResponse : IServiceResponse
    {
        [XmlElement("status", Namespace = "")]
        public StatusModel Status { get; set; }

        [XmlElement("danePodmiotu", Namespace = "")]
        public DanePodmiotu DanePodmiotu { get; set; }

        [XmlElement("daneNadawcy", Namespace = "")]
        public DaneNadawcy DaneNadawcy { get; set; }

        [XmlElement("dataNadania", Namespace = "")]
        public DateTime DataNadania { get; set; }

        [XmlElement("nazwaSkrytki", Namespace = "")]
        public string NazwaSkrytki { get; set; }

        [XmlElement("adresSkrytki", Namespace = "")]
        public string AdresSkrytki { get; set; }

        [XmlElement("adresOdpowiedzi", Namespace = "")]
        public string AdresOdpowiedzi { get; set; }

        [XmlElement("czyTestowe", Namespace = "")]
        public bool CzyTestowe { get; set; }

        [XmlElement("daneDodatkowe", Namespace = "")]
        public byte[] DaneDodatkowe { get; set; }

        [XmlElement("dokument", Namespace = "")]
        public Dokument Dokument { get; set; }
    }

    public class DanePodmiotu
    {
        [XmlElement("identyfikator", Namespace = "")]
        public string Identyfikator { get; set; }

        [XmlElement("typOsoby", Namespace = "")]
        public string TypOsoby { get; set; }

        [XmlElement("imieSkrot", Namespace = "")]
        public string ImieSkrot { get; set; }

        [XmlElement("nazwiskoNazwa", Namespace = "")]
        public string NazwiskoNazwa { get; set; }

        [XmlElement("nip", Namespace = "")]
        public string Nip { get; set; }

        [XmlElement("pesel", Namespace = "")]
        public string PESEL { get; set; }

        [XmlElement("regon", Namespace = "")]
        public string REGON { get; set; }

        [XmlElement("zgoda", Namespace = "")]
        public bool Zgoda { get; set; }
    }

    public class DaneNadawcy
    {
        [XmlElement("uzytkownik", Namespace = "")]
        public string Uzytkownik { get; set; }

        [XmlElement("system", Namespace = "")]
        public string System { get; set; }
    }

    public class Dokument
    {
        [XmlElement("nazwaPliku", Namespace = "")]
        public string NazwaPliku { get; set; }
        [XmlElement("typPliku", Namespace = "")]
        public string TypPliku { get; set; }
        [XmlElement("zawartosc", Namespace = "")]
        public byte[] Zawartosc { get; set; }
    }
}
