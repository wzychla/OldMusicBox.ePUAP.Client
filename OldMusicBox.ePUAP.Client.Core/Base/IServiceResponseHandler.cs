using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core.Model
{
    /// <summary>
    /// Response handler, creates the response out of the SOAP string
    /// </summary>
    public interface IServiceResponseHandler<TResult>
        where TResult : class, IServiceResponse
    {
        TResult FromSOAP(string soapResponse, out FaultModel fault);
    }
}
