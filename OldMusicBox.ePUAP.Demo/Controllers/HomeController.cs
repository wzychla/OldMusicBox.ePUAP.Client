using OldMusicBox.ePUAP.Client;
using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.ePUAP.Demo.Models.Home;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OldMusicBox.ePUAP.Demo.Controllers
{
    public class HomeController : Controller
    {
        const string ExampleDocument =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Example>
  <Content>
    This is example document. You can sign any XML you want with ePUAP. Local letters: żółć
  </Content>
</Example>";

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
            var model      = new AddDocumentToSigningModel();
            model.Document = ExampleDocument;

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
            var certificate = new ClientCertificateProvider().GetClientCertificate();

            //WSSkrytka_Demo(certificate);
            //WSPull_Demo(certificate);
            //WSZarzadzanieDokumentami_DodajDokument_Demo(certificate);
            WSDoreczyciel_Dorecz_Demo(certificate);

            return Redirect("/Home/Index");
        }

        /// <summary>
        /// Nadanie dokumentu do zewnętrznej skrytki ze wskazaniem własnej skrytki jako skrytki odpowiedzi
        /// </summary>
        private void WSSkrytka_Demo(X509Certificate2 certificate)
        {
            FaultModel fault;

            var _podmiot         = "vulcandpo";
            var _adresSkrytki    = "/vulcandpo/testowa";
            var _adresOdpowiedzi = "/vulcandpo/domyslna";

            var client = new SkrytkaClient(SkrytkaClient.INTEGRATION_URI, certificate);
            var response = client.Nadaj(
                _podmiot,
                _adresSkrytki,
                _adresOdpowiedzi,                
                false,
                null,
                new Client.Model.Skrytka.DocumentType()
                {
                    NazwaPliku = "testowy.xml",
                    TypPliku   = "text/xml",
                    Zawartosc  = Encoding.UTF8.GetBytes(ExampleDocument)
                },
                out fault);
        }

        /// <summary>
        /// Sprawdzenie liczby oczekujacych dokumentów a następnie odebranie dokumentów
        /// </summary>
        private void WSPull_Demo(X509Certificate2 certificate)
        {
            FaultModel fault;

            var client = new PullClient(PullClient.INTEGRATION_URI, certificate);

            var _podmiot      = "vulcandpo";
            var _nazwaSkrytki = "testowa";
            var _adresSkrytki = "/vulcandpo/testowa";

            var oczekujaceDokumenty = client.OczekujaceDokumenty(_podmiot, _nazwaSkrytki, _adresSkrytki, out fault);
            if ( fault != null )
            {
                throw new ApplicationException("Consult fault object for more details");
            }

            if ( oczekujaceDokumenty.Oczekujace > 0 )
            {
                // repeat this in a loop
                var pobierzNastepny = client.PobierzNastepny(_podmiot, _nazwaSkrytki, _adresSkrytki, out fault);
                if (fault != null)
                {
                    throw new ApplicationException("Consult fault object for more details");
                }

                if (pobierzNastepny.Dokument != null &&
                    pobierzNastepny.Dokument.Zawartosc != null
                    )
                {
                    using (var sha1 = new SHA1CryptoServiceProvider())
                    {
                        var _skrot = sha1.ComputeHash(pobierzNastepny.Dokument.Zawartosc);

                        var potwierdzOdebranie = client.PotwierdzOdebranie(_podmiot, _nazwaSkrytki, _adresSkrytki, _skrot, out fault);
                    }
                }
            }
        }

        /// <summary>
        /// WS-Doręczyciel - doręczenie dokumentu do wskazanego odbiorcy z żądaniem UPD
        /// </summary>
        private void WSDoreczyciel_Dorecz_Demo(X509Certificate2 certificate)
        {
            FaultModel fault;

            var client = new DoreczycielClient(DoreczycielClient.INTEGRATION_URI, certificate);

            var _podmiot                = "vulcandpo";
            var _adresSkrytki           = "/vulcandpo/domyslna";
            var _adresOdpowiedzi        = "/vulcandpo/testowa";
            var _identyfikatorDokumentu = "id_123456";
            var _identyfikatorSprawy    = "ids_123456";

            // uwaga ten dokument nie przejdzie przez dorecz, ponieważ nie jest podpisany
            // tylko dokumenty podpisane PK przechodzą
            var doreczenie              = client.Dorecz(_podmiot, _adresSkrytki, _adresOdpowiedzi, 
                                                    DateTime.UtcNow, false, 
                                                    _identyfikatorDokumentu, _identyfikatorSprawy,
                                                    null, null,
                                                    new Client.Model.Doreczyciel.DocumentType()
                                                    {
                                                        //NazwaPliku = "testowy.xml",
                                                        //TypPliku   = "text/xml",
                                                        //Zawartosc  = Encoding.UTF8.GetBytes(ExampleDocument)
                                                        NazwaPliku   = "test20200821_01.xml",
                                                        TypPliku     = "text/xml",
                                                        Zawartosc    = System.IO.File.ReadAllBytes(@"c:\Temp\ePUAP\xades\test.637335615492962051.xml")
                                                    },
                                                    out fault);
            if (fault != null)
            {
                throw new ApplicationException("Consult fault object for more details");
            }
        }

        /// <summary>
        /// Demo pokazuje jak umieścić dokument we wskazanej skrzynce właściciela certyfikatu
        /// 
        /// Jako Folder należy podać
        /// * RECEIVED - dokument trafia do foldera Odebrane
        /// * SENT     - dokument trafia do foldera Wysłane
        /// * [puste]  - dokument trafia do foldera Robocze
        /// </summary>
        /// <param name="certificate"></param>
        private void WSZarzadzanieDokumentami_DodajDokument_Demo(X509Certificate2 certificate)
        {
            FaultModel fault;

            var client   = new ZarzadzanieDokumentamiClient(ZarzadzanieDokumentamiClient.INTEGRATION_URI, certificate);
            var response = client.DodajDokument(
                new Client.Model.ZarzadzanieDokumentami.Sklad()
                {
                    Nazwa   = "testowa",
                    Podmiot = "vulcandpo"
                },
                new Client.Model.ZarzadzanieDokumentami.Dokument()
                {
                    SzczegolyDokumentu = new Client.Model.ZarzadzanieDokumentami.SzczegolyDokumentu()
                    {
                        Nazwa = "dokument123.xml",
                        Adresat = new Client.Model.ZarzadzanieDokumentami.NadawcaOdbiorca()
                        {
                            Nazwa = "vulcandpo",
                            Adres = "/vulcandpo/testowa"
                        },
                        Nadawca = new Client.Model.ZarzadzanieDokumentami.NadawcaOdbiorca()
                        {
                            Nazwa = "vulcandpo",
                            Adres = "/vulcandpo/domyslna"
                        },
                        Folder = "RECEIVED"
                    },
                    Tresc = Encoding.UTF8.GetBytes(ExampleDocument)
                },
                out fault
                );
        }



        #endregion
    }
}