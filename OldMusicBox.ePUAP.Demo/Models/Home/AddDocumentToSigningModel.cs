using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OldMusicBox.ePUAP.Demo.Models.Home
{
    public class AddDocumentToSigningModel
    {
        [Required]
        public string Document { get; set; }
    }
}