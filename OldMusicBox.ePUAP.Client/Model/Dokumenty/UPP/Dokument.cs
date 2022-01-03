using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.UPP
{
    /// <summary>
    /// Model dokumentu UPO zwracanego przez WSSkrytka::Nadaj jeśli skrytka odbiorcza jest skonfigurowana na tryb odbierania UPP
    /// </summary>
    [XmlRoot("Dokument", Namespace = Namespaces.CRD_UPO)]
    public class Dokument
    {

        [XmlElement("UPP", Namespace = Namespaces.CRD_UPO)]
        public UPP UPP { get; set; }

    }

    public class UPP
    {
        [XmlElement("IdentyfikatorPoswiadczenia", Namespace = Namespaces.CRD_UPO)]
        public string IdentyfikatorPoswiadczenia { get; set; }

        [XmlElement("Adresat", Namespace = Namespaces.CRD_UPO)]
        public Podmiot Adresat { get; set; }

        [XmlElement("Nadawca", Namespace = Namespaces.CRD_UPO)]
        public Podmiot Nadawca { get; set; }

        [XmlElement("DataDoreczenia", Namespace = Namespaces.CRD_UPO)]
        public DateTime DataDoreczenia { get; set; }

        [XmlElement("DataWytworzeniaPoswiadczenia", Namespace = Namespaces.CRD_UPO)]
        public DateTime DataWytworzeniaPoswiadczenia { get; set; }

        [XmlElement("IdentyfikatorDokumentu", Namespace = Namespaces.CRD_UPO)]
        public string IdentyfikatorDokumentu { get; set; }

        [XmlElement("InformacjaUzupelniajaca", Namespace = Namespaces.CRD_UPO)]
        public InformacjaUzupelniajaca[] InformacjaUzupelniajaca { get; set; }
    }

    public class Podmiot
    {
        [XmlElement("Nazwa", Namespace = Namespaces.CRD_UPO)]
        public string Nazwa { get; set; }

        [XmlElement("IdentyfikatorPodmiotu", Namespace = Namespaces.CRD_UPO)]
        public IdentyfikatorPodmiotu IdentyfikatorPodmiotu { get; set; }
    }

    public class IdentyfikatorPodmiotu
    {
        [XmlAttribute("TypIdentyfikatora", Namespace = Namespaces.CRD_UPO)]
        public string TypIdentyfikatora { get; set; }

        [XmlText]
        public string Wartosc { get; set; }
    }

    public class InformacjaUzupelniajaca
    {
        [XmlAttribute("TypInformacjiUzupelniajacej", Namespace = Namespaces.CRD_UPO)]
        public string TypInformacjiUzupelniajacej { get; set; }

        [XmlText]
        public string Wartosc { get; set; }
    }

}
