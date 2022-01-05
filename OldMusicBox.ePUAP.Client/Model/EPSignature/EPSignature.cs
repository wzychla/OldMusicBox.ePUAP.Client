using OldMusicBox.ePUAP.Client.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Signature
{
    [XmlRoot("EPSignature", Namespace = Namespaces.PODPIS_ZAUFANY)]
    public class EPSignature
    {
        [XmlElement("NaturalPerson", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public NaturalPerson NaturalPerson { get; set; }

        [XmlElement("SignatureData", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public SignatureData SignatureData { get; set; }

        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return this.NaturalPerson != null &&
                !string.IsNullOrEmpty(this.NaturalPerson.FirstName) &&
                !string.IsNullOrEmpty(this.NaturalPerson.CurrentFamilyName) &&
                !string.IsNullOrEmpty(this.NaturalPerson.PersonalIdentifier);
            }
        }

        public override string ToString()
        {
            var serializer = new XmlSerializer(typeof(EPSignature));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
    }

    public class NaturalPerson
    {
        [XmlElement("CurrentFamilyName", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public string CurrentFamilyName { get; set; }
        [XmlElement("FirstName", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public string FirstName { get; set; }
        [XmlElement("DateOfBirth", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public string DateOfBirth { get; set; }
        [XmlElement("PersonalIdentifier", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public string PersonalIdentifier { get; set; }
    }

    public class SignatureData
    {
        [XmlElement("IdentityIssuer", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public string IdentityIssuer { get; set; }
        [XmlElement("IdentityIssueTimestamp", Namespace = Namespaces.PODPIS_ZAUFANY)]
        public DateTime IdentityIssueTimestamp { get; set; }

    }
}
