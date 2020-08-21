using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Dokumenty
{
    [XmlRoot("Dokument", Namespace = Namespaces.WNIO_PODPISANYDOKUMENT)]
    public class Dokument
    {
        public Dokument()
        {
            this.Opis  = new OpisDokumentu();
            this.Dane  = new DaneDokumentu();
            this.Tresc = new TrescDokumentu();
        }
        
        [XmlElement("OpisDokumentu", Namespace = Namespaces.WNIO_PODPISANYDOKUMENT)]
        public OpisDokumentu Opis { get; set; }

        [XmlElement("DaneDokumentu", Namespace = Namespaces.WNIO_PODPISANYDOKUMENT)]
        public DaneDokumentu Dane { get; set; }

        [XmlElement("TrescDokumentu", Namespace = Namespaces.WNIO_PODPISANYDOKUMENT)]

        public TrescDokumentu Tresc { get; set; }
    }

    #region Opis

    public class OpisDokumentu
    {
        public OpisDokumentu()
        {
            this.Data = new Data();
        }

        [XmlElement("Data", Namespace = Namespaces.WNIO_META)]
        public Data Data { get; set; }
    }

    public class Data
    {
        public Data()
        {
            this.TypDaty = "stworzony";
            this.Czas    = new Czas();
        }

        [XmlAttribute("typDaty", Namespace = "")]
        public string TypDaty { get; set; }

        [XmlElement("Czas", Namespace = Namespaces.WNIO_META)]
        public Czas Czas { get; set; }
    }

    public class Czas
    {
        [XmlText(DataType = "dateTime")]
        public DateTime Wartosc { get; set; }
    }

    #endregion

    #region Dane

    public class DaneDokumentu
    {
        public DaneDokumentu()
        {
            this.Naglowek = new Naglowek();
            this.Data     = new Data();
        }

        [XmlElement("Naglowek", Namespace = Namespaces.WNIO_STR)]
        public Naglowek Naglowek { get; set; }

        [XmlElement("Data", Namespace = Namespaces.WNIO_META)]
        public Data Data { get; set; }
    }

    public class Naglowek
    {
        public Naglowek()
        {
            this.Nazwa = new NazwaDokumentu();
        }

        [XmlElement("NazwaDokumentu", Namespace = Namespaces.WNIO_STR)]
        public NazwaDokumentu Nazwa { get; set; }
    }

    public class NazwaDokumentu
    {
        [XmlIgnore]
        public string Wartosc { get; set; }

        [XmlText]
        public string Wartosc_Render
        {
            get
            {
                return string.Format("Podpisany plik, załącznik base64: {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Wartosc)));
            }
        }
    }

    #endregion

    #region Treść

    public class TrescDokumentu
    {
        public TrescDokumentu()
        {
            this.Kodowanie = "base64";
            this.Rodzaj    = "inny";
        }

        [XmlAttribute("format", Namespace = "")]
        public string Format { get; set; }

        [XmlAttribute("kodowanie", Namespace = "")]
        public string Kodowanie { get; set; }

        [XmlAttribute("rodzaj", Namespace = "")]
        public string Rodzaj { get; set; }

        [XmlElement("Zalaczniki", Namespace = Namespaces.WNIO_PODPISANYDOKUMENT)]
        public Zalacznik[] Zalaczniki { get; set; }
    }

    [XmlRoot("Zalacznik", Namespace = Namespaces.WNIO_PODPISANYDOKUMENT)]
    public class Zalacznik
    {
        public Zalacznik()
        {
            this.DaneZalacznika = new DaneZalacznika();
        }

        [XmlAttribute("format", Namespace = "")]
        public string Format { get; set; }

        [XmlAttribute("kodowanie", Namespace = "")]
        public string Kodowanie { get; set; }

        [XmlAttribute("nazwaPliku", Namespace = "")]
        public string NazwaPliku { get; set; }

        [XmlElement("DaneZalacznika", Namespace = Namespaces.WNIO_PODPISANYDOKUMENT)]
        public DaneZalacznika DaneZalacznika { get; set; }
    }

    public class DaneZalacznika
    {
        [XmlText]
        public byte[] Zawartosc { get; set; }
    }

    #endregion
}

