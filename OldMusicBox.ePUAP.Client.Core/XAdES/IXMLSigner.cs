using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.Core.XAdES
{
    public interface IXMLSigner
    {
        XmlElement Sign(XmlDocument document, X509Certificate2 certificate, bool embeddSignature = true);
    }
}
