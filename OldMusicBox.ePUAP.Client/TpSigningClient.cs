using OldMusicBox.ePUAP.Client.Model.AddDocumentToSigning;
using OldMusicBox.ePUAP.Client.Model.Fault;
using OldMusicBox.ePUAP.Client.Model.GetSignedDocument;
using OldMusicBox.ePUAP.Client.Model.VerifySignedDocument;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// Tp-Signing client
    /// </summary>
    public class TpSigningClient : BaseClient
    {
        public TpSigningClient(string serviceUri, X509Certificate2 signingCertificate) : base(serviceUri, signingCertificate)
        {

        }

        #region AddDocumentToSigning

        /// <summary>
        /// AddDocumentToSigning call
        /// </summary>
        public virtual AddDocumentToSigningResponse AddDocumentToSigning(
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
                new AddDocumentToSigningRequest()
                {
                    Doc            = document,
                    SuccessUrl     = urlSuccess,
                    FailureUrl     = urlFailed,
                    AdditionalInfo = additionalInfo
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<AddDocumentToSigningRequest, AddDocumentToSigningResponse, AddDocumentToSigningResponseHandler>(
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
        public virtual GetSignedDocumentResponse GetSignedDocument(
            string id,
            out FaultModel fault)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            fault = null;

            // request
            var request =
                new GetSignedDocumentRequest()
                {
                    Id = id
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<GetSignedDocumentRequest, GetSignedDocumentResponse, GetSignedDocumentResponseHandler>(
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
        public virtual VerifySignedDocumentResponse VerifySignedDocument(
            byte[] document,
            out FaultModel fault)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            fault = null;

            // request
            var request =
                new VerifySignedDocumentRequest()
                {
                    Document = document
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<VerifySignedDocumentRequest, VerifySignedDocumentResponse, VerifySignedDocumentResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion
    }
}
