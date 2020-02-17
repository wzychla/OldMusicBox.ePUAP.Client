using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Convert raw response to model instance
        /// </summary>
        public static GetTpUserInfoResponse FromSOAP(string soapEnvelope)
        {
            if (string.IsNullOrEmpty(soapEnvelope))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(soapEnvelope);

                var serializer = new XmlSerializer(typeof(GetTpUserInfoResponse));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);
                nsManager.AddNamespace("ns2", Namespaces.USERINFO);

                var response = xml.SelectSingleNode("//ns2:getTpUserInfoResponse", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as GetTpUserInfoResponse;
                    }
                }

                return null;
            }
            catch ( Exception ex )
            {
                throw new ServiceClientException("Cannot deserialize GetTpUserInfoResponse", ex);
            }
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
