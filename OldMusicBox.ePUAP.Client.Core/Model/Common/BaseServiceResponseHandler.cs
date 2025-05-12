using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.Common
{
    /// <summary>
    /// Base class for service responses that respond with 200 and containt the fault model as the response
    /// </summary>
    public abstract class BaseServiceResponseHandler 
    {
        /// <summary>
        /// Faults are passed inside SOAP envs
        /// </summary>
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

        /// <summary>
        /// Template method for derived classes
        /// </summary>
        protected T FromSOAP_Template<T>(string soapResponse, out FaultModel fault)
            where T : class, IServiceResponse
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
                if (this.TryDeserializeFaultModel(soapResponse, out fault))
                {
                    return null;
                }

                // response?
                var serializer = new XmlSerializer(typeof(T));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);

                this.AddManagerNamespaces(nsManager);

                var response = xml.SelectSingleNode(this.GetResponseXPath(), nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as T;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ServiceClientException(string.Format("Cannot deserialize {0}", typeof(T).Name ), ex);
            }
        }

        protected abstract void AddManagerNamespaces(XmlNamespaceManager manager);

        protected abstract string GetResponseXPath();
    }
}
