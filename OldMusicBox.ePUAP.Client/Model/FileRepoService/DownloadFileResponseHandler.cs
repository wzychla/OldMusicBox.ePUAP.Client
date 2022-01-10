using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace OldMusicBox.ePUAP.Client.Model.FileRepoService
{
    /// <summary>
    /// downloadFile response handler
    /// </summary>
    public class DownloadFileResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<DownloadFileResponse>
    {
        public DownloadFileResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            return this.FromSOAP_Template<DownloadFileResponse>(soapResponse, out fault);
        }

        public DownloadFileResponse FromSOAP(byte[] soapResponse, string content_typeResponse, out FaultModel fault)
        {
            fault = null;
            try
            {
                var ms                     = new MemoryStream(soapResponse);
                Envelope obj               = new Envelope();

                DataContractSerializer dcs = new DataContractSerializer(obj.GetType());
                XmlDictionaryReader reader = XmlDictionaryReader.CreateMtomReader(ms, new Encoding[] { Encoding.UTF8 }, content_typeResponse, XmlDictionaryReaderQuotas.Max);
                obj                        = (Envelope)dcs.ReadObject(reader);

                return obj.Body.DownloadFileResponse;
            }
            catch (Exception ex)
            {
                throw new ServiceClientException(string.Format("Cannot deserialize {0}", typeof(DownloadFileResponse).Name), ex);
            }
        }

        protected override void AddManagerNamespaces(XmlNamespaceManager manager)
        {
            manager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
            manager.AddNamespace("ns2", Namespaces.FILEREPOCORE);
            manager.AddNamespace("wsu", Namespaces.WS_SEC_UTILITY);
        }

        protected override string GetResponseXPath()
        {
            return "//soapenv:Envelope/soapenv:Body/ns2:DataDocument";
        }
    }
}
