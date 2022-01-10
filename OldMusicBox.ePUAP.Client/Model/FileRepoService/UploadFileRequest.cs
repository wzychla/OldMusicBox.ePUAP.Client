using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.FileRepoService
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
