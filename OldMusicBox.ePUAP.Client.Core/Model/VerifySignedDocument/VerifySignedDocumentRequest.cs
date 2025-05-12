using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldMusicBox.ePUAP.Client.Core.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Core.Constants;

namespace OldMusicBox.ePUAP.Client.Core.Model.VerifySignedDocument
{
    /// <summary>
    /// Verify signed document request
    /// </summary>
    [XmlRoot("verifySignedDocument", Namespace = Namespaces.COMARCH_SIGN)]
    public class VerifySignedDocumentRequest : IServiceRequest
    {
        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return null;
            }
        }

        [XmlElement(ElementName = "document", Namespace = "")]
        public byte[] Document { get; set; }


        public string SOAPAction
        {
            get
            {
                return "verifySignedDocument";
            }
        }
    }
}
