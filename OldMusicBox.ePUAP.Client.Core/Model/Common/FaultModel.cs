using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Common
{
    [XmlRoot("Fault", Namespace = Namespaces.SOAPENVELOPE)]
    public class FaultModel : IServiceResponse
    {
        [XmlElement("faultcode", Namespace = "")]
        public string FaultCode { get; set; }

        [XmlElement("faultstring", Namespace = "")]
        public string FaultString { get; set; }

        [XmlElement("detail", Namespace = "")]
        public FaultDetail Detail { get; set; }
    }

    public class FaultDetail
    {
        [XmlElement("Wyjatek", Namespace = Namespaces.OBI)]
        public FaultException Wyjatek { get; set; }
    }

    public class FaultException
    {
        [XmlElement("kod", Namespace = "")]
        public string Kod { get; set; }

        [XmlElement("komunikat", Namespace = "")]
        public string Komunikat { get; set; }
    }
}
