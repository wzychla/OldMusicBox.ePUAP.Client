using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// NET Core extensions
    /// </summary>
    public static class ClientServiceCollectionExtensions
    {
        public static void AddePUAPClients( this IServiceCollection services )
        {
            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new DoreczycielClient( serviceUriProvider.DoreczycielUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new FileRepoServiceClient( serviceUriProvider.FileRepoServiceUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new ObslugaUPPClient( serviceUriProvider.ObslugaUPPUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new PullClient( serviceUriProvider.PullUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new ServiceClient( serviceUriProvider.GetTpUserInfoUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new SkrytkaClient( serviceUriProvider.SkrytkaUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new TpSigningClient( serviceUriProvider.TpSigningUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new TpSigning5Client( serviceUriProvider.TpSigning5Uri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new TpUserObjectsInfoClient( serviceUriProvider.TpUserObjectsInfoUri, certificateProvider, loggerFactory );

                return client;
            } );

            services.AddTransient( services =>
            {
                var serviceUriProvider  = services.GetRequiredService<IServiceUriProvider>();
                var certificateProvider = services.GetRequiredService<ICertificateProvider>();
                var loggerFactory       = services.GetRequiredService<ILoggerFactory>();

                var client = new ZarzadzanieDokumentamiClient( serviceUriProvider.ZarzadzanieDokumentamiUri, certificateProvider, loggerFactory );

                return client;
            } );
        }


    }
}
