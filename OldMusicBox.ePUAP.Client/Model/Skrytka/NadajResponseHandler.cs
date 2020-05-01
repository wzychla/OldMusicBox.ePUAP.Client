using OldMusicBox.ePUAP.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Model.Skrytka
{
    /// <summary>
    /// Nadaj response handler
    /// </summary>
    public class NadajResponseHandler
        : IServiceResponseHandler<NadajResponse>
    {
        public NadajResponse FromSOAP(string soapResponse)
        {
            throw new NotImplementedException();
        }
    }
}
