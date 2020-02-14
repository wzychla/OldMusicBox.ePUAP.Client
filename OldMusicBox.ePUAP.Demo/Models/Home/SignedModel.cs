using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace OldMusicBox.ePUAP.Demo.Models.Home
{
    public class SignedModel
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}