using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase
{
    public interface IXmlDatabase
    {

        //  METHODS

        #region DATA MANAGEMENT METHODS

        /// <summary> Insert new object into XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="dataModel"> DataModel instance to insert into XML Database. </param>
        /// <returns> True - if data model instance was inserted corretly into database, False - otherwise. </returns>
        bool AddObject<TDataModel>(TDataModel dataModel) where TDataModel : DataModel;

        /// <summary> Remove all type objects from XML Database. </summary>
        void ClearDatabase();

        /// <summary> Remove all objects from XML Database with specified data model type. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        void ClearObject<TDataModel>() where TDataModel : DataModel;

        /// <summary> Get object from XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="identifier"> DataModel instance identifier. </param>
        /// <returns> DataModel instance from database or null if it not exist. </returns>
        TDataModel GetObject<TDataModel>(string identifier) where TDataModel : DataModel;

        /// <summary> Get list of objects from XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="expression"> Linq query expression conditions for filtering data. </param>
        /// <returns> List of DataModel instancse from database or empty if they not exist. </returns>
        List<TDataModel> GetObjects<TDataModel>(Expression<Func<TDataModel, bool>> expression = null)
            where TDataModel : DataModel;

        /// <summary> Check if XML Database contains specific data model. </summary>
        /// <typeparam name="TDataModel"> DataModel type to check. </typeparam>
        /// <returns> True - data model exists in database, False - data modes does not exist in database. </returns>
        bool HasDataModel<TDataModel>() where TDataModel : DataModel;

        /// <summary> Check if XML Database has registered data model. </summary>
        /// <typeparam name="TDataModel"> DataModel type to check. </typeparam>
        /// <returns> True - data model is registered, False - data model is not registered. </returns>
        bool HasRegisteredDataModel<TDataModel>() where TDataModel : DataModel;

        /// <summary> Check if XML Database contains data model with specified identifier. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="identifier"> DataModel instance identifier. </param>
        /// <returns> True - data model with specified identifier exists in database, False - otherwise. </returns>
        bool HasObject<TDataModel>(string identifier) where TDataModel : DataModel;

        /// <summary> Remove object from XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="dataModel"> DataModel instance to remove from XML Database. </param>
        /// <returns> True - if data model instance was removed corretly from database, False - otherwise. </returns>
        bool RemoveObject<TDataModel>(TDataModel dataModel) where TDataModel : DataModel;

        /// <summary> Update object in XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="dataModel"> DataModel instance to update in XML Database. </param>
        /// <returns> True - if data model instance was updated corretly into database, False - otherwise. </returns>
        bool UpdateObject<TDataModel>(TDataModel dataModel) where TDataModel : DataModel;

        #endregion DATA MANAGEMENT METHODS

        #region DATA SAVE METHODS

        /// <summary> Save root XML root XElement database to file. </summary>
        /// <param name="filePath"> Path to save XML file. </param>
        void SaveToFile(string filePath);

        #endregion DATA SAVE METHODS

        #region SETUP METHODS

        /// <summary> Register the data models to be the data structures for the xml file. </summary>
        /// <typeparam name="TDataModel"> Type of data model inherited from DataModel. </typeparam>
        void RegisterDataModel<TDataModel>() where TDataModel : DataModel;

        #endregion SETUP METHODS

        #region VERSION MANAGEMENT

        /// <summary> Get version of XML database file. </summary>
        /// <returns> Version of XML database file. </returns>
        XMLDatabaseVersion GetFileVersion();

        /// <summary> Get max version of required XML database file. </summary>
        /// <returns> Max required version of XML database file. </returns>
        XMLDatabaseVersion GetCurrentVersion();

        /// <summary> Get minimal version of required XML database file. </summary>
        /// <returns> Minimal required version of XML database file. </returns>
        XMLDatabaseVersion GetMinimalVersion();

        #endregion VERSION MANAGEMENT

    }
}
