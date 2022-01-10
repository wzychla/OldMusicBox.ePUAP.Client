using OldMusicBox.ePUAP.Client.Constants;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.FileRepoService
{
    /// <summary>
    /// uploadFile response
    /// </summary>    
    [XmlRoot("fileId5", Namespace = Namespaces.FILEREPOCORE)]
    public class UploadFileResponse : IServiceResponse
    {
        [XmlText()]
        public string FileId5 { get; set; }

    }
}

