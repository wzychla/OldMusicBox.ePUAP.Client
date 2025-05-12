using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Client.Model.Core.XML
{
    /// <summary>
    /// Generic XML converter
    /// </summary>
    public static class XmlConverter
    {
        /// <summary>
        /// Try to convert any class to XML
        /// </summary>
        public static XmlDocument ToXmlDocument<T>( this T dokument, XmlSerializerNamespaces namespaces = null )
            where T : class
        {
            if ( namespaces == null )
            {
                namespaces = new XmlSerializerNamespaces();
            }

            var xs = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream())
            using (var xw = XmlWriter.Create(ms, new XmlWriterSettings { Indent = false, Encoding = new UTF8Encoding(false) }))
            {
                xs.Serialize(xw, dokument, namespaces);

                ms.Seek(0, SeekOrigin.Begin);

                var xd = new XmlDocument();
                xd.PreserveWhitespace = true;
                xd.Load(ms);

                return xd;
            }
        }
    }
}
