using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Model.Doreczyciel
{
    /// <summary>
    /// Dorecz response handler
    /// </summary>
    public class DoreczResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<DoreczResponse>
    {
        public DoreczResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            return FromSOAP_Template<DoreczResponse>(soapResponse, out fault);
        }

        protected override void AddManagerNamespaces(XmlNamespaceManager manager)
        {
            manager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
            manager.AddNamespace("p140",    Namespaces.OBI);
        }

        protected override string GetResponseXPath()
        {
            return "//soapenv:Envelope/soapenv:Body/p140:OdpowiedzDoreczyciela";
        }
    }
}
