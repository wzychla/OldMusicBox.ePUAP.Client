using OldMusicBox.ePUAP.Client;
using OldMusicBox.Saml2;
using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Model;
using OldMusicBox.Saml2.Model.Artifact;
using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.Saml2.Model.Request;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OldMusicBox.ePUAP.Client.Constants;

namespace OldMusicBox.ePUAP.Demo.Controllers
{
    /// <summary>
    /// ePUAP SSO is performed here
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Trigger SAML2 SSO AuthnRequest
        /// </summary>
        /// <returns></returns>
        public ActionResult Logon()
        {
            var saml2 = new Saml2AuthenticationModule();

            // parameters
            var assertionConsumerServiceURL = ConfigurationManager.AppSettings["issuerSSO"];
            var assertionIssuer             = ConfigurationManager.AppSettings["issuerName"];
            var identityProvider            = ConfigurationManager.AppSettings["ePUAPSSO"];
            var artifactResolve             = ConfigurationManager.AppSettings["ePUAPArtifact"];

            var getTpUserInfoUri            = ConfigurationManager.AppSettings["tpUserInfo"];

            var requestBinding =  Binding.POST;
            var responseBinding = Binding.ARTIFACT;

            // this is optional
            var x509Configuration = new X509Configuration()
            {
                SignatureCertificate = new ClientCertificateProvider().GetClientCertificate(),
                IncludeKeyInfo       = true,
                SignatureAlgorithm = OldMusicBox.Saml2.Signature.SignatureAlgorithm.SHA256
            };

            // check if this is 
            if (!saml2.IsSignInResponse(this.Request))
            {
                // AuthnRequest factory
                var authnRequestFactory = new AuthnRequestFactory();

                authnRequestFactory.AssertionConsumerServiceURL = assertionConsumerServiceURL;
                authnRequestFactory.AssertionIssuer             = assertionIssuer;
                authnRequestFactory.Destination                 = identityProvider;

                authnRequestFactory.X509Configuration = x509Configuration;

                authnRequestFactory.RequestBinding  = requestBinding;
                authnRequestFactory.ResponseBinding = responseBinding;

                return Content(authnRequestFactory.CreatePostBindingContent());
            }
            else
            {
                // the token is created from the IdP's response
                var artifactConfig = new ArtifactResolveConfiguration()
                {
                    ArtifactResolveUri = artifactResolve,
                    Issuer             = assertionIssuer,
                    X509Configuration  = x509Configuration
                };

                var securityToken = saml2.GetArtifactSecurityToken(this.Request, artifactConfig);

                // fail if there is no token
                if (securityToken == null ||
                    securityToken.Assertion == null ||
                    string.IsNullOrEmpty( securityToken.Assertion.ID )
                   )
                {
                    throw new ArgumentNullException("No valid security token found in the response accoding to the ARTIFACT Response Binding");
                }

                // the token will be validated
                #warning Implement token validation

                // this is the SessionIndex, store it if necessary
                var sessionIndex = securityToken.Assertion.ID;
                var client       = new ServiceClient(getTpUserInfoUri, x509Configuration.SignatureCertificate);
                FaultModel fault;
                var tpUserInfo   = client.GetTpUserInfo(sessionIndex, out fault);
                if ( tpUserInfo == null || !tpUserInfo.IsValid )
                {
                    throw new NullReferenceException(string.Format("GetTpUserInfo returned nothing. Error message is: {0}", fault != null ? fault.FaultString : "."));
                }

                // create the identity
                var identity = new ClaimsIdentity("ePUAP");

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, tpUserInfo.Podpis.Dane.DaneOsobyFizycznej.IdKontaUzytkownikaEpuap));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, tpUserInfo.Podpis.Dane.DaneOsobyFizycznej.Imie));
                identity.AddClaim(new Claim(ClaimTypes.Surname, tpUserInfo.Podpis.Dane.DaneOsobyFizycznej.Nazwisko.Value));
                identity.AddClaim(new Claim(OMBClaimTypes.PESEL, tpUserInfo.Podpis.Dane.DaneOsobyFizycznej.PESEL));
                identity.AddClaim(new Claim(OMBClaimTypes.SessionIndex, sessionIndex));

                if ( !string.IsNullOrEmpty( tpUserInfo.Return.AccountEmailAddress ) )
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, tpUserInfo.Return.AccountEmailAddress));
                }

                // the token is validated succesfully
                var principal = new ClaimsPrincipal(identity);
                if (principal.Identity.IsAuthenticated)
                {
                    // use the SessionAuthenticationModule to store the auth cookie
                    var sam = FederatedAuthentication.SessionAuthenticationModule;

                    var token = sam.CreateSessionSecurityToken(principal, string.Empty,
                            DateTime.Now.ToUniversalTime(), DateTime.Now.AddMinutes(20).ToUniversalTime(), false);
                    sam.WriteSessionTokenToCookie(token);

                    // fixed return url because the provider redirects here without the query string
                    return Redirect("/Home/Signed");
                }
                else
                {
                    throw new ArgumentNullException("principal", "Unauthenticated principal returned from token validation");
                }
            }
        }
    }
}