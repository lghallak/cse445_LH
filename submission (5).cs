using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Text;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://YOUR_GITHUB_USERNAME.github.io/cse445_a4/NationalParks.xml";
        public static string xmlErrorURL = "https://YOUR_GITHUB_USERNAME.github.io/cse445_a4/NationalParksErrors.xml";
        public static string xsdURL = "https://YOUR_GITHUB_USERNAME.github.io/cse445_a4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
            StringBuilder errors = new StringBuilder();

            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                string xsdContent = DownloadContent(xsdUrl);
                using (StringReader sr = new StringReader(xsdContent))
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    schemaSet.Add(null, xr);
                }

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors.AppendLine(e.Message);
                };

                string xmlContent = DownloadContent(xmlUrl);
                using (StringReader sr = new StringReader(xmlContent))
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read()) { }
                }
            }
            catch (Exception ex)
            {
                errors.AppendLine(ex.Message);
            }

            if (errors.Length == 0)
                return "No errors are found";
            else
                return errors.ToString().TrimEnd();
        }

        public static string Xml2Json(string xmlUrl)
        {
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            string xmlContent = DownloadContent(xmlUrl);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, false);
            return jsonText;
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

}