using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AonHewitt.Lib
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public T Deserialize<T>(string value)
        {
            T result;
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(value))
            {
                result = (T)serializer.Deserialize(reader);
                return result;
            }
        }

        public string Serialize<T>(T @object)
        {
            //our xml string to return
            string strXMLString;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            };
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, @object);
                    strXMLString = textWriter.ToString();
                    return strXMLString;
                }
            }
        }
    }
}
