using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace YAPI
{
    public class xml
    {

        public static byte[] ToXML(object instance)
        {
            MemoryStream memoryStream = new MemoryStream();
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(instance.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xmlTextWriter.Formatting = Formatting.Indented;
            x.Serialize(xmlTextWriter, instance);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

            return memoryStream.ToArray();
        }

        public static T FromXML<T>(byte[] data)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(data);
            XmlTextReader xmlTextWriter = new XmlTextReader(memoryStream);

            return (T)xs.Deserialize(xmlTextWriter);
        }
    }
}
