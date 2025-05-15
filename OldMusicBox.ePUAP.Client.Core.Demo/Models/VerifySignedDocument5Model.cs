using OldMusicBox.ePUAP.Client.Core.Model.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OldMusicBox.ePUAP.Client.Core.Demo.Models.Home
{
    public class VerifySignedDocument5Model
    {
        public IFormFile Document { get; set; }

        public EPSignature Signature { get; set; }
    }
}