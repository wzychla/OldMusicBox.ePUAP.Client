using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Headers
{
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
}
