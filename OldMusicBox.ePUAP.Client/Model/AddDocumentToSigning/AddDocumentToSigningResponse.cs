using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.AddDocumentToSigning
{
    /// <summary>
    /// AddDocumentToSigning response
    /// </summary>
    [XmlRoot("addDocumentToSigningResponse", Namespace = Namespaces.COMARCH_SIGN)]
    public class AddDocumentToSigningResponse : IServiceResponse
    {
        [XmlElement("addDocumentToSigningReturn", Namespace = "")]
        public AddDocumentToSigningReturn Return { get; set; }
    }

    public class AddDocumentToSigningReturn
    {
        [XmlText]
        public string Url { get; set; }
    }
}
