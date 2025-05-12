using System;
using OldMusicBox.ePUAP.Client.Core.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Core.Constants;

namespace OldMusicBox.ePUAP.Client.Core.Model.Pull
{
    /// <summary>
    /// PobierzNastepny request
    /// </summary>
    [XmlRoot("ZapytaniePullPobierz", Namespace = Namespaces.OBI)]
    public class PobierzNastepnyRequest : IServiceRequest
    {
        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return null;
            }
        }

        public string SOAPAction
        {
            get
            {
                return null;
            }
        }


        [XmlElement("podmiot", Namespace = "")]
        public string Podmiot { get; set; }

        [XmlElement("nazwaSkrytki", Namespace = "")]
        public string NazwaSkrytki { get; set; }

        [XmlElement("adresSkrytki", Namespace = "")]
        public string AdresSkrytki { get; set; }
    }
}
