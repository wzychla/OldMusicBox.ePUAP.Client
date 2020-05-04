using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Model.Pull
{
    /// <summary>
    /// PobierzNastepny response handler
    /// </summary>
    public class PobierzNastepnyResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<PobierzNastepnyResponse>
    {
        public PobierzNastepnyResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            return this.FromSOAP_Template<PobierzNastepnyResponse>(soapResponse, out fault);
        }

        protected override void AddManagerNamespaces(XmlNamespaceManager manager)
        {
            manager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
            manager.AddNamespace("p140", Namespaces.OBI);
        }

        protected override string GetResponseXPath()
        {
            return "//soapenv:Envelope/soapenv:Body/p140:OdpowiedzPullPobierz";
        }
    }
}
