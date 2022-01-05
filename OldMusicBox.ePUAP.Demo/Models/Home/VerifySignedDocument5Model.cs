using OldMusicBox.ePUAP.Client.Model.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OldMusicBox.ePUAP.Demo.Models.Home
{
    public class VerifySignedDocument5Model
    {
        public HttpPostedFileBase Document { get; set; }

        public EPSignature Signature { get; set; }
    }
}