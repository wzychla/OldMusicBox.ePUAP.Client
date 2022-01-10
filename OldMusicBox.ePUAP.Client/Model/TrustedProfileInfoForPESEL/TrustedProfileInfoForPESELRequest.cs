using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.TrustedProfileInfoForPESEL
{
    /// <summary>
    /// TrustedProfileInfoForPESELRequest
    /// </summary>
    //[XmlRoot("reqTrustedProfileInfoForPESEL", Namespace = Namespaces.OBJECTINFO)]
    [XmlRoot("reqGetTrustedProfileInfoForPESEL", Namespace = Namespaces.OBJECTINFO)]    
    public class TrustedProfileInfoForPESELRequest : IServiceRequest
    {
        [XmlIgnore]
        public string SOAPAction
        {
            get
            {
                return "urn:getTrustedProfileInfoForPESEL";
            }
        }

        [XmlAttribute("callId")]
        public string CallId
        {
            get
            {
                return new Random().Next().ToString();
            }
            set
            {

            }
        }

        [XmlAttribute("requestTimestamp")]
        public DateTime RequestTimestamp
        {
            get
            {
                return DateTime.Now;
            }
            set
            {

            }
        }

        [XmlElement(ElementName = "PESEL", Namespace = Namespaces.OBJECTINFO)]
        public string PESEL { get; set; }

        [XmlElement(ElementName = "profileInfo", Namespace = Namespaces.OBJECTINFO)]
        public ProfileInfoEnum ProfileInfo { get; set; }

        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Profile info enum 
    /// </summary>
    public enum ProfileInfoEnum
    {
        VALID_ONLY,
        MOST_RECENT
    }
}
