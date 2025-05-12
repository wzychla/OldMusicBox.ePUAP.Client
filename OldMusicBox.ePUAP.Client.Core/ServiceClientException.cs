using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// Generic exception
    /// </summary>
    public class ServiceClientException : Exception
    {
        public ServiceClientException() : base() { }

        public ServiceClientException(string message) : base(message) { }

        public ServiceClientException(string message, Exception inner) : base(message, inner) { }
    }
}
