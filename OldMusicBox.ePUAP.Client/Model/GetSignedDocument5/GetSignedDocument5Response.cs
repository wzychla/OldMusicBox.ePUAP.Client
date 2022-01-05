using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Signature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.GetSignedDocument
{
    /// <summary>
    /// GetSignedDocument Response
    /// </summary>
    [XmlRoot("getSignedDocumentReturn", Namespace = "")]
    public class GetSignedDocument5Response : IServiceResponse
    {
        [XmlText]
        public string Content { get; set; }

        [XmlIgnore]
        public EPSignature Signature
        {
            get
            {
                if (string.IsNullOrEmpty(Content))
                {
                    return null;
                }

                // first, decode the response
                var rawContent = Encoding.UTF8.GetString(Convert.FromBase64String(this.Content));

                // then read it
                var xml = new XmlDocument();
                xml.LoadXml(rawContent);

                // then find the user info
                var signatures = xml.GetElementsByTagName("EPSignature", Namespaces.PODPIS_ZAUFANY);
                if (signatures.Count > 0)
                {
                    var signature = signatures.Item(0);

                    var serializer = new XmlSerializer(typeof(EPSignature));
                    using (var reader = new StringReader(signature.OuterXml))
                    {
                        return serializer.Deserialize(reader) as EPSignature;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Checks if the three: given name, surname and PESEL are there
        /// </summary>
        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return
                    this.Signature != null &&
                    this.Signature.IsValid;
            }
        }
    }
}
