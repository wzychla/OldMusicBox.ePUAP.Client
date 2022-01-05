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

namespace OldMusicBox.ePUAP.Client.Model.VerifySignedDocument
{
    /// <summary>
    /// VerifySignedDocument response handler
    /// </summary>
    public class VerifySignedDocument5ResponseHandler
         : IServiceResponseHandler<VerifySignedDocument5Response>
    {
        public VerifySignedDocument5Response FromSOAP(string soapResponse, out FaultModel fault)
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

                var manager    = new XmlNamespaceManager(xd.NameTable);
                manager.AddNamespace("ns1", Namespaces.COMARCH_SIGN);

                // tak zwraca TpSigning5
                var response = xd.SelectSingleNode("//verifySignedDocumentReturn") as XmlElement;
                if (response != null)
                {
                    var serializer = new XmlSerializer(typeof(VerifySignedDocument5Response));

                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as VerifySignedDocument5Response;
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
