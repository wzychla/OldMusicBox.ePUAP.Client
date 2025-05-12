using OldMusicBox.ePUAP.Client.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core.Model
{
    /// <summary>
    /// Marker interface
    /// </summary>
    public interface IServiceRequest
    {
        string SOAPAction { get; }

        HeaderAttribute[] HeaderAttributes { get; }
    }
}
