using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.AddDocumentToSigning
{
    /// <summary>
    /// AddDocumentToSigning response handler
    /// </summary>
    public class AddDocumentToSigningResponseHandler
        : IServiceResponseHandler<AddDocumentToSigningResponse>
    {
        public AddDocumentToSigningResponse FromSOAP(string soapResponse, out FaultModel fault)
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

                var serializer = new XmlSerializer(typeof(AddDocumentToSigningResponse));
                var nsManager  = new XmlNamespaceManager(xd.NameTable);
                nsManager.AddNamespace("ns1", Namespaces.COMARCH_SIGN);

                var response = xd.SelectSingleNode("//ns1:addDocumentToSigningResponse", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as AddDocumentToSigningResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ServiceClientException("Cannot deserialize AddDocumentToSigning", ex);
            }
        }
    }
}
