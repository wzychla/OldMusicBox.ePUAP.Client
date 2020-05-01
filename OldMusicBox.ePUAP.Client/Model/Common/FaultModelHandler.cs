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
    /// Fault model handler
    /// </summary>
    public class FaultModelHandler
    {
        public FaultModel FromSOAP(string soapEnvelope)
        {
            if (string.IsNullOrEmpty(soapEnvelope))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(soapEnvelope);

                var serializer = new XmlSerializer(typeof(FaultModel));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);
                nsManager.AddNamespace("soap", Namespaces.SOAPENVELOPE);

                var response = xml.SelectSingleNode("//soap:Fault", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as FaultModel;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
