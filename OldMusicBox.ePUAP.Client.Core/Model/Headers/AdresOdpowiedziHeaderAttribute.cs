using OldMusicBox.ePUAP.Client.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Headers
{
    public class AdresOdpowiedziHeaderAttribute : HeaderAttribute
    {
        public AdresOdpowiedziHeaderAttribute() { }
        public AdresOdpowiedziHeaderAttribute(string value)
        {
            this.Value = value;
        }

        [XmlText]
        public string Value { get; set; }
    }
}
