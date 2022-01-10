using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Model.FileRepoService
{

    /// <summary>
    /// uploadFile response handler
    /// </summary>
    public class UploadFileResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<UploadFileResponse>
    {
        public UploadFileResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            return this.FromSOAP_Template<UploadFileResponse>(soapResponse, out fault);
        }

        public UploadFileResponse FromSOAP(byte[] soapResponse, string content_typeResponse, out FaultModel fault)
        {
            throw new NotImplementedException();
        }

        protected override void AddManagerNamespaces(XmlNamespaceManager manager)
        {
            manager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
            manager.AddNamespace("ns2", Namespaces.FILEREPOCORE);
            manager.AddNamespace("wsu", Namespaces.WS_SEC_UTILITY);
        }

        protected override string GetResponseXPath()
        {
            return "//soapenv:Envelope/soapenv:Body/ns2:fileId5";
        }
    }
}
