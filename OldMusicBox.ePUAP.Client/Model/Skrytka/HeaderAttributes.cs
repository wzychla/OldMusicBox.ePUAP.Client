using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Skrytka
{
    public class DaneDodatkoweHeaderAttribute : HeaderAttribute
    {
        public DaneDodatkoweHeaderAttribute() { }
        public DaneDodatkoweHeaderAttribute(byte[] value)
        {
            this._value = value;
        }

        private byte[] _value;

        [XmlText]
        public string Value
        {
            get
            {
                if (this._value != null)
                {
                    return Convert.ToBase64String(this._value);
                }
                else
                {
                    return null;
                }
            }
            set
            {

            }
        }
    }

    public class CzyProbneHeaderAttribute : HeaderAttribute
    {
        public CzyProbneHeaderAttribute() { }
        public CzyProbneHeaderAttribute(bool value)
        {
            this._value = value ? 1 : 0;
        }

        private int _value;

        [XmlText]
        public string Value
        {
            get
            {
                return _value.ToString();
            }
            set
            {

            }
        }
    }

    public class AdresOdpowiedziHeaderAttribute : HeaderAttribute
    {
        public AdresOdpowiedziHeaderAttribute() { }
        public AdresOdpowiedziHeaderAttribute( string value )
        {
            this.Value = value;
        }

        [XmlText]
        public string Value { get; set; }
    }

    public class AdresSkrytkiHeaderAttribute : HeaderAttribute
    {
        public AdresSkrytkiHeaderAttribute() { }
        public AdresSkrytkiHeaderAttribute(string value)
        {
            this.Value = value;
        }

        [XmlText]
        public string Value { get; set; }
    }

    public class IdentyfikatorPodmiotuHeaderAttribute : HeaderAttribute
    {
        public IdentyfikatorPodmiotuHeaderAttribute() { }
        public IdentyfikatorPodmiotuHeaderAttribute(string value)
        {
            this.Value = value;
        }

        [XmlText]
        public string Value { get; set; }
    }
}
