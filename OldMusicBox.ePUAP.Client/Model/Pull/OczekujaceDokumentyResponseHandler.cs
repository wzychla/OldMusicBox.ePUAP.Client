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

namespace OldMusicBox.ePUAP.Client.Model.Pull
{
    public class OczekujaceDokumentyResponseHandler : 
        BaseServiceResponseHandler,
        IServiceResponseHandler<OczekujaceDokumentyResponse>
    {
        public OczekujaceDokumentyResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            fault = null;

            if (string.IsNullOrEmpty(soapResponse))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(soapResponse);

                // fault?
                if ( this.TryDeserializeFaultModel( soapResponse, out fault ) )
                {
                    return null;
                }

                // response?
                var serializer = new XmlSerializer(typeof(OczekujaceDokumentyResponse));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);
                nsManager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
                nsManager.AddNamespace("p140", Namespaces.OBI);

                var response = xml.SelectSingleNode("//soapenv:Envelope/soapenv:Body/p140:OdpowiedzPullOczekujace", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as OczekujaceDokumentyResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ServiceClientException("Cannot deserialize OczekujaceDokumentyResponse", ex);
            }
        }
    }
}
