using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Headers
{
    public class TerminDoreczeniaHeaderAttribute : HeaderAttribute
    {
        public TerminDoreczeniaHeaderAttribute() { }
        public TerminDoreczeniaHeaderAttribute(DateTime value)
        {
            this._value = value;
        }

        private DateTime _value;

        [XmlText]
        public string Value
        {
            get
            {
                return _value.ToString("o"); // iso 8601
            }
            set
            {

            }
        }
    }
}
