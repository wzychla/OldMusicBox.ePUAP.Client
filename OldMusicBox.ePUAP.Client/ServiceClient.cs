using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model;
using OldMusicBox.ePUAP.Client.Model.Fault;
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

            var requestFactory = new RequestFactory(this.SigningCertificate);
            var requestString  = requestFactory.CreateRequest(request);

            // call ePUAP service and parse the response
            var response = WSSecurityRequest(serviceUrl, SoapActions.GETTPUSERINFO, requestString, GetTpUserInfoResponse.FromSOAP, out fault);

            // parse response
            return response;
        }

        #endregion

        #region Generic call 

        public TResult WSSecurityRequest<TResult>( 
            string serviceUrl, 
            string soapAction,
            string request,
            Func<string, TResult> converter,
            out FaultModel fault)
            where TResult : class, IServiceResponse
        {
            fault = null;

            if ( string.IsNullOrEmpty( serviceUrl ) ||
                 string.IsNullOrEmpty( soapAction ) ||
                 string.IsNullOrEmpty( request ) ||
                 converter == null
                )
            {
                throw new ArgumentNullException("Can't call a service with incomplete parameters");
            }

            // log
            new LoggerFactory().For(this).Debug(Event.SignedMessage, request);

            // sending WS-Security request
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("SOAPAction", soapAction);
                webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";

                string response = null;
                try
                {
                    // POST it
                    response = webClient.UploadString(serviceUrl, request );
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

                                fault = FaultModel.FromSOAP(responseFault);

                                return null;
                            }
                        }
                    }

                    // fallback
                    throw new ServiceClientException(string.Format("Client call failed for {0} at {1}", soapAction, serviceUrl ), ex);
                }

                if (!string.IsNullOrEmpty(response))
                {
                    // log
                    new LoggerFactory().For(this).Debug(Event.SignedMessage, response);

                    return converter(response);
                }
                else
                {
                    throw new ServiceClientException(string.Format( "Got en empty response from {0} at {1}", soapAction, serviceUrl));
                }
            }
        }

        #endregion
    }
}
