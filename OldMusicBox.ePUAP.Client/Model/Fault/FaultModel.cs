using OldMusicBox.ePUAP.Client.Constants;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Fault
{
    [XmlRoot("Fault", Namespace = Namespaces.SOAPENVELOPE)]
    public class FaultModel : IServiceResponse
    {
        [XmlElement("faultcode", Namespace = "")]
        public string FaultCode { get; set; }

        [XmlElement("faultstring", Namespace = "")]
        public string FaultString { get; set; }
    }
}
