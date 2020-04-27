using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Tests
{
    public class TestCertProvider
    {
        private static X509Certificate2 _clientCertificate;

        public X509Certificate2 GetClientCertificate()
        {
            if (_clientCertificate == null)
            {
                var stream = typeof(TestCertProvider).Assembly.GetManifestResourceStream("OldMusicBox.ePUAP.Tests.testcert.p12");
                using (var fs = new BinaryReader(stream))
                {
                    byte[] bytes = new byte[stream.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    _clientCertificate = new X509Certificate2(bytes, "1234", X509KeyStorageFlags.Exportable);
                }
            }

            return _clientCertificate;
        }
    }
}
