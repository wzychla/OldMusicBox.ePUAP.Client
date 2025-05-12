using OldMusicBox.ePUAP.Client.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.AddDocumentToSigning
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

    [XmlRoot("addDocumentToSigningReturn", Namespace = "")]
    public class AddDocumentToSigningReturn
    {
        [XmlText]
        public string Url { get; set; }
    }
}
