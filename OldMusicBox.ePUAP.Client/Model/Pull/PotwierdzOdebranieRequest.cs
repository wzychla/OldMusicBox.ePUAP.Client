using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldMusicBox.ePUAP.Client.Request;

namespace OldMusicBox.ePUAP.Client.Model.Pull
{
    /// <summary>
    /// PotwierdzOdebranie request
    /// </summary>
    public class PotwierdzOdebranieRequest : IServiceRequest
    {
        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SOAPAction
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
