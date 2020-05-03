using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Constants;

namespace OldMusicBox.ePUAP.Client.Model.ZarzadzanieDokumentami
{
    /// <summary>
    /// DodajDokument request
    /// </summary>
    [XmlRoot("dodajDokument", Namespace = Namespaces.ZARZADZANIEDOKUMENTAMI)]
    public class DodajDokumentRequest : IServiceRequest
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

        [XmlElement("sklad", Namespace = "")]
        public Sklad Sklad { get; set; }

        [XmlElement("dokument", Namespace = "")]
        public Dokument Dokument { get; set; }
    }

    public class Sklad
    {
        [XmlElement("nazwa", Namespace = "")]
        public string Nazwa { get; set; }

        [XmlElement("podmiot", Namespace = "")]
        public string Podmiot { get; set; }
    }

    public class Dokument
    {
        [XmlElement("szczegolyDokumentu", Namespace ="")]
        public SzczegolyDokumentu SzczegolyDokumentu { get; set; }

        [XmlElement("tresc", Namespace = "")]
        public byte[] Tresc { get; set; }
    }

    public class SzczegolyDokumentu
    {
        [XmlElement("id", Namespace = "", IsNullable = true)]
        public int? Id { get; set; }

        [XmlElement("nazwa", Namespace = "")]
        public string Nazwa { get; set; }

        [XmlElement("metadane", Namespace = "", IsNullable = true)]
        public byte[] Metadane { get; set; }

        [XmlElement("idUPO", Namespace = "", IsNullable = true)]
        public int? IdUPO { get; set; }

        [XmlElement("nadawca", Namespace = "", IsNullable = true)]
        public NadawcaOdbiorca Nadawca { get; set; }

        [XmlElement("adresat", Namespace = "", IsNullable = true)]
        public NadawcaOdbiorca Adresat { get; set; }

        [XmlElement("dataNadania", Namespace = "", IsNullable = true)]
        public DateTime? DataNadania { get; set; }

        [XmlElement("dataOdebrania", Namespace = "", IsNullable = true)]
        public DateTime? DataOdebrania { get; set; }

        [XmlElement("dataUtworzenia", Namespace = "", IsNullable = true)]
        public DateTime? DataUtworzenia { get; set; }

        [XmlElement("formularz", Namespace = "", IsNullable = true)]
        public Formularz Formularz { get; set; }

        [XmlElement("folder", Namespace = "", IsNullable = true)]
        public string Folder { get; set; }
    }

    public class NadawcaOdbiorca
    {
        [XmlElement("nazwa", Namespace = "")]
        public string Nazwa { get; set; }
        [XmlElement("adres", Namespace = "")]
        public string Adres { get; set; }
    }

    public class Formularz
    {
        [XmlElement("podmiot", Namespace = "")]
        public string Podmiot { get; set; }

        [XmlElement("nazwa", Namespace = "")]
        public string Nazwa { get; set; }
    }
}
