using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace TestWebservice.Services
{
    public static class CvsService
    {
        public static string XmlToCsv(string xmlFile, string operationType, string cvsFile)
        {
            if (string.IsNullOrEmpty(xmlFile) ||
                string.IsNullOrEmpty(operationType) ||
                string.IsNullOrEmpty(cvsFile))
                return string.Empty;
            
            var str = "";
            var nodes = new List<XElement>();
            
            ReadXml(xmlFile, operationType, nodes);

            str = GenStrFromNodes(nodes, str);
            
            WriteStrToCsvFile(cvsFile, str);
            
            return str;
            
        }

        private static void ReadXml(string xmlFile, string operationType, List<XElement> nodes)
        {
            using (var reader = XmlReader.Create(xmlFile))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element) continue;
                    if (reader.Name != "PersonOperation") continue;
                    if (!(XNode.ReadFrom(reader) is XElement el)) continue;

                    nodes.AddRange(from node in el.Descendants("OperationType")
                        where node.Value == operationType
                        where node.Parent != null
                        select node.Parent);
                }
            }
        }

        private static void WriteStrToCsvFile(string cvsFile, string str)
        {
            using (var writer = new StreamWriter(new FileStream(cvsFile,
                FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine(str);
            }
        }

        private static string GenStrFromNodes(IEnumerable<XElement> nodes, string str)
        {
            foreach (var node in nodes)
            {
                str += node.Descendants("Name").FirstOrDefault()?.Value + ";";
                str += node.Descendants("Phone").FirstOrDefault()?.Value + ";";
                str += node.Descendants("City").FirstOrDefault()?.Value + ";";
                str += node.Descendants("Account").FirstOrDefault()?.Value + ";";
                str += node.Descendants("Amount").FirstOrDefault()?.Value + ";";
                str += node.Descendants("AmountEUR").FirstOrDefault()?.Value + ";";
                str += node.Descendants("Date").FirstOrDefault()?.Value + ";";
                str += "\n";
            }

            return str;
        }
    }
}