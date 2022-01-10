using OldMusicBox.ePUAP.Client.Constants;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.FileRepoService
{
    /// <summary>
    /// downloadFile response
    /// </summary>    
    [XmlRoot("DataDocument", Namespace = Namespaces.FILEREPOCORE)]
    [DataContract(Name = "DataDocument", Namespace = "")]
    public class DownloadFileResponse : IServiceResponse
    {
        [XmlElement("file", Namespace = "")]
        [DataMember(Name = "file")]
        public byte[] File { get; set; }

        [XmlElement("filename", Namespace = "")]
        [DataMember(Name = "filename")]
        public string Filename { get; set; }

        [XmlElement("mimeType", Namespace = "")]
        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }

        [XmlElement("encoding", Namespace = "")]
        [DataMember(Name = "encoding")]
        public string Encoding { get; set; }
    }

    [DataContract(Name = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [DataMember(Name = "Body")]
        public Body Body { get; set; }
    }

    [DataContract(Name = "Body", Namespace = Namespaces.FILEREPOCORE)]
    public class Body
    {
        [DataMember(Name = "DataDocument")]
        public DownloadFileResponse DownloadFileResponse { get; set; }
    }
}
