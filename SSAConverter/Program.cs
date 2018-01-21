using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SSAConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "-e":
                    switch (Path.GetExtension(args[1]).ToLower())
                    {
                        case ".ssa":
                            File.WriteAllText(args[1] + ".xml", ToXmlString(SSA.FromByteArray(File.ReadAllBytes(args[1]))));
                            break;
                    }
                    break;
                case "-i":
                    switch (Path.GetExtension(args[1]).ToLower())
                    {
                        case ".xml":
                            File.WriteAllBytes(args[1] + ".ssa", SSA.ToByteArray(FromXmlString<SSA>(File.ReadAllText(args[1]))));
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Usable modes are:\n-e\n-i");
                    break;
            }
        }

        public static string ToXmlString<T>(T input)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var xmlSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                NewLineOnAttributes = false,
                IndentChars = "\t",
                CheckCharacters = false,
                OmitXmlDeclaration = true
            };
            using (var sw = new StringWriter())
            {
                new XmlSerializer(typeof(T)).Serialize(XmlWriter.Create(sw, xmlSettings), input, ns);
                return sw.ToString();
            }
        }

        public static T FromXmlString<T>(string xml)
        {
            using (var sr = new StringReader(xml))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(sr);
            }
        }
    }
}
