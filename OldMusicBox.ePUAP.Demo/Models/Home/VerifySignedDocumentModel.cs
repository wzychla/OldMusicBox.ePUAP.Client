using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OldMusicBox.ePUAP.Demo.Models.Home
{
    public class VerifySignedDocumentModel
    {
        public HttpPostedFileBase Document { get; set; }
    
        public Client.Model.GetTpUserInfo.PodpisZP Podpis { get; set; }
    }
}