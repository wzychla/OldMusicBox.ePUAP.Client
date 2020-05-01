using OldMusicBox.ePUAP.Client;
using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.ePUAP.Demo.Models.Home;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OldMusicBox.ePUAP.Demo.Controllers
{
    public class HomeController : Controller
    {
        #region Anonymous welcome page

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region SSO demo

        /// <summary>
        /// The view for authenticated user
        /// </summary>
        /// <remarks>
        /// Since it's attributed with [Authorize]
        /// it redirects to /Account/Logon and the
        /// SAML2 SSO flow picks up from there
        /// </remarks>
        [Authorize]
        public ActionResult Signed()
        {
            var model    = new SignedModel();
            model.Claims = (this.User as ClaimsPrincipal).Claims;

            return View(model);
        }

        #endregion

        #region Add document to signing

        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddDocumentToSigning()
        {
            var model = new AddDocumentToSigningModel();
            model.Document =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Example>
  <Content>
    This is example document. You can sign any XML you want with ePUAP. Local letters: żółć
  </Content>
</Example>";
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDocumentToSigning(AddDocumentToSigningModel model)
        {
            if (this.ModelState.IsValid)
            {
                var tpSigningUri = ConfigurationManager.AppSettings["tpSigning"];
                var certificate  = new ClientCertificateProvider().GetClientCertificate();

                var document = Encoding.UTF8.GetBytes(model.Document);
                var urlSuccess =
                    Url.Action("AddDocumentToSigningSuccess", "Home",
                        routeValues: null,
                        protocol: Request.Url.Scheme);
                var urlFailed =
                    Url.Action("AddDocumentToSigningFailure", "Home",
                        routeValues: null,
                        protocol: Request.Url.Scheme);

                var additionalInfo = "Some additional info";

                // call ePUAP and get their redirect uri
                // they redirect back to one of your uris
                var client = new TpSigningClient(tpSigningUri, certificate);
                FaultModel fault;
                var response = client.AddDocumentToSigning(document, urlSuccess, urlFailed, additionalInfo, out fault);

                if ( response != null && 
                     response.Return != null && 
                     !string.IsNullOrEmpty(response.Return.Url)
                    )
                {
                    // the returned url has to be stored
                    // it will be used to query the GetSignedDocument
                    this.Session.Add("url", response.Return.Url);
                    return Redirect(response.Return.Url);
                }
                else
                {
                    if ( fault != null )
                    {
                        this.TempData.Add("Message", string.Format("ePUAP fault: {0}, information: {1}", fault.FaultCode, fault.FaultString));

                    }
                    else
                    {
                        this.TempData.Add("Message", "Unknown error");
                    }

                    return Redirect("/Home/Index");
                }
            }

            return View(model);
        }

        const string SESSIONDOCUMENT = "session_document";

        /// <summary>
        /// ePUAP redirects here when the document is signed correctly.
        /// This is where the GetSignedDocument has to be called
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddDocumentToSigningSuccess()
        {
            string message = string.Empty;

            var url = this.Session["url"] as string;
            if ( !string.IsNullOrEmpty(url))
            {
                var tpSigningUri = ConfigurationManager.AppSettings["tpSigning"];
                var certificate  = new ClientCertificateProvider().GetClientCertificate();

                // call ePUAP and get their redirect uri
                // they redirect back to one of your uris
                var client = new TpSigningClient(tpSigningUri, certificate);
                FaultModel fault;
                var response = client.GetSignedDocument(url, out fault);

                if (response != null &&
                    response.IsValid
                    )
                {
                    var model = new AddDocumentToSigningSuccessModel();

                    // this is the document signed by the user
                    model.Document = Encoding.UTF8.GetString(Convert.FromBase64String(response.Return.Content));
                    // it contains the full user information
                    model.Podpis   = response.Podpis;

                    // add to session
                    this.Session.Add(SESSIONDOCUMENT, Convert.FromBase64String(response.Return.Content));

                    return View(model);
                }
                else
                {
                    if (fault != null)
                    {
                        this.TempData.Add("Message", string.Format("ePUAP fault: {0}, information: {1}", fault.FaultCode, fault.FaultString));
                    }
                    else
                    {
                        this.TempData.Add("Message", "Unknown error");
                    }

                    return Redirect("/Home/Index");
                }
            }

            // fallback to the main page with message to the user
            this.TempData.Add("Message", message);
            return Redirect("/Home/Index");

        }

        /// <summary>
        /// Let the user download the document they have succesfully signed
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddDocumentToSigningSuccess(FormCollection form)
        {
            if ( this.Session[SESSIONDOCUMENT] != null )
            {
                byte[] document = this.Session[SESSIONDOCUMENT] as byte[];

                return File(document, "text/xml", "signedDocument" );
            }
            else
            {
                return new EmptyResult();
            }
        }


        /// <summary>
        /// ePUAP redirects here when user cancels signing
        /// </summary>
        [AllowAnonymous] 
        public ActionResult AddDocumentToSigningFailure()
        {
            this.TempData.Add("Message", "ePUAP document signing cancelled by the user");
            return Redirect("/Home/Index");
        }

        #endregion

        #region Verify signed document

        [AllowAnonymous]
        [HttpGet]
        public ActionResult VerifySignedDocument()
        {
            VerifySignedDocumentModel model = new VerifySignedDocumentModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult VerifySignedDocument(VerifySignedDocumentModel model)
        {
            if ( model.Document == null )
            {
                this.ViewBag.Message = "Należy wskazać niepusty dokument do walidacji";
            }
            else
            {
                try
                {
                    var tpSigningUri = ConfigurationManager.AppSettings["tpSigning"];
                    var certificate  = new ClientCertificateProvider().GetClientCertificate();

                    var client = new TpSigningClient(tpSigningUri, certificate);

                    byte[] documentData = null;
                    using (var binaryReader = new BinaryReader(model.Document.InputStream))
                    {
                        documentData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                    }

                    FaultModel fault;
                    var result = client.VerifySignedDocument(documentData, out fault);

                    if (fault != null)
                    {
                        this.ViewBag.Message = fault.FaultString;
                    }
                    else
                    {
                        model.Podpis         = result.Podpis;
                        this.ViewBag.Message = result.Return.Content;
                    }
                }
                catch ( Exception ex )
                {
                    this.ViewBag.Message = ex.Message;
                }
            }

            return View(model);
        }


        #endregion

        #region Other services

        public ActionResult Other()
        {
            var uri         = "https://int.epuap.gov.pl/pk_external_ws/services/pull";
            var certificate = new ClientCertificateProvider().GetClientCertificate();
            var client      = new PullClient(uri, certificate);

            FaultModel fault;
            var response = client.OczekujaceDokumenty("vulcandpo", "domyslna", "/vulcandpo/domyslna", out fault);

            return Redirect("/Home/Index");
        }

        #endregion
    }
}