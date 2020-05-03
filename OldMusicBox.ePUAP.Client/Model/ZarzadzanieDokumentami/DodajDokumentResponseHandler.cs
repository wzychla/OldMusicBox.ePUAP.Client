using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.ZarzadzanieDokumentami
{
    /// <summary>
    /// DodajDokument response handler
    /// </summary>
    public class DodajDokumentResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<DodajDokumentResponse>
    {
        public DodajDokumentResponse FromSOAP(string soapResponse, out FaultModel fault)
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
                var serializer = new XmlSerializer(typeof(DodajDokumentResponse));
                var nsManager  = new XmlNamespaceManager(xml.NameTable);
                nsManager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
                nsManager.AddNamespace("p521", Namespaces.ZARZADZANIEDOKUMENTAMI);

                var response = xml.SelectSingleNode("//soapenv:Envelope/soapenv:Body/p521:dodajDokumentResponse", nsManager) as XmlElement;
                if (response != null)
                {
                    using (var reader = new StringReader(response.OuterXml))
                    {
                        return serializer.Deserialize(reader) as DodajDokumentResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ServiceClientException("Cannot deserialize dodajDokumentResponse", ex);
            }
        }
    }
}
