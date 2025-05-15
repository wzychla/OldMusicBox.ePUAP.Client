using Microsoft.Extensions.Logging;
using OldMusicBox.ePUAP.Client.Core.Model;
using OldMusicBox.ePUAP.Client.Core.Model.Common;
using OldMusicBox.ePUAP.Client.Core.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// Base WS-* implementation
    /// </summary>
    public abstract class BaseClient
    {        
        public BaseClient(string serviceUri, ICertificateProvider certificateProvider, ILoggerFactory loggerFactory )
        {
            if ( string.IsNullOrEmpty( serviceUri ) )
            {
                throw new ArgumentNullException( "serviceUri" );
            }
            if ( certificateProvider == null )
            {
                throw new ArgumentNullException();
            }
            if ( loggerFactory == null )
            {
                throw new ArgumentNullException();
            }

            var signingCertificate = certificateProvider.GetCertificate();
            if (signingCertificate == null ||
                signingCertificate.GetRSAPrivateKey() == null
                )
            {
                throw new ArgumentNullException("signingCertificate");
            }

            var logger = loggerFactory.CreateLogger(this.GetType().FullName);

            this.SigningCertificate = signingCertificate;
            this.ServiceUri         = serviceUri;
            this.Logger             = logger;
        }

        public ILogger Logger { get; set; }

        public string ServiceUri { get; set; }

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
            this.Logger?.LogDebug($"[Event.SignedMessage] {requestString}");

            // sending WS-Security request
            using (var webClient = new WebClient())
            {
                //webClient.Proxy = new System.Net.WebProxy("http://localhost:8888");
                webClient.Encoding = Encoding.UTF8;
                if (!string.IsNullOrEmpty(request.SOAPAction))
                {
                    webClient.Headers.Add("SOAPAction", request.SOAPAction);
                }
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
                                this.Logger?.LogDebug($"[Event.SignedMessage] {responseFault}");

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
                    this.Logger?.LogDebug($"[Event.SignedMessage] {response}");

                    var responseHandler = new TResultResponseHandler();
                    return responseHandler.FromSOAP(response, out fault);
                }
                else
                {
                    throw new ServiceClientException(string.Format("Got en empty response from {0} at {1}", request.SOAPAction, serviceUrl));
                }
            }
        }
    }
}
