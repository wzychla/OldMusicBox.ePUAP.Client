using OldMusicBox.ePUAP.Client.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// SignedXml with specific Id
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/5099156/malformed-reference-element-when-adding-a-reference-based-on-an-id-attribute-w
    /// </remarks>
    public class SignedXmlWithId : SignedXml
    {
        public SignedXmlWithId(XmlDocument xml) 
            : base(xml)
        {
        }

        public SignedXmlWithId(XmlElement xmlElement)
            : base(xmlElement)
        {
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // check to see if it's a standard ID reference
            XmlElement idElem = base.GetIdElement(doc, id);

            if (idElem == null)
            {
                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("wsu", Namespaces.WS_SEC_UTILITY);

                idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
            }

            return idElem;
        }
    }
}
