using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model.Signature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.GetSignedDocument
{
    /// <summary>
    /// GetSignedDocument Response
    /// </summary>
    [XmlRoot("getSignedDocumentReturn", Namespace = "")]
    public class GetSignedDocument5Response : IServiceResponse
    {
        const string PROOFOFAPPROVAL = "http://uri.etsi.org/01903/v1.2.2#ProofOfApproval";

        [XmlText]
        public string Content { get; set; }

        [XmlIgnore]
        public IEnumerable<string> SigningCertificates
        {
            get
            {
                if ( string.IsNullOrEmpty( Content ) )
                {
                    return Enumerable.Empty<string>();
                }

                // first, decode the response
                var rawContent = Encoding.UTF8.GetString(Convert.FromBase64String(this.Content));

                // then read it
                var xml = new XmlDocument();
                xml.LoadXml( rawContent );

                // then find the user info
                var keyInfo              = xml.GetElementsByTagName( "KeyInfo", Namespaces.XMLDSIG ).OfType<XmlNode>().FirstOrDefault();
                var qualifyingProperties = xml.GetElementsByTagName( "QualifyingProperties", Namespaces.XADES ).OfType<XmlNode>().FirstOrDefault();

                if ( keyInfo != null && qualifyingProperties != null )
                {
                    var ns = new XmlNamespaceManager(xml.NameTable);
                    ns.AddNamespace( "ds", Namespaces.XMLDSIG );
                    ns.AddNamespace( "xades", Namespaces.XADES );
                    var identifier = qualifyingProperties.SelectNodes( "//xades:SignedProperties/xades:SignedDataObjectProperties/xades:CommitmentTypeIndication/xades:CommitmentTypeId/xades:Identifier", ns ).OfType<XmlNode>().FirstOrDefault();
                    if ( identifier != null && identifier.InnerText == PROOFOFAPPROVAL )
                    {
                        return
                            keyInfo
                                .SelectNodes( "//ds:X509Data/ds:X509Certificate", ns )
                                .OfType<XmlNode>()
                                .Select( node => node.InnerText );
                    }

                }

                // fallback - empty
                return Enumerable.Empty<string>();
            }
        }

        [XmlIgnore]
        public EPSignature Signature
        {
            get
            {
                if (string.IsNullOrEmpty(Content))
                {
                    return null;
                }

                // first, decode the response
                var rawContent = Encoding.UTF8.GetString(Convert.FromBase64String(this.Content));

                // then read it
                var xml = new XmlDocument();
                xml.LoadXml(rawContent);

                // then find the user info (new way)
                var signatures = xml.GetElementsByTagName("EPSignature", Namespaces.PODPIS_ZAUFANY);
                if (signatures.Count > 0)
                {
                    var signature = signatures.Item(0);

                    var serializer = new XmlSerializer(typeof(EPSignature));
                    using (var reader = new StringReader(signature.OuterXml))
                    {
                        return serializer.Deserialize(reader) as EPSignature;
                    }
                }

                // or find the user info (old way, required at the server from 10-03-2025!)
                // then find the user info
                var podpisZPs = xml.GetElementsByTagName("PodpisZP", Namespaces.PPZP);
                if ( podpisZPs.Count > 0 )
                {
                    var podpisZP = podpisZPs.Item(0);

                    var serializer = new XmlSerializer(typeof(GetTpUserInfo.PodpisZP));
                    using ( var reader = new StringReader( podpisZP.OuterXml ) )
                    {
                        var podpis = serializer.Deserialize( reader ) as GetTpUserInfo.PodpisZP;

                        return EPSignature.FromPodpisZP( podpis );
                    }
                }

                // fallback
                return null;
            }
        }
    }

    /// <summary>
    /// Checks if the three: given name, surname and PESEL are there in the signature
    /// </summary>
    /// <remarks>
    /// Removed. Są dwa rodzaje sygnatur: 
    /// * z podpisu profilem zaufanym (wtedy Signature != null)
    /// * z podpisu podpisem kwalifikowanym (wtedy SignatureCertificates zawiera łańcuch certyfikatów)
    ///   który należy sprawdzić pod kątem tego czy pierwszy jest kwalifikowany i zawiera imię/nazwisko/PESEL
    /// </remarks>
    /*
    [XmlIgnore]
    public bool IsValid 
    {
        get
        {
            return this.Signature != null && this.Signature.IsValid;
        }
    }
    */
    /*
    <ds:Object>
        <xades:QualifyingProperties xmlns:xades="http://uri.etsi.org/01903/v1.3.2#"
                                    Target="#id-041a8a6a24dad234b7c9f38510523d55">
            <xades:SignedProperties Id="xades-id-041a8a6a24dad234b7c9f38510523d55">
                <xades:SignedSignatureProperties>
                    <xades:SigningTime>2024-03-04T11:45:41Z</xades:SigningTime>
                    <xades:SigningCertificate>
                        <xades:Cert>
                            <xades:CertDigest>
                                <ds:DigestMethod Algorithm="http://www.w3.org/2001/04/xmlenc#sha256"/>
                                <ds:DigestValue>XRZEqB7aVrDHs+Iy11rZSBfz0GMQO/Sq+/1vZ1wKyqg=</ds:DigestValue>
                            </xades:CertDigest>
                            <xades:IssuerSerial>
                                <ds:X509IssuerName>2.5.4.97=#0c10564154504c2d35323630333030353137,CN=COPE SZAFIR - Kwalifikowany,O=Krajowa Izba Rozliczeniowa S.A.,C=PL</ds:X509IssuerName>
                                <ds:X509SerialNumber>85814820236245346760263045778883259920497286457</ds:X509SerialNumber>
                            </xades:IssuerSerial>
                        </xades:Cert>
                    </xades:SigningCertificate>
                </xades:SignedSignatureProperties>
                <xades:SignedDataObjectProperties>
                    <xades:DataObjectFormat ObjectReference="#ID-e9abc28dd0f14fb60d2082cf3798d433">
                        <xades:MimeType>text/xml</xades:MimeType>
                    </xades:DataObjectFormat>
                    <xades:CommitmentTypeIndication>
                        <xades:CommitmentTypeId>
                            <xades:Identifier>http://uri.etsi.org/01903/v1.2.2#ProofOfApproval</xades:Identifier>
                        </xades:CommitmentTypeId>
                        <xades:AllSignedDataObjects/>
                    </xades:CommitmentTypeIndication>
                </xades:SignedDataObjectProperties>
            </xades:SignedProperties>
        </xades:QualifyingProperties>
    */
}
