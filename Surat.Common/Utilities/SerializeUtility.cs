using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Surat.Common.Utilities
{
    public class SerializeUtility
    {

        #region Methods

        public static string SerializeToXML(object objectToSerialize)
        {
            XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType());
            StringBuilder stringBuilder = new StringBuilder();

            using(StringWriter stringWriter = new StringWriter(stringBuilder))
            {
                serializer.Serialize(stringWriter,objectToSerialize);
            }

            return stringBuilder.ToString();
        }

        public static object DeSerializeFromXML<T>(string serializedObject)
        {
            object objectDeserialized;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader stringReader = new StringReader(serializedObject))
            {
                objectDeserialized = serializer.Deserialize(stringReader);
            }

            return objectDeserialized;

        }
        #endregion

    }
}
