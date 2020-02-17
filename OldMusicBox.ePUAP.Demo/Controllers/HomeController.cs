using OldMusicBox.ePUAP.Demo.Models.Home;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OldMusicBox.ePUAP.Demo.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The view for authenticated user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Signed()
        {
            var model    = new SignedModel();
            model.Claims = (this.User as ClaimsPrincipal).Claims;

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult AddDocumentToSigning()
        {
            return Content("Not yet implemented, be patient");
        }

        [AllowAnonymous]
        public ActionResult VerifySignedDocument()
        {
            return Content("Not yet implemented, be patient");
        }
    }
}