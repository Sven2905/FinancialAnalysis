using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FinancialAnalysis.Logic
{
    public static class XmlHelper
    {
        public static bool NewLineOnAttributes { get; set; }

        /// <summary>
        ///     Serializes an object to an XML string, using the specified namespaces.
        /// </summary>
        public static string ToXml(object obj, XmlSerializerNamespaces ns)
        {
            var T = obj.GetType();

            var xs = new XmlSerializer(T);
            var ws = new XmlWriterSettings
                {Indent = true, NewLineOnAttributes = NewLineOnAttributes, OmitXmlDeclaration = true};

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, ws))
            {
                xs.Serialize(writer, obj, ns);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Serializes an object to an XML string.
        /// </summary>
        public static string ToXml(object obj)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            return ToXml(obj, ns);
        }

        /// <summary>
        ///     Deserializes an object from an XML string.
        /// </summary>
        public static T FromXml<T>(string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(xml))
            {
                return (T) xs.Deserialize(sr);
            }
        }

        /// <summary>
        ///     Deserializes an object from an XML string, using the specified type name.
        /// </summary>
        public static object FromXml(string xml, string typeName)
        {
            var T = Type.GetType(typeName);
            var xs = new XmlSerializer(T);
            using (var sr = new StringReader(xml))
            {
                return xs.Deserialize(sr);
            }
        }

        /// <summary>
        ///     Serializes an object to an XML file.
        /// </summary>
        public static void ToXmlFile(object obj, string filePath)
        {
            var xs = new XmlSerializer(obj.GetType());
            var ns = new XmlSerializerNamespaces();
            var ws = new XmlWriterSettings
                {Indent = true, NewLineOnAttributes = NewLineOnAttributes, OmitXmlDeclaration = true};
            ns.Add("", "");

            using (var writer = XmlWriter.Create(filePath, ws))
            {
                xs.Serialize(writer, obj);
            }
        }

        /// <summary>
        ///     Deserializes an object from an XML file.
        /// </summary>
        public static T FromXmlFile<T>(string filePath)
        {
            var sr = new StreamReader(filePath);
            try
            {
                var result = FromXml<T>(sr.ReadToEnd());
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("There was an error attempting to read the file " + filePath + "\n\n" +
                                    e.InnerException.Message);
            }
            finally
            {
                sr.Close();
            }
        }
    }
}