using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.TrustedProfileInfoForPESEL
{
    /// <summary>
    /// TrustedProfileInfoForPESELResponseHandler
    /// </summary>
    public class TrustedProfileInfoForPESELResponseHandler
        : IServiceResponseHandler<TrustedProfileInfoForPESELResponse>
    {
        public TrustedProfileInfoForPESELResponse FromSOAP(string soapEnvelope, out FaultModel fault)
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

                var serializer = new XmlSerializer(typeof(TrustedProfileInfoForPESELResponse));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);
                nsManager.AddNamespace("ns3", Namespaces.OBJECTINFO);

                var response = xml.SelectSingleNode("//ns3:respTrustedProfileInfoForPESEL", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as TrustedProfileInfoForPESELResponse;
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
