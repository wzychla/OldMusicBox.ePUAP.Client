using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.GetSignedDocument
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
    }
}
