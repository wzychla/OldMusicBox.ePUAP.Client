using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.GetSignedDocument
{
    /// <summary>
    /// GetSignedDocument Request
    /// </summary>
    [XmlRoot("getSignedDocument", Namespace = Namespaces.COMARCH_SIGN)]
    public class GetSignedDocumentRequest : IServiceRequest
    {
        public string SOAPAction
        {
            get
            {
                return "getSignedDocument";
            }
        }

        [XmlElement("id", Namespace = "")]
        public string Id { get; set; }

        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return null;
            }
        }
    }
}
