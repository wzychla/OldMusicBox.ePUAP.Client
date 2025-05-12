using OldMusicBox.ePUAP.Client.Core.Constants;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.FileRepoService
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

