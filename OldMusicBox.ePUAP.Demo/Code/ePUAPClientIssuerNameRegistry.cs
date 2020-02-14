using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace OldMusicBox.ePUAP.Demo
{
    /// <summary>
    /// The issuer name registry can be restricted to accept only 
    /// ePUAP certificate
    /// </summary>
    public class ePUAPClientIssuerNameRegistry : IssuerNameRegistry
    {
        public override string GetIssuerName(SecurityToken securityToken)
        {
            X509SecurityToken x509Token = securityToken as X509SecurityToken;
            if (x509Token != null)
                return x509Token.Certificate.Subject;
            else
                return null;
        }
    }
}