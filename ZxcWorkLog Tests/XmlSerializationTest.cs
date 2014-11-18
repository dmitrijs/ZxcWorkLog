using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZxcWorkLog.Data;
using ZxcWorkLog.Data.Serializable;

namespace ZxcWorkLog_Tests
{
    [TestClass]
    public class XmlSerializationTest
    {
        private static string BuildXml(string title)
        {
            var sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);

            var wic = new WorkItemCollection { { 1, new WorkItem() } };
            wic[1].Title = title;

            var wl = new XmlWorkLog(wic);
            new XmlSerializer(typeof(XmlWorkLog)).Serialize(tw, wl);

            return sb.ToString();
        }

        [TestMethod]
        public void TestXmlSerialization()
        {
            var xml = BuildXml("Sample Title");
            Assert.IsTrue(xml.Contains("<Title>Sample Title</Title>"));
        }

        [TestMethod]
        public void TestXmlDeserialization()
        {
            var xml = BuildXml("Sample Title");
            var r = XmlReader.Create(new StringReader(xml));

            var wll = (XmlWorkLog) new XmlSerializer(typeof(XmlWorkLog)).Deserialize(r);

            Assert.IsTrue(wll.Items.Count == 1);
            Assert.IsTrue(wll.Items[0].Title == "Sample Title");
        }
    }
}
