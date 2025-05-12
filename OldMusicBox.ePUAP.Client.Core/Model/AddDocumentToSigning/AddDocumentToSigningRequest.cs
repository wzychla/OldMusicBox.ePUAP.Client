using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.AddDocumentToSigning
{
    /// <summary>
    /// Add document to signing request
    /// </summary>
    [XmlRoot("addDocumentToSigning", Namespace = Namespaces.COMARCH_SIGN)]
    public class AddDocumentToSigningRequest : IServiceRequest
    {
        [XmlIgnore]
        public string SOAPAction
        {
            get
            {
                return "addDocumentToSigning";
            }
        }

        [XmlElement(ElementName = "doc", Namespace = "")]
        public byte[] Doc { get; set; }

        [XmlElement(ElementName = "successURL", Namespace = "")]
        public string SuccessUrl { get; set; }

        [XmlElement(ElementName = "failureURL", Namespace = "")]
        public string FailureUrl { get; set; }

        [XmlElement(ElementName = "additionalInfo", Namespace = "")]
        public string AdditionalInfo { get; set; }

        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return null;
            }
        }
    }
}
