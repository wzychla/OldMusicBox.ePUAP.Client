using OldMusicBox.ePUAP.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Constants;

namespace OldMusicBox.ePUAP.Tests.Model
{
    [XmlRoot("ExampleRequest", Namespace = Namespaces.COMARCH_SIGN)]
    public class ExampleRequest : IServiceRequest
    {
        [XmlElement("Id", Namespace = "")]
        public string Id { get; set; }

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
                return "exampleRequest";
            }
        }
    }
}
