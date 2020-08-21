using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.ePUAP.Client.XAdES
{
    /// <summary>
    /// Simple XAdES compatible signature
    /// </summary>
    public class XAdESBESSigner : IXMLSigner
    {
        public const string XADES_Prefix           = "xades";

        /// <summary>
        /// XAdES SignedProperties
        /// </summary>
        public const string XADES_SignedProperties = "http://uri.etsi.org/01903#SignedProperties";

        private const string SignatureId           = "Signature";
        private const string SignaturePropertiesId = "SignedProperties";

        /// <summary>
        /// Create XAdES-BES compatible signature and optionally embedd it in the document.
        /// Return the signature
        /// </summary>
        public XmlElement Sign( XmlDocument document, X509Certificate2 certificate, bool embeddSignature = true )
        {
            if (document == null || certificate == null) return null;

            var signature          = new XAdESSignedXml(document);
            signature.Signature.Id = SignatureId;
            signature.SigningKey   = certificate.PrivateKey;

            AddSignatureReference(signature);
            AddKeyInfo(certificate, signature);
            AddXAdESProperties(document, certificate, signature);

            // compute the signature
            signature.ComputeSignature();
            var signatureXml = signature.GetXml();

            // import the signature into the document
            if (signatureXml != null && embeddSignature )
            {
                document.DocumentElement.AppendChild( document.ImportNode(signatureXml, true));
            }

            return signatureXml;
        }

        private void AddSignatureReference(XAdESSignedXml signature)
        {
            var signatureReference = new Reference { Uri = "", };
            signatureReference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            //var c14t = new XmlDsigC14NTransform();
            //signatureReference.AddTransform(c14t);
            signature.AddReference(signatureReference);
        }

        private void AddKeyInfo(X509Certificate2 certificate, XAdESSignedXml signature)
        {
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificate));
            signature.KeyInfo = keyInfo;
        }

        /// <summary>
        /// Create simplest XAdES-BES compliant ds:Object
        /// Based on https://stackoverflow.com/questions/50096199/c-sharp-how-to-properly-sign-message-with-xades-using-signedxml
        /// </summary>
        private void AddXAdESProperties(XmlDocument document, X509Certificate2 certificate, XAdESSignedXml signature)
        {
            var xadesReference = new Reference
            {
                Uri  = string.Format( "#{0}", SignaturePropertiesId ),
                Type = XADES_SignedProperties
            };
            //var c14t = new XmlDsigC14NTransform();
            //xadesReference.AddTransform(c14t);
            signature.AddReference(xadesReference);

            // <Object>
            var objectNode = document.CreateElement("Object", SignedXml.XmlDsigNamespaceUrl);

            // <Object><QualifyingProperties>
            var qualifyingPropertiesNode = document.CreateElement(XADES_Prefix, "QualifyingProperties", Namespaces.XADES);
            qualifyingPropertiesNode.SetAttribute("Target", string.Format( "#{0}", SignatureId));
            objectNode.AppendChild(qualifyingPropertiesNode);

            // <Object><QualifyingProperties><SignedProperties>
            var signedPropertiesNode = document.CreateElement(XADES_Prefix, "SignedProperties", Namespaces.XADES);
            signedPropertiesNode.SetAttribute("Id", SignaturePropertiesId);
            qualifyingPropertiesNode.AppendChild(signedPropertiesNode);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties>
            var signedSignaturePropertiesNode = document.CreateElement(XADES_Prefix, "SignedSignatureProperties", Namespaces.XADES);
            signedPropertiesNode.AppendChild(signedSignaturePropertiesNode);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties> </SigningTime>
            var signingTime = document.CreateElement(XADES_Prefix, "SigningTime", Namespaces.XADES);
            signingTime.InnerText = DateTime.UtcNow.ToString("s")+"Z";
            signedSignaturePropertiesNode.AppendChild(signingTime);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate>
            var signingCertificateNode = document.CreateElement(XADES_Prefix, "SigningCertificate", Namespaces.XADES);
            signedSignaturePropertiesNode.AppendChild(signingCertificateNode);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate><Cert>
            var certNode = document.CreateElement(XADES_Prefix, "Cert", Namespaces.XADES);
            signingCertificateNode.AppendChild(certNode);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate><Cert><CertDigest>
            var certDigestNode = document.CreateElement(XADES_Prefix, "CertDigest", Namespaces.XADES);
            certNode.AppendChild(certDigestNode);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate><Cert><CertDigest> </DigestMethod>
            var digestMethod                          = document.CreateElement("DigestMethod", SignedXml.XmlDsigNamespaceUrl);
            var digestMethodAlgorithmAtribute         = document.CreateAttribute("Algorithm");
            digestMethodAlgorithmAtribute.InnerText   = SignedXml.XmlDsigSHA1Url;
            //digestMethodAlgorithmAtribute.InnerText = SignedXml.XmlDsigSHA256Url;
            digestMethod.Attributes.Append(digestMethodAlgorithmAtribute);
            certDigestNode.AppendChild(digestMethod);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate><Cert><CertDigest> </DigestMethod>
            var digestValue         = document.CreateElement("DigestValue", SignedXml.XmlDsigNamespaceUrl);
            digestValue.InnerText   = Convert.ToBase64String(certificate.GetCertHash());
            //digestValue.InnerText = Convert.ToBase64String(new SHA256Managed().ComputeHash(signingCertificate.GetRawCertData()));
            certDigestNode.AppendChild(digestValue);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate><Cert><IssuerSerial>
            var issuerSerialNode = document.CreateElement(XADES_Prefix, "IssuerSerial", Namespaces.XADES);
            certNode.AppendChild(issuerSerialNode);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate><Cert><IssuerSerial> </X509IssuerName>
            var x509IssuerName       = document.CreateElement("X509IssuerName", SignedXml.XmlDsigNamespaceUrl);
            x509IssuerName.InnerText = certificate.Issuer;
            issuerSerialNode.AppendChild(x509IssuerName);

            // <Object><QualifyingProperties><SignedProperties><SignedSignatureProperties><SigningCertificate><Cert><IssuerSerial> </X509SerialNumber>
            var x509SerialNumber       = document.CreateElement("X509SerialNumber", SignedXml.XmlDsigNamespaceUrl);
            x509SerialNumber.InnerText = certificate.SerialNumber.ToDecimalString();
            issuerSerialNode.AppendChild(x509SerialNumber);

            // register newly created nodes
            var dataObject = new DataObject();
            dataObject.Data = qualifyingPropertiesNode.SelectNodes(".");
            signature.RegisterObject(dataObject);
        }
    }

    public static class StringExtensions
    {
        // parse big int
        public static string ToDecimalString(this string serialNumber)
        {
            BigInteger bi;

            if (BigInteger.TryParse(serialNumber, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out bi))
            {
                return bi.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return serialNumber;
            }
        }

    }
}
