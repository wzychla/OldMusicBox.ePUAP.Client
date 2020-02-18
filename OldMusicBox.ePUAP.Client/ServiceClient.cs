using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Model.AddDocumentToSigning;
using OldMusicBox.ePUAP.Client.Model.Fault;
using OldMusicBox.ePUAP.Client.Model.GetSignedDocument;
using OldMusicBox.ePUAP.Client.Model.GetTpUserInfo;
using OldMusicBox.ePUAP.Client.Request;
using OldMusicBox.Saml2.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// The service client.
    /// Responsible for calling multiple services
    /// </summary>
    public class ServiceClient
    {
        public ServiceClient( X509Certificate2 signingCertificate )
        {
            if ( signingCertificate == null ||
                 signingCertificate.PrivateKey == null
                )
            {
                throw new ArgumentNullException("signingCertificate");
            }

            this.SigningCertificate = signingCertificate;
        }

        public X509Certificate2 SigningCertificate { get; set; }

        #region AddDocumentToSigning

        /// <summary>
        /// AddDocumentToSigning call
        /// </summary>
        public virtual AddDocumentToSigningResponse AddDocumentToSigning(
            string serviceUrl,
            string base64Document, 
            string urlSuccess, 
            string urlFailed, 
            string additionalInfo, 
            out FaultModel fault)
        {
            if (string.IsNullOrEmpty(base64Document))
                throw new ArgumentNullException("base64Document");
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
                    Doc = base64Document,
                    SuccessUrl = urlSuccess,
                    FailureUrl = urlFailed,
                    AdditionalInfo = additionalInfo
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<AddDocumentToSigningRequest, AddDocumentToSigningResponse, AddDocumentToSigningResponseHandler>(
                serviceUrl,
                request,
                out fault);

            // parse response
            return response;
        }

        #endregion

        #region GetTpUserInfo

        /// <summary>
        /// GetTpUserInfo call
        /// </summary>
        /// <remarks>
        /// Either returns a valid response or a fault information
        /// </remarks>
        public virtual GetTpUserInfoResponse GetTpUserInfo(
            string serviceUrl, 
            string sessionIndex, 
            out FaultModel fault )
        {
            if (string.IsNullOrEmpty(sessionIndex))
            {
                throw new ArgumentNullException("sessionIndex");
            }
            fault = null;

            // request
            var request =
                new GetTpUserInfoRequest()
                {
                    TgSid                = sessionIndex,
                    SystemOrganisationId = "0"
                };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<GetTpUserInfoRequest, GetTpUserInfoResponse, GetTpUserInfoResponseHandler>(
                serviceUrl, 
                request,
                out fault);

            // parse response
            return response;
        }

        #endregion

        #region GetSignedDocument

        /// <summary>
        /// GetSignedDocument call
        /// </summary>
        public virtual GetSignedDocumentResponse GetSignedDocument(
            string serviceUrl,
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
                serviceUrl,
                request,
                out fault);

            // parse response
            return response;
        }

        #endregion

        #region Generic call 

        protected virtual TResult WSSecurityRequest<TRequest, TResult, TResultResponseHandler>( 
            string serviceUrl, 
            TRequest request,
            out FaultModel fault
            )
            where TRequest : class, IServiceRequest
            where TResult : class, IServiceResponse
            where TResultResponseHandler: class, IServiceResponseHandler<TResult>, new()
        {
            fault = null;

            if ( string.IsNullOrEmpty( serviceUrl ) ||
                 request == null
                )
            {
                throw new ArgumentNullException("Can't call a service with incomplete parameters");
            }

            var requestFactory = new RequestFactory(this.SigningCertificate);
            var requestString  = requestFactory.CreateRequest(request);

            // log
            new LoggerFactory().For(this).Debug(Event.SignedMessage, requestString);

            // sending WS-Security request
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("SOAPAction", request.SOAPAction);
                webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";

                string response = null;
                try
                {
                    // POST it
                    response = webClient.UploadString(serviceUrl, requestString );
                }
                catch (WebException ex)
                {
                    if ( ex.Response != null )
                    {
                        var responseStream = ex.Response.GetResponseStream();
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                var responseFault = reader.ReadToEnd();

                                // log
                                new LoggerFactory().For(this).Debug(Event.SignedMessage, responseFault);

                                fault = new FaultModelHandler().FromSOAP(responseFault);

                                return null;
                            }
                        }
                    }

                    // fallback
                    throw new ServiceClientException(string.Format("Client call failed for {0} at {1}", request.SOAPAction, serviceUrl ), ex);
                }

                if (!string.IsNullOrEmpty(response))
                {
                    // log
                    new LoggerFactory().For(this).Debug(Event.SignedMessage, response);

                    var responseHandler = new TResultResponseHandler();
                    return responseHandler.FromSOAP(response);
                }
                else
                {
                    throw new ServiceClientException(string.Format( "Got en empty response from {0} at {1}", request.SOAPAction, serviceUrl));
                }
            }
        }

        #endregion
    }
}
