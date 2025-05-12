using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Request;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.FileRepoService
{
    /// <summary>
    /// downloadFile Request
    /// </summary>
    [XmlRoot("DownloadFileParam", Namespace = Namespaces.FILEREPOCORE)]
    public class DownloadFileRequest : IServiceRequest
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

        [XmlElement("fileId", Namespace = "")]
        public string FileId { get; set; }

        [XmlElement("subject", Namespace = "")]
        public string Subject { get; set; }
    }
}
