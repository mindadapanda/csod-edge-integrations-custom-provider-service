using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service
{
    public class SerializationUtil
    {
        public static string SerializeXML<T>(T objProcessRequest, bool omitXmlRootDeclaration, bool omitRootNameSpace = false)
        {
            Dictionary<string, string> nSpaces = null;
            var writerSetting = new XmlWriterSettings { OmitXmlDeclaration = omitXmlRootDeclaration };

            if (omitRootNameSpace)
            {
                nSpaces = new Dictionary<string, string> { { "", "" } };
            }

            return SerializeXML(objProcessRequest, nSpaces, writerSetting);
        }

        public static string SerializeXML<T>(T objProcessRequest, Dictionary<string, string> nameSpaces, bool omitXmlRootDeclaration)
        {
            var writerSetting = new XmlWriterSettings { OmitXmlDeclaration = omitXmlRootDeclaration };

            return SerializeXML(objProcessRequest, nameSpaces, writerSetting);
        }

        public static string SerializeXML<T>(T objProcessRequest, Dictionary<string, string> nameSpaces, XmlWriterSettings writerSettings)
        {
            var nSpace = new XmlSerializerNamespaces();

            if (nameSpaces != null)
            {
                foreach (var item in nameSpaces)
                {
                    nSpace.Add(item.Key, item.Value);
                }
            }

            var serializer = new XmlSerializer(typeof(T));

            using (StringWriter textWriter = new Utf8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, writerSettings))
                {
                    serializer.Serialize(xmlWriter, objProcessRequest, nSpace);
                }
                var strXmlString = textWriter.ToString();
                return strXmlString;
            }
        }

        public static T DeserialiseXMLResponse<T>(string strEYModifiedResponse)
        {
            try
            {
                T result;
                var deSerializer = new XmlSerializer(typeof(T));


                using (TextReader reader = new StringReader(strEYModifiedResponse))
                {
                    result = (T)deSerializer.Deserialize(reader);
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public static bool TryDeserializeXML<T>(string xmlPayload, out T result)
        {
            bool isSuccess = true;

            try
            {
                var deSerializer = new XmlSerializer(typeof(T));

                using (TextReader reader = new StringReader(xmlPayload))
                {
                    result = (T)deSerializer.Deserialize(reader);
                }
            }
            catch
            {
                isSuccess = false;
                result = default(T);
            }
            return isSuccess;
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
