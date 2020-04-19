using OldMusicBox.ePUAP.Client.Constants;
using OldMusicBox.ePUAP.Client.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.GetTpUserInfo
{
    /// <summary>
    /// GetTpUserInfo Request
    /// </summary>
    [XmlRoot("getTpUserInfo", Namespace = Namespaces.USERINFO)]
    public class GetTpUserInfoRequest : IServiceRequest
    {
        public GetTpUserInfoRequest()
        {
            this.SystemOrganisationId = "0";
        }

        [XmlIgnore]
        public string SOAPAction
        {
            get
            {
                return "getTpUserInfo";
            }
        }

        [XmlElement(ElementName = "tgsid", Namespace = "")]
        public string TgSid { get; set; }

        [XmlElement(ElementName = "systemOrganisationId", Namespace = "")]
        public string SystemOrganisationId { get; set; }

        public HeaderAttribute[] HeaderAttributes
        {
            get
            {
                return null;
            }
        }
    }
}
