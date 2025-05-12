using OldMusicBox.ePUAP.Client.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Headers
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
}
