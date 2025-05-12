using OldMusicBox.ePUAP.Client.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Headers
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
