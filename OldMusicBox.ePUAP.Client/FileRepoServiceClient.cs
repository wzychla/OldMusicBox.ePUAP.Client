using OldMusicBox.ePUAP.Client.Model.Common;
using OldMusicBox.ePUAP.Client.Model.FileRepoService;
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client
{
    /// <summary>
    /// WS-FileRepoService client
    /// 
    /// Służy do wysyłania/pobierania załączników do dokumentów
    /// </summary>
    /// <remarks>
    /// Production uri:  https://ws.epuap.gov.pl/repo-ws-ext/FileRepoService
    /// Integration uri: https://ws-int.epuap.gov.pl/repo-ws-ext/FileRepoService
    /// WSDL:            https://ws.epuap.gov.pl/repo-ws-ext/FileRepoService/WEB-INF/wsdl/filerepo.wsdl
    /// </remarks>
    public class FileRepoServiceClient : BaseClient
    {
        public const string INTEGRATION_URI = "https://ws-int.epuap.gov.pl/repo-ws-ext/FileRepoService";
        public const string PRODUCTION_URI  = "https://ws.epuap.gov.pl/repo-ws-ext/FileRepoService";

        public FileRepoServiceClient(string serviceUri, X509Certificate2 signingCertificate) : base(serviceUri, signingCertificate)
        {

        }

        #region UploadFile

        /// <summary>
        /// Interfejs służy do wysyłania załącznika/dokumentu do repozytorium plików
        /// </summary>
        /// <param name="podmiot">Identyfikator podmiotu</param>
        /// <param name="nazwaSkrytki">Nazwa sprawdzanej skrytki</param>
        /// <param name="adresSkrytki">Adres sprawdzanej skrytki</param>
        public virtual UploadFileResponse UploadFile(
            string file,
            string filename,
            string mimeType,
            string subject,
            out FaultModel fault
            )
        {
            // validation
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");
            if (string.IsNullOrEmpty(mimeType))
                throw new ArgumentNullException("mimeType");

            var request = new UploadFileRequest()
            {
                File     = file,
                Filename = filename,
                MimeType = mimeType,
                Subject  = subject
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<UploadFileRequest, UploadFileResponse, UploadFileResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion

        #region DownloadFile

        /// <summary>
        /// Interfejs służy do pobierania załącznika/dokumentu z repozytorium plików
        /// </summary>
        /// <param name="fileId">Identyfikator pliku/załącznika</param>
        public virtual DownloadFileResponse DownloadFile(
            string fileId,
            string podmiot,
            out FaultModel fault
            )
        {
            // validation
            if (string.IsNullOrEmpty(fileId))
                throw new ArgumentNullException("fileId");

            //string queryString = new System.Uri(fileId).Query;
            //var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

            var request = new DownloadFileRequest()
            {
                //mozna podac caly link
                //FileId = queryDictionary["fileId"],
                FileId = fileId,
                Subject = podmiot
            };

            // call ePUAP service and parse the response
            var response = WSSecurityRequest<DownloadFileRequest, DownloadFileResponse, DownloadFileResponseHandler>(
                this.ServiceUri,
                request,
                out fault);

            // parsed response
            return response;
        }

        #endregion

    }
}
