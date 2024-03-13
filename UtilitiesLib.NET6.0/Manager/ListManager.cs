using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitiesLib.NET6._0.Open_Save_Files;

namespace UtilitiesLib.NET6._0.Manager
{
    /// <summary>
    /// Generic List<T, with protected field list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListManager<T> : IListManager<T>
    {
        //Fields
        protected List<T> list;
        //---------------------------------------------------------------
        #region Constructor
        /// <summary>
        /// Base constructor. Create a new list of T.
        /// </summary>
        public ListManager()
        {
            list = new List<T>();
        }
        #endregion
        //---------------------------------------------------------------
        #region Properties
        public int Count
        {
            get { return list.Count; }
        }
        /// <summary>
        /// Get/Set the List of type T.
        /// </summary>
        public List<T> List
        {
            get { return list; }
            set
            {
                if (value != null)
                {
                    list = value;
                }

            }
        }
        #endregion
        //---------------------------------------------------------------
        #region Methods
        // ****************************************************************
        #region Comparison

        /// <summary>
        /// Call classes that implement IComparer to sort type T
        /// </summary>
        /// <param name="sorter"></param>
        public void Sort(IComparer<T> sorter)
        {
            list.Sort(sorter);
        }
        #endregion Comparison
        // ****************************************************************
        /// <summary>
        /// If T type is not null, add T to list.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Add(T type)
        {
            if (type != null)
            {
                list.Add(type);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Change T if index is within range.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ChangeAt(T type, int index)
        {
            if (CheckIndex(index))
            {
                list[index] = type;
                return true;
            }
            // else
            return false;
        }

        public bool CheckIndex(int index)
        {
            if (index >= 0 && index < list.Count)
            {
                return true;
            }
            //else
            return false;
        }

        public void DeleteAll()
        {
            list.Clear();
        }
        /// <summary>
        /// Remove an object, at index, if index within range.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool DeleteAt(int index)
        {
            if (CheckIndex(index))
            {
                list.RemoveAt(index);
                return true;
            }
            // else
            return false;
        }
        /// <summary>
        /// Get T at index, if index within range.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetAt(int index)
        {
            if (CheckIndex(index))
            {
                return list[index];
            }
            //else
            return default(T);
        }
        /// <summary>
        /// Convert List<T> to string[] through ToString().
        /// </summary>
        /// <returns>
        /// An array of strings.
        /// </returns>
        public string[] ToStringArray()
        {

            string[] info = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                info[i] = list.ToString();
            }
            return info;
        }
        /// <summary>
        /// Convert List to a list of strings through ToString().
        /// </summary>
        /// <returns>
        /// A list of strings.
        /// </returns>
        public List<string> ToStringList()
        {
            List<string> listString = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                listString.Add(list[i].ToString());
            }
            return listString;
        }
        // ****************************************************************
        #region Assignment 4

        /// <summary>
        /// Store a generic List T in a binary file
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveBinGenList_T(string fileName)
        {
            // Call Utility
            BinarySerializerUtility.
                BinaryFileSerialize<List<T>>(fileName, list);

        }
        /// <summary>
        /// Load generic List T from binary file to a calling List
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void OpenBinGenList_T(string fileName)
        {
            // Call Utility
            list = BinarySerializerUtility.
                BinaryFileDeSerialize<List<T>>(fileName);

        }
        /// <summary>
        /// Save generic List T to XML file
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveXMLGenList_T(string filePath)
        {
            // Call utility
            XML_SerializerUtility.XMLFileSerialize<List<T>>(filePath, list);
        }

        /// <summary>
        /// Load generic List T from XML file
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenXMLGenList_T(string filePath)
        {
            // Call Utility
            list = XML_SerializerUtility.XMLFileDeSerialize<List<T>>(filePath);
        }

        #endregion Assignment 4
        // ****************************************************************
        #endregion
        //---------------------------------------------------------------
    }
}
