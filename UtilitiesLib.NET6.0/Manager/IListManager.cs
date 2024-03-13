using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesLib.NET6._0.Manager
{
    /// <summary>
    /// Interface for implementation by manager classes hosting a collection
    /// of the type List<T> where T can be any object type. The collection
    /// is here referred to as "list".
    /// Class implementing IListManager decide type of <T> at declaration,
    /// after that, T must have the same type in all methods included in 
    /// this interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListManager<T>
    {

        #region Methods Assignment C2A3 and below
        /// <summary>
        /// Return the number of items in the collection list
        /// </summary>
        int Count { get; }

        //---------------------------------------------------------------
        /// <summary>
        /// Add an object to the collection list
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Add(T type);

        /// <summary>
        /// Edit an object at index, in the collection list
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        bool ChangeAt(T type, int index);

        /// <summary>
        /// Check index is within range
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool CheckIndex(int index);

        /// <summary>
        /// Delete all object of the collection list
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Remove an object, at index, from the collection list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool DeleteAt(int index);

        /// <summary>
        /// Get an object at index, from the collection list 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T GetAt(int index);

        /// <summary>
        /// Get all objects of the collection list in an array of strings
        /// </summary>
        /// <returns></returns>
        string[] ToStringArray();

        /// <summary>
        /// Get all objects of the collection list in a List of strings 
        /// </summary>
        /// <returns></returns>
        List<string> ToStringList();
        #endregion Methods Assignment C2A3 and below
        // ****************************************************************
        #region Assignment C2A4
        void SaveBinGenList_T(string fileName);
        void OpenBinGenList_T(string fileName);
        void SaveXMLGenList_T(string fileName);
        void OpenXMLGenList_T(string fileName);
        #endregion Assignment C2A4

        //---------------------------------------------------------------
    }
}
