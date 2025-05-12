using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model;
using OldMusicBox.ePUAP.Client.Core.Model.Headers;
using OldMusicBox.ePUAP.Client.Core.Model.Skrytka;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Request
{
    /// <summary>
    /// Generic model of a WS-Security compliant SOAP envelope
    /// </summary>
    [XmlRoot(Namespace = Namespaces.SOAPENVELOPE)]
    public class Envelope
    {
        // random generator
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        public Envelope()
        {
            this.Body   = new Body();
            this.Header = new Header();
        }

        [XmlElement("Header", Namespace = Namespaces.SOAPENVELOPE)]
        public Header Header { get; set; }

        [XmlElement("Body", Namespace = Namespaces.SOAPENVELOPE)]
        public Body Body { get; set; }

        /// <summary>
        /// Create signed representation of the envelope
        /// </summary>
        /// <remarks>
        /// The WS-Security compliant signature must be crafted specifically
        /// for ePUAP.
        /// 
        /// First and only documentation for this is the
        /// Instrukcja_dla_Integratora_DT.pdf from ePUAP website.
        /// 
        /// I've tried a WCF client with custom binding with no result.
        /// </remarks>
        public virtual string GetSignedXML(X509Certificate2 signatureCertificate)
        {
            // check
            if ( this.Header == null ||
                 this.Header.Security == null ||
                 this.Header.Security.BinarySecurityToken == null ||

                 this.Body == null ||
                 this.Body.Contents == null
                )
            {
                throw new ArgumentException("Can't compute signature of an incomplete Envelope");
            }
            if ( signatureCertificate == null ||
                 signatureCertificate.GetRSAPrivateKey() == null 
                )
            {
                throw new ArgumentException("Can't compute signature without actual certificate");
            }

            // set ids
            var binarySecurityTokenId = string.Format("X509-{0}", Guid.NewGuid().ToString());
            var bodyId = string.Format("id-{0}", _random.Next());

            this.Body.Id                                                  = bodyId;
            this.Header.Security.BinarySecurityToken.Id                   = binarySecurityTokenId;
            this.Header.Security.BinarySecurityToken.SignatureCertificate = signatureCertificate;

            // sign
            var xml        = this.GetXML();
            var xmlrequest = new XmlDocument();
            xmlrequest.LoadXml(xml);

            var signedXml        = new SignedXmlWithId(xmlrequest);

            signedXml.SigningKey                        = signatureCertificate.GetRSAPrivateKey();
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod        = SignedXml.XmlDsigRSASHA256Url;

            // reference
            var reference = new Reference();
            reference.Uri = string.Format("#{0}", bodyId);

            // transform
            var c14transform = new XmlDsigExcC14NTransform();
            reference.AddTransform(c14transform);
            reference.DigestMethod = SignedXml.XmlDsigSHA256Url;

            // keyinfo
            var keyInfo   = new KeyInfo();
            keyInfo.Id    = string.Format("KI-{0}", _random.Next());
            var str       = new SecurityTokenReference();
            str.Reference = string.Format( "#{0}", binarySecurityTokenId );
            str.ValueType = Namespaces.WS_SEC_TOKENPROFILE;

            keyInfo.AddClause(str);
            signedXml.KeyInfo = keyInfo;

            // compose signature
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            // import the signature node into the document
            var signatureXml = signedXml.GetXml();

            var headerNode = xmlrequest
                .DocumentElement.ChildNodes
                .Cast<XmlNode>()
                .FirstOrDefault(n => n.LocalName == "Header");
            var headerSecurityNode = headerNode
                        .ChildNodes
                        .Cast<XmlNode>()
                        .FirstOrDefault(n => n.LocalName == "Security");
            var binarySecurityTokenNode = headerSecurityNode.FirstChild;

            // insert it just after BSE
            var importedSignatureXml = xmlrequest.ImportNode(signatureXml, true);
            headerSecurityNode.InsertAfter(importedSignatureXml, binarySecurityTokenNode);

            return xmlrequest.OuterXml;
        }

        /// <summary>
        /// Gets the envelope as string
        /// </summary>
        /// <returns></returns>
        public virtual string GetXML()
        {
            // first, serialize to string
            var sb = new StringBuilder();
            var xs = new XmlSerializer(typeof(Envelope));
            using (var sw = new StringWriter8(sb))
            {
                xs.Serialize(sw, this);
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// BinarySecurityToken element
    /// </summary>
    public class BinarySecurityToken
    {
        [XmlAttribute("Id", Namespace = Namespaces.WS_SEC_UTILITY)]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "EncodingType", Namespace = "")]
        public string EncodingType
        {
            get
            {
                return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
            }
            set
            {

            }
        }

        [XmlAttribute(AttributeName = "ValueType", Namespace = "")]
        public string ValueType
        {
            get
            {
                return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
            }
            set
            {
                // an empty setter is required for the serializer
            }
        }

        /// <summary>
        /// Signature certificate as base64
        /// </summary>
        [XmlText]
        public string CertificateBase64
        {
            get
            {
                if (this.SignatureCertificate != null)
                {
                    return Convert.ToBase64String(this.SignatureCertificate.Export(X509ContentType.Cert), Base64FormattingOptions.None);
                }
                else
                    return string.Empty;
            }
            set
            {

            }
        }

        [XmlIgnore]
        public X509Certificate2 SignatureCertificate { get; set; }
    }

    /// <summary>
    /// SOAP Body model
    /// </summary>
    public class Body : IXmlSerializable
    {
        public IServiceRequest Contents { get; set; }

        public string Id { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {

        }

        public void WriteXml(XmlWriter writer)
        {
            if (!string.IsNullOrEmpty(this.Id))
            {
                writer.WriteAttributeString("Id", Namespaces.WS_SEC_UTILITY, this.Id);
            }

            if (this.Contents != null)
            {
                var settings                = new XmlWriterSettings();
                var serializer              = new XmlSerializer(this.Contents.GetType());
                settings.OmitXmlDeclaration = true;

                using (var stringWriter = new StringWriter8())
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, this.Contents);

                    writer.WriteRaw(stringWriter.ToString());
                }
            }
        }
    }

    public class Header
    {
        public Header()
        {
            this.Security = new Security();
        }

        [XmlElement("Security", Namespace = Namespaces.WS_SEC_EXT)]
        public Security Security { get; set; }

        [XmlElement("AdresSkrytki",           typeof(AdresSkrytkiHeaderAttribute),           Namespace = Namespaces.OBI)]
        [XmlElement("AdresOdpowiedzi",        typeof(AdresOdpowiedziHeaderAttribute),        Namespace = Namespaces.OBI)]
        [XmlElement("CzyProbne",              typeof(CzyProbneHeaderAttribute),              Namespace = Namespaces.OBI)]
        [XmlElement("DaneDodatkowe",          typeof(DaneDodatkoweHeaderAttribute),          Namespace = Namespaces.OBI)]
        [XmlElement("IdentyfikatorDokumentu", typeof(IdentyfikatorDokumentuHeaderAttribute), Namespace = Namespaces.OBI)]
        [XmlElement("IdentyfikatorPodmiotu",  typeof(IdentyfikatorPodmiotuHeaderAttribute),  Namespace = Namespaces.OBI)]
        [XmlElement("IdentyfikatorSprawy",    typeof(IdentyfikatorSprawyHeaderAttribute),    Namespace = Namespaces.OBI)]
        [XmlElement("NazwaPliku",             typeof(NazwaPlikuHeaderAttribute),             Namespace = Namespaces.OBI)]
        [XmlElement("TerminDoreczenia",       typeof(TerminDoreczeniaHeaderAttribute),       Namespace = Namespaces.OBI)]
        public HeaderAttribute[] Attributes { get; set; }

        private XmlSerializerNamespaces _xmlns;

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns
        {
            get
            {
                if (_xmlns == null)
                {
                    _xmlns = new XmlSerializerNamespaces();
                    _xmlns.Add("obi", Namespaces.OBI);
                }
                return _xmlns;
            }

            set
            {
                _xmlns = value;
            }
        }

    }

    public class Security
    {
        public Security()
        {
            this.BinarySecurityToken = new BinarySecurityToken();
        }

        [XmlElement("BinarySecurityToken", Namespace = Namespaces.WS_SEC_EXT)]
        public BinarySecurityToken BinarySecurityToken { get; set; }
    }

    /// <summary>
    /// Base class for additional header attribute classes
    /// </summary>
    [XmlRoot(Namespace = "")]
    public abstract class HeaderAttribute
    {

    }
}
