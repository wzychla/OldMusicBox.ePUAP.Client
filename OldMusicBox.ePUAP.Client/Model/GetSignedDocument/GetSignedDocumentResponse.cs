using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.GetSignedDocument
{
    /// <summary>
    /// GetSignedDocument Response
    /// </summary>
    [XmlRoot("getSignedDocumentResponse", Namespace = Namespaces.COMARCH_SIGN)]
    public class GetSignedDocumentResponse : IServiceResponse
    {
        [XmlElement("getSignedDocumentReturn", Namespace = "")]
        public GetSignedDocumentReturn Return { get; set; }

        /// <summary>
        /// LoD wrapper over the inner information
        /// </summary>
        [XmlIgnore]
        public GetTpUserInfo.PodpisZP Podpis
        {
            get
            {
                if (this.Return != null && this.Return.IsValid)
                {
                    return this.Return.PodpisZP;
                }
                else
                {
                    return null;
                }
            }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return
                    this.Podpis != null &&
                    this.Podpis.IsValid;
            }
        }

    }

    public class GetSignedDocumentReturn
    {
        [XmlText]
        public string Content { get; set; }

        [XmlIgnore]
        public GetTpUserInfo.PodpisZP PodpisZP
        {
            get
            {
                if (string.IsNullOrEmpty(Content))
                {
                    return null;
                }

                // first, decode the response
                var rawContent = Encoding.UTF8.GetString(Convert.FromBase64String(this.Content));

                // then read it
                var xml = new XmlDocument();
                xml.LoadXml(rawContent);

                // then find the user info
                var podpisZPs = xml.GetElementsByTagName("PodpisZP", Namespaces.PPZP);
                if (podpisZPs.Count > 0)
                {
                    var podpisZP = podpisZPs.Item(0);

                    var serializer = new XmlSerializer(typeof(GetTpUserInfo.PodpisZP));
                    using (var reader = new StringReader(podpisZP.OuterXml))
                    {
                        return serializer.Deserialize(reader) as GetTpUserInfo.PodpisZP;
                    }
                }
                else
                {
                    return null;
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
}
