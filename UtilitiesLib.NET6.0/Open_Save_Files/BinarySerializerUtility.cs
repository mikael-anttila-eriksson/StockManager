using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesLib.NET6._0.Open_Save_Files
{
    public class BinarySerializerUtility
    {
        // ****************************************************************
        #region Binary Serializer
        /// <summary>
        /// Save object to filePath using Binary Serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        public static void BinaryFileSerialize<T>(string filePath, T obj)
        {
            FileStream fileStream = null;


            // set filePath and that
            // fileStream should should created a new file
            fileStream = new FileStream(filePath, FileMode.Create);

            // Create Bin Serializer
            BinaryFormatter binFormat = new BinaryFormatter();

            try
            {
                // Choose which Stream to use and
                // which object to Serialize
                binFormat.Serialize(fileStream, obj);
            }
            // xxx OBS! Let Exception fall to the GUI!!! xxxxxxxxxxxxx
            // i.e. no try-catch-block here
            finally
            {
                // Close resources if they were used(open)
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }







        }
        /// <summary>
        /// Open object with Binary DeSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns>Generic (T)object</returns>
        public static T BinaryFileDeSerialize<T>(string filePath)
        {
            FileStream fileStream = null;
            //errorMsg = null;
            Object obj = null;

            // set file path and
            // decide what fileStream should do
            // open an existing file
            fileStream = new FileStream(filePath, FileMode.Open);

            // Create Bin DeSerializer
            BinaryFormatter binFormat = new BinaryFormatter();

            try
            {
                // DeSerialize choosen Stream into an object
                obj = binFormat.Deserialize(fileStream);
            }
            // xxx OBS! Let Exception fall to the GUI!!! xxxxxxxxxxxxx
            // i.e. no try-catch-block here
            finally
            {
                // Close resources
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }


            return (T)obj;

        }

        #endregion Binary Serializer
    }
}
