using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Constants;

namespace OldMusicBox.ePUAP.Client.Model.VerifySignedDocument
{
    /// <summary>
    /// Verify signed document request
    /// </summary>
    [XmlRoot("verifySignedDocument", Namespace = Namespaces.COMARCH_SIGN)]
    public class VerifySignedDocument5Request : IServiceRequest
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
