using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OldMusicBox.ePUAP.Demo.Models.Home
{
    public class AddDocumentToSigningSuccessModel
    {
        public string Document { get; set; }

        public Client.Model.GetTpUserInfo.PodpisZP Podpis { get; set; }
    }
}