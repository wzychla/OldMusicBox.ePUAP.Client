using Microsoft.Extensions.Hosting.Internal;
using System.Security.Cryptography.X509Certificates;

namespace OldMusicBox.ePUAP.Client.Core.Demo
{
    public class Program
    {
        public static void Main( string[] args )
        {
            // container

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddLogging( options =>
            {
                options.AddConsole();
            } );

            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<ICertificateProvider, CertificateProvider>();
            builder.Services.AddTransient<IServiceUriProvider, IntegrationServiceUriProvider>();
            builder.Services.AddePUAPClients();

            var app = builder.Build();

            // middlewares
            app.UseRouting();
            app.UseSession(); 

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute( "default", "{controller=Home}/{action=Index}/{Id?}" );

            app.Run();
        }
    }

    /// <summary>
    /// Demo certificate provider. Replace with your own in production
    /// </summary>
    public class CertificateProvider : ICertificateProvider
    {
        private IConfiguration _configuration;

        public CertificateProvider( IConfiguration configuration )
        {
            this._configuration = configuration;
        }

        private static X509Certificate2 _clientCertificate;

        public X509Certificate2 GetCertificate()
        {
            if ( _clientCertificate == null )
            {
                var p12location = this._configuration["P12Location"];
                var p12password = this._configuration["P12Password"];

                using ( var fs = File.Open( p12location, FileMode.Open, FileAccess.Read, FileShare.Read ) )
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read( bytes, 0, bytes.Length );

                    _clientCertificate = new X509Certificate2( bytes, p12password, X509KeyStorageFlags.Exportable );
                }
            }

            return _clientCertificate;
        }
    }
}
