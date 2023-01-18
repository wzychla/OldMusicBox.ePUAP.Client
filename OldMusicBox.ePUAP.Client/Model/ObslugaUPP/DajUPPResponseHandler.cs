using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Common;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Model.ObslugaUPP
{
    /// <summary>
    /// Daj UPP response handler
    /// </summary>
    public class DajUPPResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<DajUPPResponse>
    {
        public DajUPPResponse FromSOAP( string soapResponse, out FaultModel fault )
        {
            return FromSOAP_Template<DajUPPResponse>( soapResponse, out fault );
        }

        protected override void AddManagerNamespaces( XmlNamespaceManager manager )
        {
            manager.AddNamespace( "soapenv", Namespaces.SOAPENVELOPE );
            manager.AddNamespace( "p140", Namespaces.OBI );
        }

        protected override string GetResponseXPath()
        {
            return "//soapenv:Envelope/soapenv:Body/p140:OdpowiedzDajUpp";
        }
    }
}
