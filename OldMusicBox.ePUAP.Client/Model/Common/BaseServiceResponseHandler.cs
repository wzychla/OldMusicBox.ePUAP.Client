using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Common
{
    /// <summary>
    /// Base class for service responses that respond with 200 and containt the fault model as the response
    /// </summary>
    public abstract class BaseServiceResponseHandler 
    {
        public bool TryDeserializeFaultModel( string soapResponse, out FaultModel fault )
        {
            fault = null;

            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(soapResponse);

                var serializer = new XmlSerializer(typeof(FaultModel));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);
                nsManager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);

                var response = xml.SelectSingleNode("//soapenv:Envelope/soapenv:Body/soapenv:Fault", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        fault = serializer.Deserialize(reader) as FaultModel;
                        if ( fault != null )
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }

        }
    }
}
