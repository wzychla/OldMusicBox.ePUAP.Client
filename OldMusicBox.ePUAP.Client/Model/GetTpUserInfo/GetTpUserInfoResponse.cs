using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.GetTpUserInfo
{
    /// <summary>
    /// GetTpUserInfo Response
    /// </summary>
    [XmlRoot("getTpUserInfoResponse", Namespace = Constants.Namespaces.USERINFO)]
    public class GetTpUserInfoResponse
    {
        [XmlElement("getTpUserInfoReturn", Namespace = "")]
        public GetTpUserInfoReturn Return
        {
            get; set;
        }
    }

    public class GetTpUserInfoReturn
    {
        [XmlElement("accountEmailAddress")]
        public string AccountEmailAddress { get; set; }

        [XmlElement("claimedRole")]
        public string ClaimedRole { get; set; }
    }

    [XmlRoot("PodpisZP", Namespace = Namespaces.PPZP)]
    public class PodpisZP
    {
        [XmlElement("DaneZP", Namespace = Namespaces.PPZP)]
        public DaneZP Dane { get; set; }
    }

    [XmlRoot("DaneZP", Namespace = Namespaces.PPZP)]
    public class DaneZP
    {
        [XmlElement("DaneZPOsobyFizycznej", Namespace = Namespaces.PPZP)]
        public DaneZPOsobyFizycznej DaneOsobyFizycznej { get; set; }
    }

    public class DaneZPOsobyFizycznej
    {
        [XmlElement("Nazwisko", Namespace = Namespaces.PPZPOS)]
        public Nazwisko Nazwisko { get; set; }

        [XmlElement("Imie", Namespace = Namespaces.PPZPOS)]
        public string Imie { get; set; }

        [XmlElement("PESEL", Namespace = Namespaces.PPZPOS)]
        public string PESEL { get; set; }

        [XmlElement(ElementName = "IdKontaUzytkownikaEpuap", Namespace = Namespaces.PPZP)]
        public string IdKontaUzytkownikaEpuap { get; set; }
    }

    public class Nazwisko
    {
        [XmlAttribute("rodzajCzlonu", Namespace = Namespaces.PPZPOS)]
        public string RodzajCzlonu { get; set; }

        [XmlText()]
        public string Value { get; set; }
    }
}
