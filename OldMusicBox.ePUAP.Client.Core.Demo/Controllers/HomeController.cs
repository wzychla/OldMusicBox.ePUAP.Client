using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OldMusicBox.ePUAP.Client.Core.Demo.Models.Home;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using System.Text;

namespace OldMusicBox.ePUAP.Client.Core.Demo.Controllers
{
    public class HomeController : Controller
    {
        private TpSigning5Client _tpSigning5Client;

        public HomeController( TpSigning5Client tpSigning5Client )
        {
            this._tpSigning5Client = tpSigning5Client;
        }

        const string ExampleDocument =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Example>
  <Content>
    This is example document. You can sign any XML you want with ePUAP. Local letters: żółć
  </Content>
</Example>";

        const string SESSIONDOCUMENT = "session_document";

        public ActionResult Index()
        {
            return View();
        }

        #region Add document to signing (TpSiging5)

        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddDocumentToSigning5()
        {
            var model = new AddDocumentToSigning5Model();
            model.Document = ExampleDocument;

            return View( model );
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddDocumentToSigning5( AddDocumentToSigning5Model model )
        {
            if ( this.ModelState.IsValid )
            {
                var document   = Encoding.UTF8.GetBytes(model.Document);
                var urlSuccess =
                    Url.Action("AddDocumentToSigning5Success", "Home",
                        null,
                        this.Request.Scheme);
                var urlFailed =
                    Url.Action("AddDocumentToSigning5Failure", "Home",
                        null,
                        Request.Scheme);

                var additionalInfo = "Some additional info";

                // call ePUAP and get their redirect uri
                // they redirect back to one of your uris
                FaultModel fault;
                var response = this._tpSigning5Client.AddDocumentToSigning(document, urlSuccess, urlFailed, additionalInfo, out fault);

                if ( response != null &&
                    !string.IsNullOrEmpty( response.Url )
                    )
                {
                    // the returned url has to be stored
                    // it will be used to query the GetSignedDocument
                    this.HttpContext.Session.SetString( "url", response.Url );
                    return Redirect( response.Url );
                }
                else
                {
                    if ( fault != null )
                    {
                        this.TempData.Add( "Message", string.Format( "ePUAP fault: {0}, information: {1}", fault.FaultCode, fault.FaultString ) );

                    }
                    else
                    {
                        this.TempData.Add( "Message", "Unknown error" );
                    }

                    return Redirect( "/Home/Index" );
                }
            }

            return View( model );
        }

        /// <summary>
        /// ePUAP redirects here when the document is signed correctly.
        /// This is where the GetSignedDocument has to be called
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddDocumentToSigning5Success()
        {
            string message = string.Empty;

            var url = this.HttpContext.Session.GetString("url") as string;
            if ( !string.IsNullOrEmpty( url ) )
            {
                // call ePUAP and get their redirect uri
                // they redirect back to one of your uris
                FaultModel fault;
                var response = this._tpSigning5Client.GetSignedDocument(url, out fault);

                if ( response != null )
                {
                    var model = new AddDocumentToSigning5SuccessModel();

                    // this is the document signed by the user
                    model.Document = Encoding.UTF8.GetString( Convert.FromBase64String( response.Content ) );
                    // it contains the full user information
                    model.Signature = response.Signature;

                    // add to session
                    this.HttpContext.Session.SetString( SESSIONDOCUMENT, response.Content );

                    return View( model );
                }
                else
                {
                    if ( fault != null )
                    {
                        this.TempData.Add( "Message", string.Format( "ePUAP fault: {0}, information: {1}", fault.FaultCode, fault.FaultString ) );
                    }
                    else
                    {
                        this.TempData.Add( "Message", "Unknown error" );
                    }

                    return Redirect( "/Home/Index" );
                }
            }

            // fallback to the main page with message to the user
            this.TempData.Add( "Message", message );
            return Redirect( "/Home/Index" );

        }

        /// <summary>
        /// Let the user download the document they have succesfully signed
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddDocumentToSigning5Success( FormCollection form )
        {
            if ( this.HttpContext.Session.GetString(SESSIONDOCUMENT) != null )
            {
                byte[] document = Convert.FromBase64String( this.HttpContext.Session.GetString( SESSIONDOCUMENT ) );

                return File( document, "text/xml", "signedDocument" );
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
        public ActionResult AddDocumentToSigning5Failure()
        {
            this.TempData.Add( "Message", "ePUAP document signing cancelled by the user" );
            return Redirect( "/Home/Index" );
        }

        #endregion

        #region Verify signed document (TpSigning5)

        [AllowAnonymous]
        [HttpGet]
        public ActionResult VerifySignedDocument5()
        {
            var model = new VerifySignedDocument5Model();
            return View( model );
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult VerifySignedDocument5( VerifySignedDocument5Model model )
        {
            if ( model.Document == null )
            {
                this.ViewBag.Message = "Należy wskazać niepusty dokument do walidacji";
            }
            else
            {
                try
                {
                    byte[] documentData = null;
                    using ( var binaryReader = new BinaryReader( model.Document.OpenReadStream() ) )
                    {
                        documentData = binaryReader.ReadBytes( (int)model.Document.Length );
                    }

                    FaultModel fault;
                    var result = this._tpSigning5Client.VerifySignedDocument(documentData, out fault);

                    if ( fault != null )
                    {
                        this.ViewBag.Message = fault.FaultString;
                    }
                    else
                    {
                        model.Signature = result.Signature;
                        this.ViewBag.Message = result.Content;
                    }
                }
                catch ( Exception ex )
                {
                    this.ViewBag.Message = ex.Message;
                }
            }

            return View( model );
        }


        #endregion
    }
}
