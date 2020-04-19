using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Model.Fault;
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
    /// Base WS-* implementation
    /// </summary>
    public abstract class BaseClient
    {
        public BaseClient(X509Certificate2 signingCertificate)
        {
            if (signingCertificate == null ||
                signingCertificate.PrivateKey == null
                )
            {
                throw new ArgumentNullException("signingCertificate");
            }

            this.SigningCertificate = signingCertificate;
        }

        public X509Certificate2 SigningCertificate { get; set; }

        protected virtual TResult WSSecurityRequest<TRequest, TResult, TResultResponseHandler>(
             string serviceUrl,
             TRequest request,
             out FaultModel fault
             )
             where TRequest : class, IServiceRequest
             where TResult  : class, IServiceResponse
             where TResultResponseHandler : class, IServiceResponseHandler<TResult>, new()
        {
            fault = null;

            if (string.IsNullOrEmpty(serviceUrl) ||
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
                    response = webClient.UploadString(serviceUrl, requestString);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
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
                    throw new ServiceClientException(string.Format("Client call failed for {0} at {1}", request.SOAPAction, serviceUrl), ex);
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
                    throw new ServiceClientException(string.Format("Got en empty response from {0} at {1}", request.SOAPAction, serviceUrl));
                }
            }
        }
    }
}
