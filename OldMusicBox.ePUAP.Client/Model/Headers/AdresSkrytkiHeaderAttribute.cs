using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Headers
{
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
}
