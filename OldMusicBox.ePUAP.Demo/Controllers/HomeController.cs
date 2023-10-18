using OldMusicBox.ePUAP.Client;
using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.ePUAP.Client.Model.Dokumenty;
using OldMusicBox.ePUAP.Client.Model.ObslugaUPP;
using OldMusicBox.ePUAP.Client.Model.XML;
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
using System.Xml;
using System.Xml.Serialization;

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

        #region Add document to signing (TpSigning)

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

        #region Verify signed document (TpSigning)

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

        #region Add document to signing (TpSiging5)

        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddDocumentToSigning5()
        {
            var model = new AddDocumentToSigning5Model();
            model.Document = ExampleDocument;

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDocumentToSigning5(AddDocumentToSigning5Model model)
        {
            if (this.ModelState.IsValid)
            {
                var tpSigningUri = ConfigurationManager.AppSettings["tpSigning5"];
                var certificate = new ClientCertificateProvider().GetClientCertificate();

                var document   = Encoding.UTF8.GetBytes(model.Document);
                var urlSuccess =
                    Url.Action("AddDocumentToSigning5Success", "Home",
                        routeValues: null,
                        protocol: Request.Url.Scheme);
                var urlFailed =
                    Url.Action("AddDocumentToSigning5Failure", "Home",
                        routeValues: null,
                        protocol: Request.Url.Scheme);

                var additionalInfo = "Some additional info";

                // call ePUAP and get their redirect uri
                // they redirect back to one of your uris
                var client = new TpSigning5Client(tpSigningUri, certificate);
                FaultModel fault;
                var response = client.AddDocumentToSigning(document, urlSuccess, urlFailed, additionalInfo, out fault);

                if (response != null &&
                    !string.IsNullOrEmpty(response.Url)
                    )
                {
                    // the returned url has to be stored
                    // it will be used to query the GetSignedDocument
                    this.Session.Add("url", response.Url);
                    return Redirect(response.Url);
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

            return View(model);
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

            var url = this.Session["url"] as string;
            if (!string.IsNullOrEmpty(url))
            {
                var tpSigningUri = ConfigurationManager.AppSettings["tpSigning5"];
                var certificate = new ClientCertificateProvider().GetClientCertificate();

                // call ePUAP and get their redirect uri
                // they redirect back to one of your uris
                var client = new TpSigning5Client(tpSigningUri, certificate);
                FaultModel fault;
                var response = client.GetSignedDocument(url, out fault);

                if (response != null &&
                    response.IsValid
                    )
                {
                    var model = new AddDocumentToSigning5SuccessModel();

                    // this is the document signed by the user
                    model.Document = Encoding.UTF8.GetString(Convert.FromBase64String(response.Content));
                    // it contains the full user information
                    model.Signature = response.Signature;

                    // add to session
                    this.Session.Add(SESSIONDOCUMENT, Convert.FromBase64String(response.Content));

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
        public ActionResult AddDocumentToSigning5Success(FormCollection form)
        {
            if (this.Session[SESSIONDOCUMENT] != null)
            {
                byte[] document = this.Session[SESSIONDOCUMENT] as byte[];

                return File(document, "text/xml", "signedDocument");
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
            this.TempData.Add("Message", "ePUAP document signing cancelled by the user");
            return Redirect("/Home/Index");
        }

        #endregion

        #region Verify signed document (TpSigning5)

        [AllowAnonymous]
        [HttpGet]
        public ActionResult VerifySignedDocument5()
        {
            VerifySignedDocument5Model model = new VerifySignedDocument5Model();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult VerifySignedDocument5(VerifySignedDocument5Model model)
        {
            if (model.Document == null)
            {
                this.ViewBag.Message = "Należy wskazać niepusty dokument do walidacji";
            }
            else
            {
                try
                {
                    var tpSigningUri = ConfigurationManager.AppSettings["tpSigning5"];
                    var certificate = new ClientCertificateProvider().GetClientCertificate();

                    var client = new TpSigning5Client(tpSigningUri, certificate);

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
                        model.Signature = result.Signature;
                        this.ViewBag.Message = result.Content;
                    }
                }
                catch (Exception ex)
                {
                    this.ViewBag.Message = ex.Message;
                }
            }

            return View(model);
        }


        #endregion

        #region Other services

        public ActionResult WSSkrytka()
        {
            var certificate = new ClientCertificateProvider().GetClientCertificate();

            WSSkrytka_Demo(certificate);

            return Redirect("/Home/Index");
        }

        public ActionResult WSPull()
        {
            var certificate = new ClientCertificateProvider().GetClientCertificate();

            WSPull_Demo(certificate);

            return Redirect("/Home/Index");
        }

        public ActionResult WSZarzadzanieDokumentami()
        {
            var certificate = new ClientCertificateProvider().GetClientCertificate();

            WSZarzadzanieDokumentami_DodajDokument_Demo(certificate);

            return Redirect("/Home/Index");
        }

        public ActionResult WSDoreczyciel()
        {
            var certificate = new ClientCertificateProvider().GetClientCertificate();

            WSDoreczyciel_Dorecz_Demo(certificate);

            return Redirect("/Home/Index");
        }

        public ActionResult WSObslugaUPP()
        {
            var certificate = new ClientCertificateProvider().GetClientCertificate();

            WSObslugaUpp_DajUPP( certificate );

            return Redirect( "/Home/Index" );
        }

        /// <summary>
        /// Nadanie dokumentu do zewnętrznej skrytki ze wskazaniem własnej skrytki jako skrytki odpowiedzi
        /// </summary>
        private void WSSkrytka_Demo(X509Certificate2 certificate)
        {
            FaultModel fault;

            var _podmiot         = "vulcandpo";
            var _adresOdpowiedzi = "/vulcandpo/domyslna";
            var _adresSkrytki    = "/vulcandpo/testowa";

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

            // możliwe że odpowiedź zawiera UPP
            if ( response != null )
            {
                if ( response.Zalacznik != null )
                {
                    var zawartosc = response.Zalacznik.Zawartosc;
                    var nazwa     = response.Zalacznik.NazwaPliku;
                    var typ       = response.Zalacznik.TypPliku;

                    try
                    {
                        var zawartoscXml = new XmlDocument();
                        zawartoscXml.LoadXml(Encoding.UTF8.GetString(zawartosc));

                        OldMusicBox.ePUAP.Client.Model.UPP.Dokument zawartoscDokument;
                        XmlSerializer serializer = new XmlSerializer(typeof(OldMusicBox.ePUAP.Client.Model.UPP.Dokument));
                        using (XmlReader reader = new XmlNodeReader(zawartoscXml))
                        {
                            zawartoscDokument = (OldMusicBox.ePUAP.Client.Model.UPP.Dokument)serializer.Deserialize(reader);
                        }
                    }
                    catch( Exception ex )
                    {
                        // nie UPP
                    }
                }
            }
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
            //var _adresSkrytki           = "/adam_testowy/domyslna";
            var _adresSkrytki           = "/vulcandpo/domyslna";
            var _adresOdpowiedzi        = "/vulcandpo/testowa";
            //var _adresOdpowiedzi        = "/vulcandpo/domyslna";
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
                                                        NazwaPliku   = "test20200824_01.xml",
                                                        TypPliku     = "text/xml",
                                                        Zawartosc    = System.IO.File.ReadAllBytes(@"c:\Temp\ePUAP\xades\test.637338735314422836.xml")
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
        /// * DRAFT    - robocze
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
                    Nazwa   = "domyslna",
                    Podmiot = "vulcandpou"
                },
                new Client.Model.ZarzadzanieDokumentami.Dokument()
                {
                    SzczegolyDokumentu = new Client.Model.ZarzadzanieDokumentami.SzczegolyDokumentu()
                    {
                        Nazwa = "dokument123.xml",
                        Adresat = new Client.Model.ZarzadzanieDokumentami.NadawcaOdbiorca()
                        {
                            Nazwa = "vulcandpo",
                            Adres = "/vulcandpo/domyslna"
                        },
                        Nadawca = new Client.Model.ZarzadzanieDokumentami.NadawcaOdbiorca()
                        {
                            Nazwa = "vulcandpo",
                            Adres = "/vulcandpo/domyslna"
                        },
                        Folder = "DRAFT"
                    },
                    Tresc = Encoding.UTF8.GetBytes(ExampleDocument)
                },
                out fault
                );
        }

        private void WSObslugaUpp_DajUPP( X509Certificate2 certificate )
        {
            FaultModel fault;

            var _podmiot         = "vulcandpo";
            var client = new ObslugaUPPClient(ObslugaUPPClient.INTEGRATION_URI, certificate);
            var response = client.DajUPP(
                _podmiot,
                new Client.Model.ObslugaUPP.UzytkownikType()
                {
                    Identyfikator = _podmiot,
                    TypIdentyfikatora = UzytkownikType.EPUAP_ID,
                    Nazwa = "nazwa nadawcy jako napis"
                },
                new Client.Model.ObslugaUPP.DocumentType()
                {
                    NazwaPliku = "testowy.xml",
                    TypPliku   = "text/xml",
                    Zawartosc  = Encoding.UTF8.GetBytes(ExampleDocument)
                },
                out fault);

            // możliwe że odpowiedź zawiera UPP
            if ( response != null )
            {
                if ( response.UPP != null )
                {
                    var zawartosc = response.UPP.Zawartosc;
                    var nazwa     = response.UPP.NazwaPliku;
                    var typ       = response.UPP.TypPliku;

                    try
                    {
                        var zawartoscXml = new XmlDocument();
                        zawartoscXml.LoadXml( Encoding.UTF8.GetString( zawartosc ) );

                        OldMusicBox.ePUAP.Client.Model.UPP.Dokument zawartoscDokument;
                        XmlSerializer serializer = new XmlSerializer(typeof(OldMusicBox.ePUAP.Client.Model.UPP.Dokument));
                        using ( XmlReader reader = new XmlNodeReader( zawartoscXml ) )
                        {
                            zawartoscDokument = (OldMusicBox.ePUAP.Client.Model.UPP.Dokument)serializer.Deserialize( reader );
                        }
                    }
                    catch ( Exception ex )
                    {
                        // nie UPP
                    }
                }
            }
        }

        public ActionResult TrustedProfileInfoForPESEL()
        {
            var certificate = new ClientCertificateProvider().GetClientCertificate();

            FaultModel fault;

            var _pesel = "15280119601";

            var client = new TpUserObjectsInfoClient(TpUserObjectsInfoClient.INTEGRATION_URI, certificate);
            var response = client.TrustedProfileInfoForPESEL(
                _pesel,
                Client.Model.TrustedProfileInfoForPESEL.ProfileInfoEnum.MOST_RECENT,
                out fault);

            if (response != null && response.Profile != null)
            {
                var userId = response.Profile.UserId;
            }

            return Redirect("/Home/Index");
        }

        #endregion

        #region ExternalUploadServlet

        /// <summary>
        /// ExternalUploadServlet
        /// </summary>
        /// <returns></returns>
        public ActionResult ExternalUploadServlet()
        {
            var model = new ExternalUploadServletModel();

            model.XML = CreateExamplePismoOgolne();

            return View(model);
        }

        private string CreateExamplePismoOgolne()
        {
            // https://stackoverflow.com/questions/17279712/what-is-the-smallest-possible-valid-pdf
            var pdf = Convert.FromBase64String("JVBERi0xLg10cmFpbGVyPDwvUm9vdDw8L1BhZ2VzPDwvS2lkc1s8PC9NZWRpYUJveFswIDAgMyAzXT4+XT4+Pj4+Pg==");
            
            var dokument = new Dokument();

            dokument.Opis.Data.Czas.Wartosc = DateTime.Now.ToString( "o" );

            dokument.Dane.Data.Czas.Wartosc = DateTime.Now.ToString( "yyyy-MM-dd" );

            dokument.Dane.Adresaci.Podmiot.Osoba = new Osoba();
            dokument.Dane.Adresaci.Podmiot.Osoba.Nazwisko = "Kowalski";
            dokument.Dane.Adresaci.Podmiot.Osoba.Imie = "Jan";

            dokument.Dane.Nadawcy.Podmiot.Instytucja = new Instytucja();
            dokument.Dane.Nadawcy.Podmiot.Instytucja.NazwaInstytucji = "Urząd miasta Widliszki Wielkie";
            dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Miejscowosc = "Widliszki Wielkie";
            dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Ulica = "Kwiatowa";
            dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Budynek = "1-8";
            dokument.Dane.Nadawcy.Podmiot.Instytucja.Adres.Poczta = "11-110";

            dokument.Tresc.MiejscowoscDokumentu = "Widliszki Wielkie";
            dokument.Tresc.Tytul = "Zawiadomienie w sprawie 1234/2019";
            dokument.Tresc.RodzajWnioskuRozszerzony.JakisInny = "inne pismo";
            dokument.Tresc.RodzajWnioskuRozszerzony.Rodzaj = "zawiadomienie";
            dokument.Tresc.Informacje = new Informacja[]
            {
                    new Informacja()
                    {
                        Wartosc = "Ala ma kota"
                    },
                    new Informacja()
                    {
                        Wartosc = "Basia ma wózek widłowy"
                    }
            };
            dokument.Tresc.Zalaczniki = new Zalacznik[]
            {
                    new Zalacznik()
                    {
                        Format         = "application/octet-stream",
                        NazwaPliku     = "test.pdf",
                        DaneZalacznika = new DaneZalacznika()
                        {
                            Zawartosc = pdf
                        }
                    }
            };
            // act

            var namespaces = new XmlSerializerNamespaces();
            //namespaces.Add("", ePUAP.Client.Constants.Namespaces.WNIO_PODPISANYDOKUMENT);
            namespaces.Add( "wnio", ePUAP.Client.Constants.Namespaces.CRD_WNIO );
            namespaces.Add( "meta", ePUAP.Client.Constants.Namespaces.CRD_META );
            namespaces.Add( "str", ePUAP.Client.Constants.Namespaces.CRD_STR );
            namespaces.Add( "adr", ePUAP.Client.Constants.Namespaces.CRD_ADR );
            namespaces.Add( "oso", ePUAP.Client.Constants.Namespaces.CRD_OSO );
            namespaces.Add( "inst", ePUAP.Client.Constants.Namespaces.CRD_INST );

            // wnio:Dokument
            var document = dokument.ToXmlDocument(namespaces);
            var pi = document.CreateProcessingInstruction(
                "xml-stylesheet",
                "type=\"text/xsl\" href=\"http://crd.gov.pl/wzor/2013/12/12/1410/styl.xsl\"");
            document.InsertAfter( pi, document.FirstChild );

            return document.OuterXml;
        }

        #endregion
    }
}