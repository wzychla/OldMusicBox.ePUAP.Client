using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Common
{
    public class StatusModel
    {
        [XmlElement("kod", Namespace = "")]
        public string Kod { get; set; }

        [XmlElement("komunikat", Namespace = "")]
        public string Komunikat { get; set; }
    }
}
