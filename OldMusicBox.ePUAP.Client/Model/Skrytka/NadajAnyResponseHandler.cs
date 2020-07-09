using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Skrytka
{
    /// <summary>
    /// NadajAny response handler
    /// </summary>
    public class NadajAnyResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<NadajAnyResponse>
    {
        public NadajAnyResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            return FromSOAP_Template<NadajAnyResponse>(soapResponse, out fault);
        }

        protected override void AddManagerNamespaces(XmlNamespaceManager manager)
        {
            manager.AddNamespace("soapenv", Namespaces.SOAPENVELOPE);
            manager.AddNamespace("p140", Namespaces.OBI);
        }

        protected override string GetResponseXPath()
        {
            return "//soapenv:Envelope/soapenv:Body/p140:OdpowiedzSkrytki";
        }
    }
}
