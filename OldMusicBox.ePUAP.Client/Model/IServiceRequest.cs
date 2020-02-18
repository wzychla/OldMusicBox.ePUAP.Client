using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Model
{
    /// <summary>
    /// Marker interface
    /// </summary>
    public interface IServiceRequest
    {
        string SOAPAction { get; }
    }
}
