using OldMusicBox.ePUAP.Client.Core.Constants;
using OldMusicBox.ePUAP.Client.Core.Model.Signature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.VerifySignedDocument
{
    /// <summary>
    /// Verify signed document response
    /// </summary>
    [XmlRoot("verifySignedDocumentResponse", Namespace = Namespaces.COMARCH_SIGN)]
    public class VerifySignedDocumentResponse : IServiceResponse
    {
        [XmlElement("verifySignedDocumentReturn", Namespace = "")]
        public VerifySignedDocumentReturn Return { get; set; }

        /// <summary>
        /// LoD wrapper over the inner information
        /// </summary>
        [XmlIgnore]
        public GetTpUserInfo.PodpisZP Podpis
        {
            get
            {
                if (this.Return != null && this.Return.IsValid)
                {
                    return this.Return.PodpisZP;
                }
                else
                {
                    return null;
                }
            }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return
                    this.Podpis != null &&
                    this.Podpis.IsValid;
            }
        }

    }

    #region Nested models

    [XmlRoot("verifySignedDocumentReturn", Namespace = "")]
    public class VerifySignedDocumentReturn
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
                    return this.VerifyResult.StatusInfo.ZP.ClaimedRole.PodpisZP;
                }
                else
                {
                    return null;
                }
            }
        }

        [XmlIgnore]
        private VerifyResult _verifyResult;

        [XmlIgnore]
        public VerifyResult VerifyResult
        {
            get
            {
                if (string.IsNullOrEmpty(this.Content))
                {
                    return null;
                }

                if (_verifyResult == null)
                {
                    var serializer = new XmlSerializer(typeof(VerifyResult));
                    using (var reader = new StringReader(this.Content))
                    {
                        _verifyResult = serializer.Deserialize(reader) as VerifyResult;
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
                    this.VerifyResult.StatusInfo.ZP != null &&
                    this.VerifyResult.StatusInfo.ZP.ClaimedRole != null &&
                    this.VerifyResult.StatusInfo.ZP.ClaimedRole.PodpisZP != null &&
                    this.VerifyResult.StatusInfo.ZP.ClaimedRole.PodpisZP.IsValid;
            }
        }
    }

    [XmlRoot("VerifyResult", Namespace = "")]
    public class VerifyResult
    {
        [XmlElement("ValidDocumentSignature", Namespace = "")]
        public ValidDocumentSignature ValidDocumentSignature { get; set; }

        [XmlElement("SignatureType", Namespace = "")]
        public string SignatureType { get; set; }

        [XmlElement("GenerationTime", Namespace = "")]
        public string GenerationTime { get; set; }

        [XmlElement("StatusInfo", Namespace = "")]
        public StatusInfo StatusInfo { get; set; }
    }

    public class ValidDocumentSignature
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlText]
        public bool Value { get; set; }
    }

    public class StatusInfo
    {
        [XmlElement("ValidSignature", Namespace = "")]
        public ValidDocumentSignature ValidSignature { get; set; }

        [XmlElement("VerifyStatus", Namespace = "")]
        public VerifyStatus VerifyStatus { get; set; }

        [XmlElement("VerifySignerCert", Namespace = "")]
        public VerifySignerCert VerifySignerCert { get; set; }

        [XmlElement("VerifySignerCertUsage", Namespace = "")]
        public VerifySignerCertUsage VerifySignerCertUsage { get; set; }

        [XmlElement("SignatureCertIssuer", Namespace = "")]
        public string SignatureCertIssuer { get; set; }

        [XmlElement("SignatureCertSerial", Namespace = "")]
        public string SignatureCertSerial { get; set; }

        [XmlElement("SignatureCertSubject", Namespace = "")]
        public string SignatureCertSubject { get; set; }

        [XmlElement("SigningTime", Namespace = "")]
        public string SigningTime { get; set; }

        [XmlElement("ZP")]
        public ZP ZP { get; set; }
    }

    public class VerifyStatus
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlText]
        public int Value { get; set; }
    }

    public class VerifySignerCert
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlText]
        public int Value { get; set; }
    }

    public class VerifySignerCertUsage
    {
        [XmlAttribute("znaczenie", Namespace = "")]
        public string Znaczenie { get; set; }

        [XmlAttribute("kwalifikowany", Namespace = "")]
        public bool Kwalifikowany { get; set; }

        [XmlText]
        public int Value { get; set; }
    }

    public class ZP
    {
        [XmlAttribute("czy_obecny")]
        public bool CzyObecny { get; set; }

        [XmlElement("ClaimedRole", Namespace = Namespaces.XADES)]
        public ClaimedRole ClaimedRole { get; set; }
    }

    public class ClaimedRole
    {
        [XmlElement("PodpisZP", Namespace = Namespaces.PPZP)]
        public GetTpUserInfo.PodpisZP PodpisZP { get; set; }

        [XmlElement("EPSignature", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public EPSignature Signature { get; set; }

    }

    #endregion
}
