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

namespace OldMusicBox.ePUAP.Client.Core.Model.Pull
{
    /// <summary>
    /// OczekujaceDokumenty response handler
    /// </summary>
    public class OczekujaceDokumentyResponseHandler : 
        BaseServiceResponseHandler,
        IServiceResponseHandler<OczekujaceDokumentyResponse>
    {
        public OczekujaceDokumentyResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            return this.FromSOAP_Template<OczekujaceDokumentyResponse>(soapResponse, out fault);
        }

        protected override void AddManagerNamespaces(XmlNamespaceManager manager)
        {
            manager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
            manager.AddNamespace("p140", Namespaces.OBI);
        }

        protected override string GetResponseXPath()
        {
            return "//soapenv:Envelope/soapenv:Body/p140:OdpowiedzPullOczekujace";
        }
    }
}
