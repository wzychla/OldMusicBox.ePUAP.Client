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
    /// AddDocumentToSigning5 response
    /// </summary>
    [XmlRoot("addDocumentToSigningReturn", Namespace = "")]
    public class AddDocumentToSigning5Response : IServiceResponse
    {
        [XmlText]
        public string Url { get; set; }
    }
}
