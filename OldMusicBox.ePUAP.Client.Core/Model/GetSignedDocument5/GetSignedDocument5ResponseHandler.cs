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
    public class GetSignedDocument5ResponseHandler
        : IServiceResponseHandler<GetSignedDocument5Response>
    {
        public GetSignedDocument5Response FromSOAP(string soapResponse, out FaultModel fault)
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

                // tak zwraca TpSigning5
                var response = xd.SelectSingleNode("//getSignedDocumentReturn") as XmlElement;
                if (response != null)
                {
                    var serializer = new XmlSerializer(typeof(GetSignedDocument5Response));

                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as GetSignedDocument5Response;
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
