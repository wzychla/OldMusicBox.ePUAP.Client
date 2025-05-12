using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.ePUAP.Client.Core
{
    /// <summary>
    /// UTF-8 String writer
    /// </summary>
    public class StringWriter8 : StringWriter
    {
        public StringWriter8() : base() { }

        public StringWriter8(StringBuilder sb) : base(sb) { }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
