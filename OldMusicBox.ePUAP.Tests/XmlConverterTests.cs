using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldMusicBox.ePUAP.Client.Model.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Tests
{
    [TestClass]
    public class XmlConverterTests
    {
        [TestMethod]
        public void SimpleXmlModel()
        {
            // arrange
            var foo = new Foo()
            {
                Bar = new Bar()
                {
                    Qux = "17"
                }
            };

            // act
            var xml = foo.ToXmlDocument();

            // assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.OuterXml.Length > 0);
        }
    }

    public class Foo
    {
        public Bar Bar { get; set; }
    }

    public class Bar
    {
        [XmlAttribute]
        public string Qux { get; set; }
    }
}
