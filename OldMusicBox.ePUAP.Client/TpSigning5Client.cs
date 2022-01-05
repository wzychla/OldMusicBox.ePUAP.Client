using OldMusicBox.ePUAP.Client.Model.AddDocumentToSigning;
using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.ePUAP.Client.Model.GetSignedDocument;
using OldMusicBox.ePUAP.Client.Model.VerifySignedDocument;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// Tp-Signing 5 client 
    /// </summary>
    public class TpSigning5Client : BaseClient
    {
        // TpSigning5
        public const string INTEGRATION_URI = "https://int.pz.gov.pl/ep-services/tpSigning5";
        public const string PRODUCTION_URI  = "https://pz.gov.pl/ep-services/tpSigning5";

        public TpSigning5Client(string serviceUri, X509Certificate2 signingCertificate) : base(serviceUri, signingCertificate)
        {

        }

        #region AddDocumentToSigning5

        /// <summary>
        /// AddDocumentToSigning call
        /// </summary>
        public virtual AddDocumentToSigning5Response AddDocumentToSigning(
            byte[] document,
            string urlSuccess,
            string urlFailed,
            string additionalInfo,
            out FaultModel fault)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(urlSuccess))
                throw new ArgumentNullException("urlSuccess");
            if (string.IsNullOrEmpty(urlFailed))
                throw new ArgumentNullException("urlFailed");
            if (string.IsNullOrEmpty(additionalInfo))
                throw new ArgumentNullException("additionalInfo");

            fault = null;

            // request
            var request =
                new AddDocumentToSigning5Request()
                {
                    Doc = document,
                    SuccessUrl = urlSuccess,
                    FailureUrl = urlFailed,
                    AdditionalInfo = additionalInfo
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<AddDocumentToSigning5Request, AddDocumentToSigning5Response, AddDocumentToSigning5ResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion

        #region GetSignedDocument

        /// <summary>
        /// GetSignedDocument call
        /// </summary>
        public virtual GetSignedDocument5Response GetSignedDocument(
            string id,
            out FaultModel fault)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            fault = null;

            // request
            var request =
                new GetSignedDocument5Request()
                {
                    Id = id
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<GetSignedDocument5Request, GetSignedDocument5Response, GetSignedDocument5ResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion

        #region VerifySignedDocument

        /// <summary>
        /// GetSignedDocument call
        /// </summary>
        public virtual VerifySignedDocument5Response VerifySignedDocument(
            byte[] document,
            out FaultModel fault)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            fault = null;

            // request
            var request =
                new VerifySignedDocument5Request()
                {
                    Document = document
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<VerifySignedDocument5Request, VerifySignedDocument5Response, VerifySignedDocument5ResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
