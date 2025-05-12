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

namespace OldMusicBox.ePUAP.Client.Core.Model.GetTpUserInfo
{
    /// <summary>
    /// GetTpUserInfo Response Handler
    /// </summary>
    public class GetTpUserInfoResponseHandler
        : IServiceResponseHandler<GetTpUserInfoResponse>
    {
        public GetTpUserInfoResponse FromSOAP(string soapEnvelope, out FaultModel fault)
        {
            fault = null;

            if (string.IsNullOrEmpty(soapEnvelope))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(soapEnvelope);

                var serializer = new XmlSerializer(typeof(GetTpUserInfoResponse));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);
                nsManager.AddNamespace("ns2", Namespaces.USERINFO);

                var response = xml.SelectSingleNode("//ns2:getTpUserInfoResponse", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as GetTpUserInfoResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ServiceClientException("Cannot deserialize GetTpUserInfoResponse", ex);
            }
        }
    }
}
