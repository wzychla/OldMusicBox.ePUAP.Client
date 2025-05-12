using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Core.XAdES
{
    /// <summary>
    /// Signed XML with multiple cross references
    /// </summary>
    public class XAdESSignedXml : SignedXml
    {
        public XAdESSignedXml(XmlDocument document) : base(document) { }

        private List<XmlNode> _additionalNodes = new List<XmlNode>();

        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            if (string.IsNullOrEmpty(idValue))
                return null;

            var element = base.GetIdElement(document, idValue);
            if (element != null)
                return element;
     
            var node = FindNodeRecursive( this._additionalNodes, "Id", idValue);
            if (node != null)
                return node;

            return null;
        }

        private XmlElement FindNodeRecursive( IEnumerable<XmlNode>nodes, string AttributeName, string AttributeValue)
        {
            if (nodes == null) return null;

            foreach (XmlNode node in nodes)
            {
                // attributes
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if ( string.Equals( attribute.Name, AttributeName, StringComparison.InvariantCultureIgnoreCase ) &&
                         string.Equals( attribute.Value, AttributeValue, StringComparison.InvariantCultureIgnoreCase )
                        )
                    {
                        return node as XmlElement;
                    }
                }
                // recursion
                var result = FindNodeRecursive(node.ChildNodes.OfType<XmlNode>(), AttributeName, AttributeValue);
                if (result != null)
                {
                    return result;
                }

            }

            return null;
        }

        public void RegisterObject( DataObject dataObject )
        {
            base.AddObject(dataObject);
            this._additionalNodes.AddRange(dataObject.Data.OfType<XmlNode>());
        }
    }
}
