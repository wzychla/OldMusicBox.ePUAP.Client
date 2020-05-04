using OldMusicBox.ePUAP.Client.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Model.Pull
{
    public class PotwierdzOdebranieResponseHandler :
        BaseServiceResponseHandler,
        IServiceResponseHandler<PotwierdzOdebranieResponse>
    {
        public PotwierdzOdebranieResponse FromSOAP(string soapResponse, out FaultModel fault)
        {
            throw new NotImplementedException();
        }

        protected override void AddManagerNamespaces(XmlNamespaceManager manager)
        {
            throw new NotImplementedException();
        }

        protected override string GetResponseXPath()
        {
            throw new NotImplementedException();
        }
    }
}
