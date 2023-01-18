using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldMusicBox.ePUAP.Client.Request;
using System.Xml.Serialization;
using OldMusicBox.ePUAP.Client.Constants;

namespace OldMusicBox.ePUAP.Client.Model.ObslugaUPP
{
    /// <summary>
    /// DajUPP request
    /// </summary>
    [XmlRoot( "ZapytanieDajUpp", Namespace = Namespaces.OBI )]
    public class DajUPPRequest : IServiceRequest
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

        [XmlElement( "podmiot", Namespace = "" )]
        public string Podmiot { get; set; }

        [XmlElement( "nadawca", Namespace = "" )]
        public UzytkownikType Nadawca { get; set; }

        [XmlElement( "dokument", Namespace = "" )]
        public DocumentType Dokument { get; set; }
    }

    public class UzytkownikType
    {
        public const string EPUAP_ID = "ePUAP-ID";

        [XmlElement( "identyfikator", Namespace = "" )]
        public string Identyfikator { get; set; }

        [XmlElement( "typIdentyfikatora", Namespace = "" )]
        public string TypIdentyfikatora { get; set; }

        [XmlElement( "nazwa", Namespace = "" )]
        public string Nazwa { get; set; }
    }

    public class DocumentType
    {
        [XmlElement( "nazwaPliku", Namespace = "" )]
        public string NazwaPliku { get; set; }

        [XmlElement( "typPliku", Namespace = "" )]
        public string TypPliku { get; set; }

        [XmlElement( "zawartosc", Namespace = "" )]
        public byte[] Zawartosc { get; set; }
    }
}
