using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.GetSignedDocument
{
    /// <summary>
    /// GetSignedDocument Response Handler
    /// </summary>
    public class GetSignedDocumentResponseHandler
        : IServiceResponseHandler<GetSignedDocumentResponse>
    {
        public GetSignedDocumentResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            fault = null;

            if (string.IsNullOrEmpty(soapResponse))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var xd = new XmlDocument();
                xd.LoadXml(soapResponse);

                var nsManager  = new XmlNamespaceManager(xd.NameTable);
                nsManager.AddNamespace("ns1", Namespaces.COMARCH_SIGN);

                var response = xd.SelectSingleNode("//ns1:getSignedDocumentResponse", nsManager) as XmlElement;
                if (response != null)
                {
                    // tak zwraca TpSigning
                    var serializer = new XmlSerializer(typeof(GetSignedDocumentResponse));
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as GetSignedDocumentResponse;
                    }
                }

                return null;
            }
            catch ( Exception ex )
            {
                throw new ServiceClientException("Cannot deserialize GetSignedDocument", ex);
            }
        }
    }
}
