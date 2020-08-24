using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Dokumenty
{
    [XmlRoot("Dokument", Namespace = Namespaces.CRD_WNIO)]
    public class Dokument
    {
        public Dokument()
        {
            this.Opis  = new OpisDokumentu();
            this.Dane  = new DaneDokumentu();
            this.Tresc = new TrescDokumentu();
        }
        
        [XmlElement("OpisDokumentu", Namespace = Namespaces.CRD_WNIO)]
        public OpisDokumentu Opis { get; set; }

        [XmlElement("DaneDokumentu", Namespace = Namespaces.CRD_WNIO)]
        public DaneDokumentu Dane { get; set; }

        [XmlElement("TrescDokumentu", Namespace = Namespaces.CRD_WNIO)]

        public TrescDokumentu Tresc { get; set; }
    }

    #region Opis

    public class OpisDokumentu
    {
        public OpisDokumentu()
        {
            this.Data = new Data();
        }

        [XmlElement("Data", Namespace = Namespaces.CRD_META)]
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

        [XmlElement("Czas", Namespace = Namespaces.CRD_META)]
        public Czas Czas { get; set; }
    }

    public class Czas
    {
        [XmlText]
        public string Wartosc { get; set; }
    }

    #endregion

    #region Dane

    public class DaneDokumentu
    {
        public DaneDokumentu()
        {
            this.Naglowek = new Naglowek();
            this.Data     = new Data();
            this.Adresaci = new Adresaci();
            this.Nadawcy  = new Nadawcy();
        }

        [XmlElement("Naglowek", Namespace = Namespaces.CRD_STR)]
        public Naglowek Naglowek { get; set; }

        [XmlElement("Data", Namespace = Namespaces.CRD_META)]
        public Data Data { get; set; }

        [XmlElement("Adresaci", Namespace = Namespaces.CRD_STR)]
        public Adresaci Adresaci { get; set; }

        [XmlElement("Nadawcy", Namespace = Namespaces.CRD_STR)]
        public Nadawcy Nadawcy { get; set; }
    }

    public class Naglowek
    {
        public Naglowek()
        {
            this.Nazwa = new NazwaDokumentu();
        }

        [XmlElement("NazwaDokumentu", Namespace = Namespaces.CRD_STR)]
        public NazwaDokumentu Nazwa { get; set; }
    }

    public class NazwaDokumentu
    {
        [XmlText]
        public string Wartosc { get; set; }
    }

    public class Adresaci
    {
        public Adresaci()
        {
            this.Podmiot = new Podmiot();
        }

        [XmlElement("Podmiot", Namespace = Namespaces.CRD_META)]
        public Podmiot Podmiot { get; set; }
    }

    public class Nadawcy
    {
        public Nadawcy()
        {
            this.Podmiot = new Podmiot();
        }

        [XmlElement("Podmiot", Namespace = Namespaces.CRD_META)]
        public Podmiot Podmiot { get; set; }
    }

    public class Podmiot
    {
        [XmlElement("Osoba", Namespace = Namespaces.CRD_OSO)]
        public Osoba Osoba { get; set; }

        [XmlElement("Instytucja", Namespace = Namespaces.CRD_INST)]
        public Instytucja Instytucja { get; set; }
    }

    public class Osoba
    {
        [XmlElement("Imie", Namespace = Namespaces.CRD_OSO)]
        public string Imie { get; set; }

        [XmlElement("Nazwisko", Namespace = Namespaces.CRD_OSO)]
        public string Nazwisko { get; set; }

        [XmlElement("Adres", Namespace = Namespaces.CRD_ADR)]
        public Adres Adres { get; set; }
    }

    public class Instytucja
    {
        public Instytucja()
        {
            this.Adres = new Adres();
        }

        [XmlElement("NazwaInstytucji", Namespace = Namespaces.CRD_INST)]
        public string NazwaInstytucji { get; set; }

        [XmlElement("Adres", Namespace = Namespaces.CRD_ADR)]
        public Adres Adres { get; set; }
    }

    public class Adres
    {
        [XmlElement("KodPocztowy", Namespace = Namespaces.CRD_ADR)]
        public string KodPocztowy { get; set; }
        [XmlElement("Poczta", Namespace = Namespaces.CRD_ADR)]
        public string Poczta { get; set; }
        [XmlElement("Miejscowosc", Namespace = Namespaces.CRD_ADR)]
        public string Miejscowosc { get; set; }
        [XmlElement("Ulica", Namespace = Namespaces.CRD_ADR)]
        public string Ulica { get; set; }
        [XmlElement("Budynek", Namespace = Namespaces.CRD_ADR)]
        public string Budynek { get; set; }
        [XmlElement("Lokal", Namespace = Namespaces.CRD_ADR)]
        public string Lokal { get; set; }
    }   

    #endregion

    #region Treść

    public class TrescDokumentu
    {
        public TrescDokumentu()
        {
            this.Kodowanie = "XML";
            this.Format    = "text/xml";

            this.RodzajWnioskuRozszerzony = new RodzajWnioskuRozszerzony();
        }

        [XmlAttribute("format", Namespace = "")]
        public string Format { get; set; }

        [XmlAttribute("kodowanie", Namespace = "")]
        public string Kodowanie { get; set; }

        [XmlAttribute("rodzaj", Namespace = "")]
        public string Rodzaj { get; set; }

        [XmlElement("MiejscowoscDokumentu", Namespace = Namespaces.CRD_WNIO )]
        public string MiejscowoscDokumentu { get; set; }

        [XmlElement("Tytul", Namespace = Namespaces.CRD_WNIO)]
        public string Tytul { get; set; }

        [XmlElement("RodzajWnioskuRozszerzony", Namespace = Namespaces.CRD_WNIO)]
        public RodzajWnioskuRozszerzony RodzajWnioskuRozszerzony { get; set; }

        [XmlArray("Zalaczniki", Namespace = Namespaces.CRD_STR)]
        public Zalacznik[] Zalaczniki { get; set; }
    }

    public class RodzajWnioskuRozszerzony
    {
        [XmlAttribute("jakisInny", Namespace = "")]
        public string JakisInny { get; set; }
        [XmlAttribute("rodzaj", Namespace = "")]
        public string Rodzaj { get; set; }
    }

    [XmlRoot("Zalacznik", Namespace = Namespaces.CRD_STR)]
    public class Zalacznik
    {
        public Zalacznik()
        {
            this.Kodowanie = "base64";
            this.DaneZalacznika = new DaneZalacznika();
        }

        [XmlAttribute("format", Namespace = "")]
        public string Format { get; set; }

        [XmlAttribute("kodowanie", Namespace = "")]
        public string Kodowanie { get; set; }

        [XmlAttribute("nazwaPliku", Namespace = "")]
        public string NazwaPliku { get; set; }

        [XmlElement("DaneZalacznika", Namespace = Namespaces.CRD_STR)]
        public DaneZalacznika DaneZalacznika { get; set; }
    }

    public class DaneZalacznika
    {
        [XmlText]
        public byte[] Zawartosc { get; set; }
    }

    #endregion
}

