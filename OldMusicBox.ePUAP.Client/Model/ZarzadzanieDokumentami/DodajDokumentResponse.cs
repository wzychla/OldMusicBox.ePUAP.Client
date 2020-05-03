using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.ZarzadzanieDokumentami
{
    /// <summary>
    /// DodajDokument response
    /// </summary>
    [XmlRoot("dodajDokumentResponse", Namespace = Namespaces.ZARZADZANIEDOKUMENTAMI)]
    public class DodajDokumentResponse : IServiceResponse
    {
        [XmlElement("dodajDokumentResult", Namespace = "")]
        public int Result { get; set; }
    }    
}
