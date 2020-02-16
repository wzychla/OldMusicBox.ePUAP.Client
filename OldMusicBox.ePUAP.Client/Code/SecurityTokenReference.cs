using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// WS-Security compliant KeyInfoClause 
    /// </summary>
    public class SecurityTokenReference : KeyInfoClause
    {
        private static Random _random = new Random();

        public string Reference { get; set; }
        public string ValueType { get; set; }

        public override XmlElement GetXml()
        {
            var _xml = string.Format(
                @"<wsse:SecurityTokenReference xmlns:wsse=""{0}"" xmlns:wsu=""{1}"" wsu:Id=""{2}"">" +
                @"<wsse:Reference URI=""{3}"" ValueType=""{4}""/>" +
                "</wsse:SecurityTokenReference>",
                Namespaces.WS_SEC_EXT, Namespaces.WS_SEC_UTILITY,
                _random.Next(),
                this.Reference, this.ValueType);

            var xml = new XmlDocument();
            xml.LoadXml(_xml);

            return xml.DocumentElement;
        }

        public override void LoadXml(XmlElement element)
        {

        }
    }
}
