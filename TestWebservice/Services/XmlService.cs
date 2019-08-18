using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TestWebservice.Models;

namespace TestWebservice.Services
{
    public static class XmlService
    {
        public static XDocument GenerateXml(IEnumerable<PersonOperation> list)
        {
            var doc = new XElement("Operation");

            foreach (var operation in list)
            {
                var element = new XElement("PersonOperation");
                element.Add(
                    new XElement("Name", operation.Name),
                    new XElement("Phone", operation.Phone),
                    new XElement("City", operation.City),
                    new XElement("Date", operation.Date),
                    new XElement("OperationType", operation.OperationType),
                    new XElement("Account", operation.Account),
                    new XElement("Amount", operation.Amount),
                    new XElement("AmountEUR", operation.AmountEUR)
                    );
                doc.Add(element);
            }
            
            return new XDocument(doc) ;
        }

        public static void SaveXmlToFile(XDocument document, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            
            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings
            {
                Encoding = Encoding.Default ,
                Indent = true,
                NewLineOnAttributes = true
            })) document.Save(writer);
        }

        public static string PostXmlTo(string destinationUrl, string xml)
        {
            if (string.IsNullOrEmpty(destinationUrl) || 
                string.IsNullOrEmpty(xml))
                return string.Empty;

            var bytes = Encoding.Default.GetBytes(xml);

            var request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            request.ContentType = "text/xml; encoding='windows-1251'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            request.UserAgent = @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";
            request.Referer = @"http://www.somesite.com/";
            request.Headers.Add("Accept-Language", "ru-RU");
            
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            string responseStr;
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK) return null;
                using (var responseStream = response.GetResponseStream())
                {
                    responseStr = new StreamReader(responseStream ?? throw new NullReferenceException()).ReadToEnd();
                }
            }
            
            return responseStr;
        }
    }
}