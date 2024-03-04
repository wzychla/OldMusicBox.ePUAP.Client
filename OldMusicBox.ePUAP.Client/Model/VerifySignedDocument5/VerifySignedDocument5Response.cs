using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Model.Signature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.VerifySignedDocument
{
    /// <summary>
    /// Verify signed document response
    /// </summary>
    [XmlRoot("verifySignedDocumentReturn", Namespace = "")]
    public class VerifySignedDocument5Response : IServiceResponse
    {
        [XmlText()]
        public string Content { get; set; }

        [XmlIgnore]
        public GetTpUserInfo.PodpisZP PodpisZP
        {
            get
            {
                if (this.IsValid)
                {
                    if (this.VerifyResult.StatusInfo.ZP != null &&
                        this.VerifyResult.StatusInfo.ZP.ClaimedRole != null 
                        )
                    {
                        return this.VerifyResult.StatusInfo.ZP.ClaimedRole.PodpisZP;
                    }
                }

                return null;
            }
        }

        [XmlIgnore]
        public EPSignature Signature
        {
            get
            {
                if (this.IsValid)
                {
                    if (this.VerifyResult.StatusInfo.EP != null &&
                        this.VerifyResult.StatusInfo.EP.ClaimedRole != null
                        )
                    {
                        return this.VerifyResult.StatusInfo.EP.ClaimedRole.Signature;
                    }
                }

                return null;
            }
        }

        [XmlIgnore]
        private VerifyResult5 _verifyResult;

        [XmlIgnore]
        public VerifyResult5 VerifyResult
        {
            get
            {
                if (string.IsNullOrEmpty(this.Content))
                {
                    return null;
                }

                if (_verifyResult == null)
                {
                    var serializer = new XmlSerializer(typeof(VerifyResult5));
                    using (var reader = new StringReader(this.Content))
                    {
                        _verifyResult = serializer.Deserialize(reader) as VerifyResult5;
                    }
                }

                return _verifyResult;
            }
        }

        /// <summary>
        /// Checks if the three: given name, surname and PESEL are there
        /// </summary>
        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return
                    this.VerifyResult != null &&
                    this.VerifyResult.StatusInfo != null &&
                    (
                        (
                        this.VerifyResult.StatusInfo.EP != null &&
                        this.VerifyResult.StatusInfo.EP.ClaimedRole != null &&
                        this.VerifyResult.StatusInfo.EP.ClaimedRole.Signature != null &&
                        this.VerifyResult.StatusInfo.EP.ClaimedRole.Signature.IsValid
                        )
                        ||
                        (
                        this.VerifyResult.StatusInfo.ZP != null &&
                        this.VerifyResult.StatusInfo.ZP.ClaimedRole != null &&
                        this.VerifyResult.StatusInfo.ZP.ClaimedRole.PodpisZP != null &&
                        this.VerifyResult.StatusInfo.ZP.ClaimedRole.PodpisZP.IsValid
                        )
                    );
            }
        }
    }

    #region Nested models


    [XmlRoot("VerifyResult", Namespace = "")]
    public class VerifyResult5
    {
        [XmlElement("ValidDocumentSignature", Namespace = "")]
        public ValidDocumentSignature5 ValidDocumentSignature { get; set; }

        [XmlElement("SignatureType", Namespace = "")]
        public string SignatureType { get; set; }

        [XmlElement("GenerationTime", Namespace = "")]
        public string GenerationTime { get; set; }

        [XmlElement("StatusInfo", Namespace = "")]
        public StatusInfo5 StatusInfo { get; set; }
    }

    public class ValidDocumentSignature5
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlText]
        public bool Value { get; set; }
    }

    public class StatusInfo5
    {
        [XmlElement("ValidSignature", Namespace = "")]
        public ValidDocumentSignature5 ValidSignature { get; set; }

        [XmlElement("VerifyStatus", Namespace = "")]
        public VerifyStatus5 VerifyStatus { get; set; }

        [XmlElement("VerifySignerCert", Namespace = "")]
        public VerifySignerCert5 VerifySignerCert { get; set; }

        [XmlElement("VerifySignerCertUsage", Namespace = "")]
        public VerifySignerCertUsage5 VerifySignerCertUsage { get; set; }

        [XmlElement("SignatureCertIssuer", Namespace = "")]
        public string SignatureCertIssuer { get; set; }

        [XmlElement("SignatureCertSerial", Namespace = "")]
        public string SignatureCertSerial { get; set; }

        [XmlElement("SignatureCertSubject", Namespace = "")]
        public string SignatureCertSubject { get; set; }

        [XmlElement("SigningTime", Namespace = "")]
        public string SigningTime { get; set; }

        [XmlElement("UriID", Namespace = "")]
        public UriID5[] UriID { get; set; }

        [XmlElement("SignatureTimeStamp", Namespace = "")]
        public TimeStamp5 SignatureTimeStamp { get; set; }

        [XmlElement("ArchiveTimeStamp", Namespace = "")]
        public TimeStamp5 ArchiveTimeStamp { get; set; }

        [XmlElement("ZP", Namespace = "")]
        public ZP ZP { get; set; }

        [XmlElement("EP", Namespace = "")]
        public EP5 EP { get; set; }
    }
    public class VerifyStatus5
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlText]
        public int Value { get; set; }
    }

    public class VerifySignerCert5
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlText]
        public int Value { get; set; }
    }

    public class VerifySignerCertUsage5
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlAttribute("kwalifikowany", Namespace = "")]
        public bool Kwalifikowany { get; set; }

        [XmlText]
        public int Value { get; set; }
    }

    public class UriID5
    {
        [XmlAttribute("lp", Namespace = "")]
        public string Lp { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class TimeStamp5
    {
        [XmlAttribute("znaczenie")]
        public string Znaczenie { get; set; }
    }

    public class EP5
    {
        [XmlAttribute("czy_obecny")]
        public bool CzyObecny { get; set; }

        [XmlElement("ClaimedRole", Namespace = Namespaces.XADES)]
        public ClaimedRole ClaimedRole { get; set; }
    }

    #endregion
}
