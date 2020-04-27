using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldMusicBox.ePUAP.Client.Request;
using OldMusicBox.ePUAP.Tests.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.ePUAP.Tests
{
    /// <summary>
    /// Basic tests for WS-* SOAP envelope conformance
    /// </summary>
    [TestClass]
    public class EnvelopeTests
    {
        /// <summary>
        /// Test if body serializes non-empty content
        /// </summary>
        [TestMethod]
        public void BodyContainsContents()
        {
            // arrange
            var id = Guid.NewGuid().ToString();

            var body = new Body();
            body.Contents = new ExampleRequest() { Id = id };

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                var serializer = new XmlSerializer(typeof(Body));

                // act
                serializer.Serialize(writer, body);
            }

            // assert

            var xml = sb.ToString();
            Assert.IsTrue(xml.Contains(id));
        }

        /// <summary>
        /// Test if body serializes non-empty id
        /// </summary>
        [TestMethod]
        public void BodyContainsId()
        {
            // arrange
            var id = Guid.NewGuid().ToString();

            var body = new Body();
            body.Id  = id;

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                var serializer = new XmlSerializer(typeof(Body));

                // act
                serializer.Serialize(writer, body);
            }

            // assert

            var xml = sb.ToString();
            Assert.IsTrue(xml.Contains(id));
        }
    }
}
