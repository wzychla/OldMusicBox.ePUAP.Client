using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.VerifySignedDocument
{
    /// <summary>
    /// VerifySignedDocument response handler
    /// </summary>
    public class VerifySignedDocumentResponseHandler
         : IServiceResponseHandler<VerifySignedDocumentResponse>
    {
        public VerifySignedDocumentResponse FromSOAP(string soapResponse)
        {
            if (string.IsNullOrEmpty(soapResponse))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var xd = new XmlDocument();
                xd.LoadXml(soapResponse);

                var serializer = new XmlSerializer(typeof(VerifySignedDocumentResponse));
                var manager    = new XmlNamespaceManager(xd.NameTable);
                manager.AddNamespace("ns1", Namespaces.COMARCH_SIGN);

                var response = xd.SelectSingleNode("//ns1:verifySignedDocumentResponse", manager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as VerifySignedDocumentResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ServiceClientException("Cannot deserialize VerifySignedDocument", ex);
            }
        }
    }
}
