using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.XML
{
    /// <summary>
    /// Generic XML converter
    /// </summary>
    public static class XmlConverter
    {
        /// <summary>
        /// Try to convert any class to XML
        /// </summary>
        public static XmlDocument ToXmlDocument<T>( this T dokument )
            where T : class
        {
            var xs = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                xs.Serialize(ms, dokument);

                ms.Seek(0, SeekOrigin.Begin);

                var xd = new XmlDocument();
                xd.Load(ms);

                return xd;
            }
        }
    }
}
