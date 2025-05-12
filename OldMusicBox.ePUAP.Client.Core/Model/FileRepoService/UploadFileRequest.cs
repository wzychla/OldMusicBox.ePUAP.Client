using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Request;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.FileRepoService
{
    /// <summary>
    /// uploadFile Request
    /// </summary>
    [XmlRoot("UploadFileParam", Namespace = Namespaces.FILEREPOCORE)]
    public class UploadFileRequest : IServiceRequest
    {
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
                return null;
            }
        }


        [XmlElement("file", Namespace = "")]
        public string File { get; set; }

        [XmlElement("filename", Namespace = "")]
        public string Filename { get; set; }

        [XmlElement("mimeType", Namespace = "")]
        public string MimeType { get; set; }

        [XmlElement("subject", Namespace = "")]
        public string Subject { get; set; }


    }
}
