using OldMusicBox.ePUAP.Client.Core.Constants;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Core.Model.TrustedProfileInfoForPESEL
{
    /// <summary>
    /// TrustedProfileInfoForPESELResponse
    /// </summary>
    [XmlRoot("respTrustedProfileInfoForPESEL", Namespace = Namespaces.OBJECTINFO)]
    public class TrustedProfileInfoForPESELResponse : IServiceResponse
    {
        [XmlAttribute("callId")]
        public string CallId { get; set; }

        [XmlAttribute("responseTimestamp")]
        public DateTime ResponseTimestamp { get; set; }

        [XmlElement("profile", Namespace = Namespaces.OBJECTINFO)]
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        [XmlElement("status", Namespace = Namespaces.OBJECTINFO)]
        public ProfileStatus Status { get; set; }

        [XmlElement("profileId", Namespace = Namespaces.OBJECTINFO)]
        public long ProfileId { get; set; }

        [XmlElement("userId", Namespace = Namespaces.OBJECTINFO)]
        public string UserId { get; set; }

        [XmlElement("firstName", Namespace = Namespaces.OBJECTINFO)]
        public string FirstName { get; set; }

        [XmlElement("secondName", Namespace = Namespaces.OBJECTINFO)]
        public string SecondName { get; set; }

        [XmlElement("lastName", Namespace = Namespaces.OBJECTINFO)]
        public string LastName { get; set; }

        [XmlElement("PESEL", Namespace = Namespaces.OBJECTINFO)]
        public string PESEL { get; set; }

        [XmlElement("email", Namespace = Namespaces.OBJECTINFO)]
        public string Email { get; set; }

        [XmlElement("PhoneNumber", Namespace = Namespaces.OBJECTINFO)]
        public string PhoneNumber { get; set; }

        [XmlElement("authMethodId", Namespace = Namespaces.OBJECTINFO)]
        public long AuthMethodId { get; set; }

        [XmlElement("authParam", Namespace = Namespaces.OBJECTINFO)]
        public string AuthParam { get; set; }

        [XmlElement("creationDate", Namespace = Namespaces.OBJECTINFO)]
        public System.DateTime CreationDate { get; set; }

        [XmlElement("expirationDate", Namespace = Namespaces.OBJECTINFO)]
        public System.DateTime ExpirationDate { get; set; }
    }

    public enum ProfileStatus
    {

        /// <remarks/>
        V,

        /// <remarks/>
        I,

        /// <remarks/>
        E,

        /// <remarks/>
        X,
    }
}
