using OldMusicBox.ePUAP.Client.Constants;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.GetTpUserInfo
{
    /// <summary>
    /// GetTpUserInfo Response
    /// </summary>
    [XmlRoot("getTpUserInfoResponse", Namespace = Constants.Namespaces.USERINFO)]
    public class GetTpUserInfoResponse : IServiceResponse
    {
        [XmlElement("getTpUserInfoReturn", Namespace = "")]
        public GetTpUserInfoReturn Return
        {
            get; set;
        }

        /// <summary>
        /// LoD wrapper over the inner information
        /// </summary>
        [XmlIgnore]
        public PodpisZP Podpis
        {
            get
            {
                if ( this.Return != null && this.Return.IsValid )
                {
                    return this.Return.PodpisZP;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class GetTpUserInfoReturn
    {
        [XmlElement("accountEmailAddress")]
        public string AccountEmailAddress { get; set; }

        [XmlElement("claimedRole")]
        public string ClaimedRole { get; set; }

        [XmlIgnore]
        public PodpisZP PodpisZP
        {
            get
            {
                if ( string.IsNullOrEmpty( ClaimedRole ) )
                {
                    return null;
                }

                var serializer = new XmlSerializer(typeof(PodpisZP));
                using (var reader = new StringReader(this.ClaimedRole))
                {
                    return serializer.Deserialize(reader) as PodpisZP;
                }
            }
        }

        /// <summary>
        /// Checks if the three: given name, surname and PESEL are there
        /// </summary>
        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return
                    this.PodpisZP != null &&
                    this.PodpisZP.Dane != null &&
                    this.PodpisZP.Dane.DaneOsobyFizycznej != null &&
                    this.PodpisZP.Dane.DaneOsobyFizycznej.Nazwisko != null &&
                    !string.IsNullOrEmpty(this.PodpisZP.Dane.DaneOsobyFizycznej.Imie) &&
                    !string.IsNullOrEmpty(this.PodpisZP.Dane.DaneOsobyFizycznej.Nazwisko.Value) &&
                    !string.IsNullOrEmpty(this.PodpisZP.Dane.DaneOsobyFizycznej.PESEL);
            }
        }
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
