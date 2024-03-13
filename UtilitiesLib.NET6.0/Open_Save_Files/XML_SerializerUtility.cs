using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UtilitiesLib.NET6._0.Open_Save_Files
{
    public class XML_SerializerUtility
    {
        /// <summary>
        /// Save object to fileName using xml serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public static void XMLFileSerialize<T>(string fileName, T obj)
        {
            //create serializer
            XmlSerializer xmlS = new XmlSerializer(typeof(T));
            // create xml Writer; write to save file
            TextWriter writer = new StreamWriter(fileName);

            try
            {
                // Specify Writer to use and which object to
                // serialize to an XML file
                xmlS.Serialize(writer, obj);
            }
            // xxx OBS! Let Exception fall to the GUI!!! xxxxxxxxxxxxx
            // i.e. no try-catch-block here
            finally
            {
                if (writer != null)
                {
                    // Close resource
                    writer.Close();
                }
            }






        }
        /// <summary>
        /// Read XML-file to xml-DeSerialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns>
        /// Generic (T)object
        /// </returns>
        public static T XMLFileDeSerialize<T>(string fileName)
        {
            // Create object to read to
            Object obj = null;

            // create serializer
            XmlSerializer xmlS = new XmlSerializer(typeof(T));
            // create xml Reader; read from file
            TextReader reader = new StreamReader(fileName);

            try
            {
                // Specify Reader to use when reading 
                // from the XML file
                obj = xmlS.Deserialize(reader);
            }
            // xxx OBS! Let Exception fall to the GUI!!! xxxxxxxxxxxxx
            // i.e. no try-catch-block here
            finally
            {
                // Close resource
                if (reader != null)
                {
                    reader.Close();
                }

            }


            return (T)obj;
        }
    }
}
