using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Seeger.Utils
{
    public static class XmlSerializerUtil
    {
        public static string Serialize(object obj)
        {
            Require.NotNull(obj, "obj");

            var type = obj.GetType();
            var serializer = new XmlSerializer(type);
            var builder = new StringBuilder();

            using (var writer = XmlWriter.Create(builder, new XmlWriterSettings{ Indent = true, IndentChars = "\t" }))
            {
                serializer.Serialize(writer, obj);
                writer.Flush();
            }

            return builder.ToString();
        }

        public static T Deserialize<T>(string str)
        {
            Require.NotNullOrEmpty(str, "str");

            var type = typeof(T);
            var serializer = new XmlSerializer(type);

            using (var reader = new StringReader(str))
            {
                return (T) serializer.Deserialize(reader);
            }
        }
    }
}
