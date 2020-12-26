using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using Tools.XMLDatabase;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.Exceptions;
using Tools.XMLDatabase.Statics;
using Tools.XMLDatabase.Tools;

namespace chkam05.DotNetTools.XMLDatabase
{
    public class XmlDatabase : IXmlDatabase
    {

        //  VARIABLES

        private XmlDatabaseOptions _options = null;
        private XElement _root = null;

        private List<Type> _dataModels;


        //  METHODS

        #region CLASS METHODS

        /// <summary> XmlDatabase class constructor. </summary>
        /// <param name="options"> Optional XML database options. </param>
        public XmlDatabase(XmlDatabaseOptions options = null)
        {
            //  Create clean XML root data.
            _root = new XElement(XmlDatabaseStatics.XmlDatabaseRoot);

            //  Run setup method.
            Setup(options);

            //  Setup XML database version.
            VersionHandler.SetVersion(_root, _options.CurrentVersion?.AsDictionary());
        }

        /// <summary> XmlDatabase class from file constructor. </summary>
        /// <param name="xmlFilePath"> Path to XML file. </param>
        /// <param name="options"> Optional XML database options. </param>
        public XmlDatabase(string xmlFilePath, XmlDatabaseOptions options = null)
        {
            //  Load XML data from file as root XElement.
            _root = DataImporter.FromFile(xmlFilePath);

            //  Run setup method.
            Setup(options);

            //  Checkout XML database version.
            var (correctVersion, versionError) = CheckoutVersion();

            //  Raise exception if database version is not valid.
            if (!correctVersion)
                throw new IncorrectXmlVersionException(versionError);
        }

        #endregion CLASS METHODS

        #region DATA MANAGEMENT METHODS

        /// <summary> Insert new object into XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="dataModel"> DataModel instance to insert into XML Database. </param>
        /// <returns> True - if data model instance was inserted corretly into database, False - otherwise. </returns>
        public bool AddObject<TDataModel>(TDataModel dataModel)
            where TDataModel : DataModel
        {
            //  Check if object is not null.
            if (dataModel != null)
            {
                //  Check if database contains this specific data model.
                if (HasDataModel<TDataModel>())
                {
                    //  Get target database node that contains particular type of objects.
                    var node = _root.Element(typeof(TDataModel).Name);

                    //  Check if object does not exist in database.
                    if (!node.Elements().Any(
                        item => item.Attribute(XmlDatabaseStatics.XmlAttributeIdentifier).Value == dataModel.Id))
                    {
                        //  Insert object into database.
                        node.Add(dataModel.AsXml(_options));
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary> Remove all type objects from XML Database. </summary>
        public void ClearDatabase()
        {
            //  Create removed data iterator.
            foreach (var node in _root.Elements())
            {
                //  Check if data model is not version data model then skip.
                if (node.Name == typeof(XMLDatabaseVersion).Name)
                    continue;

                //  Remove child data model instances from this node.
                node.Elements().Remove();
            }
        }

        /// <summary> Remove all objects from XML Database with specified data model type. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        public void ClearObject<TDataModel>() where TDataModel : DataModel
        {
            //  Get type of data model.
            Type type = typeof(TDataModel);

            //  Check if data model is not version data model then skip.
            if (type.Name == typeof(XMLDatabaseVersion).Name)
                return;

            //  Check if database contains this specific data model.
            if (HasDataModel<TDataModel>())
            {
                //  Get target database node that contains particular type of objects.
                var node = _root.Element(type.Name);

                node.Elements().Remove();
            }
        }

        /// <summary> Get object from XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="identifier"> DataModel instance identifier. </param>
        /// <returns> DataModel instance from database or null if it not exist. </returns>
        public TDataModel GetObject<TDataModel>(string identifier)
            where TDataModel : DataModel
        {
            //  Get type of data model.
            Type type = typeof(TDataModel);

            //  Check if identifier is correct.
            if (!string.IsNullOrEmpty(identifier))
            {
                //  Check if database contains this specific data model.
                if (HasDataModel<TDataModel>())
                {
                    //  Get target database node that contains particular type of objects.
                    var node = _root.Element(typeof(TDataModel).Name);

                    //  Get current instances of data model object from database with specified identifier.
                    var items = node.Elements().Where(
                        item => item.Attribute(XmlDatabaseStatics.XmlAttributeIdentifier).Value == identifier);

                    //  If any object instances exists in database - return first.
                    if (items != null && items.Any())
                    {
                        return (TDataModel)Activator.CreateInstance(type, new object[] { items.FirstOrDefault(), _options });
                    }
                }
            }

            return null;
        }

        /// <summary> Get list of objects from XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="expression"> Linq query expression conditions for filtering data. </param>
        /// <returns> List of DataModel instancse from database or empty if they not exist. </returns>
        public List<TDataModel> GetObjects<TDataModel>(Expression<Func<TDataModel, bool>> expression = null)
            where TDataModel : DataModel
        {
            //  Get type of data model.
            Type type = typeof(TDataModel);

            //  Check if database contains this specific data model.
            if (HasDataModel<TDataModel>())
            {
                //  Get target database node that contains particular type of objects.
                var node = _root.Element(typeof(TDataModel).Name);

                //  Get all objects with particular DataModel type from database.
                var instances = from item in node.Elements()
                                select (TDataModel)Activator.CreateInstance(type, new object[] { item, _options });

                //  If any object instances exists in database - use filter and return.
                if (instances != null && instances.Any())
                {
                    //  Use filter if exist.
                    var results = expression != null ? instances.AsQueryable().Where(expression) : instances;

                    if (results != null)
                    {
                        try
                        {
                            if (results.Count() > 0)
                                return results.ToList();
                        }
                        catch
                        {
                            try
                            {
                                if (results.Any())
                                    return new List<TDataModel>() { results.FirstOrDefault() };
                            }
                            catch
                            {
                                //  Just go to the end of the function.
                            }
                        }
                    }
                }
            }

            return new List<TDataModel>();
        }

        /// <summary> Check if XML Database contains specific data model. </summary>
        /// <typeparam name="TDataModel"> DataModel type to check. </typeparam>
        /// <returns> True - data model exists in database, False - data modes does not exist in database. </returns>
        public bool HasDataModel<TDataModel>()
            where TDataModel : DataModel
        {
            //  Get type of data model.
            Type type = typeof(TDataModel);

            //  Return result whether the database contains selected data model.
            return _root.Elements().Any(item => item.Name == type.Name);
        }

        /// <summary> Check if XML Database has registered data model. </summary>
        /// <typeparam name="TDataModel"> DataModel type to check. </typeparam>
        /// <returns> True - data model is registered, False - data model is not registered. </returns>
        public bool HasRegisteredDataModel<TDataModel>()
            where TDataModel : DataModel
        {
            //  Get type of data model.
            Type type = typeof(TDataModel);

            //  Return result whether the selected data model has been registered.
            return _dataModels.Contains(type);
        }

        /// <summary> Check if XML Database contains data model with specified identifier. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="identifier"> DataModel instance identifier. </param>
        /// <returns> True - data model with specified identifier exists in database, False - otherwise. </returns>
        public bool HasObject<TDataModel>(string identifier)
            where TDataModel : DataModel
        {
            //  Get type of data model.
            Type type = typeof(TDataModel);

            //  Check if identifier is correct.
            if (!string.IsNullOrEmpty(identifier))
            {
                //  Check if database contains this specific data model.
                if (HasRegisteredDataModel<TDataModel>())
                {
                    //  Get target database node that contains particular type of objects.
                    var node = _root.Element(typeof(TDataModel).Name);

                    //  Check if object exist in database.
                    return node.Elements().Any(
                        item => item.Attribute(XmlDatabaseStatics.XmlAttributeIdentifier).Value == identifier);
                }
            }

            return false;
        }

        /// <summary> Remove object from XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="dataModel"> DataModel instance to remove from XML Database. </param>
        /// <returns> True - if data model instance was removed corretly from database, False - otherwise. </returns>
        public bool RemoveObject<TDataModel>(TDataModel dataModel)
            where TDataModel : DataModel
        {
            //  Check if object is not null.
            if (dataModel != null)
            {
                //  Check if database contains this specific data model.
                if (HasDataModel<TDataModel>())
                {
                    //  Get target database node that contains particular type of objects.
                    var node = _root.Element(typeof(TDataModel).Name);

                    //  Get current instances of data model object from database.
                    var items = node.Elements().Where(
                        item => item.Attribute(XmlDatabaseStatics.XmlAttributeIdentifier).Value == dataModel.Id);

                    //  If any object instances exists in database - remove it.
                    if (items != null && items.Any())
                    {
                        items.Remove();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary> Update object in XML Database. </summary>
        /// <typeparam name="TDataModel"> Type of data model. </typeparam>
        /// <param name="dataModel"> DataModel instance to update in XML Database. </param>
        /// <returns> True - if data model instance was updated corretly into database, False - otherwise. </returns>
        public bool UpdateObject<TDataModel>(TDataModel dataModel)
            where TDataModel : DataModel
        {
            //  Check if object is not null.
            if (dataModel != null)
            {
                //  Check if database contains this specific data model.
                if (HasDataModel<TDataModel>())
                {
                    //  Get target database node that contains particular type of objects.
                    var node = _root.Element(typeof(TDataModel).Name);

                    //  Get current instances of data model object from database.
                    var items = node.Elements().Where(
                        item => item.Attribute(XmlDatabaseStatics.XmlAttributeIdentifier).Value == dataModel.Id);

                    //  If any object instances exists in database - update objects in database.
                    if (items != null && items.Any())
                    {
                        items.Remove();
                        node.Add(dataModel.AsXml(_options));
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion DATA MANAGEMENT METHODS

        #region DATA SAVE METHODS

        /// <summary> Save root XML root XElement database to file. </summary>
        /// <param name="filePath"> Path to save XML file. </param>
        public void SaveToFile(string filePath)
        {
            //  Save current XML root data into file.
            DataExporter.ToFile(_root, filePath);
        }

        #endregion DATA SAVE METHODS

        #region SETUP METHODS

        /// <summary> Register the data models to be the data structures for the xml file. </summary>
        /// <typeparam name="TDataModel"> Type of data model inherited from DataModel. </typeparam>
        public void RegisterDataModel<TDataModel>() where TDataModel : DataModel
        {
            //  Get type.
            var type = typeof(TDataModel);

            //  Validate data model type with properties.
            ValidateDataModel(type);

            //  Check if type is alredy registred.
            if (_dataModels.Contains(type))
                throw new ArgumentException($"Selected {type.Name} is alredy registered.");

            //  Register type.
            _dataModels.Add(type);

            //  Register type in root XElement.
            if (!HasDataModel<TDataModel>())
                _root.Add(new XElement(type.Name));
        }

        /// <summary> Setup XML database with options. </summary>
        /// <param name="options"> XML database options. </param>
        private void Setup(XmlDatabaseOptions options = null)
        {
            //  Initialize variables.
            _dataModels = new List<Type>();

            //  Setup XML database options.
            if (options == null)
                _options = new XmlDatabaseOptions();
            else
                _options = options;
        }

        #endregion SETUP METHODS

        #region TYPES VALIDATION METHODS
        
        /// <summary> Validate data model type with properties. </summary>
        /// <param name="dataModelType"> Type of data model. </param>
        private void ValidateDataModel(Type dataModelType)
        {
            //  Validate type of data model.
            if (!dataModelType.IsClass || !typeof(DataModel).IsAssignableFrom(dataModelType))
                throw new InvalidDataModelException(dataModelType);

            //  Get properties
            var properties = dataModelType.GetProperties(XmlDatabaseStatics.PropertyTypes);

            //  Validate properties of data model.
            foreach (var propertyInfo in properties)
            {
                //  Ignore virtual properties.
                if (propertyInfo.GetGetMethod().IsVirtual)
                    continue;

                //  Get type of single data model property.
                var propertyType = propertyInfo.PropertyType;

                //  Validate type of property.
                var (isCorrect, innerType) = ValidatePropertyType(propertyType);

                if (!isCorrect)
                    throw new InvalidDataModelPropertyException(propertyType, innerType);
            }
        }

        /// <summary> Validate single property type of data model that will be registered. </summary>
        /// <param name="propertyType"> Type of property from data model. </param>
        /// <returns> Tuple: (Is type correct, additional inner type). </returns>
        private (bool, Type) ValidatePropertyType(Type propertyType)
        {
            //  Check if property type is enumerable type.
            if (propertyType.IsEnum)
                return (true, null);

            //  Check if property type is array type.
            else if (propertyType.IsArray)
            {
                return ValidateInnerPropertyType(propertyType, propertyType.GetElementType());
            }

            //  Check if property type is nullable type.
            else if (propertyType.IsGenericType && Nullable.GetUnderlyingType(propertyType) != null)
            {
                return ValidateInnerPropertyType(propertyType, Nullable.GetUnderlyingType(propertyType));
            }

            //  Check if property type is dictionary type.
            else if (propertyType.IsGenericType && propertyType.GetInterfaces().Contains(typeof(IDictionary)))
            {
                //  Check key dictionary property.
                var (keyResult, keyInner) = ValidateInnerKeyPropertyType(propertyType, propertyType.GetGenericArguments()[0]);
                if (!keyResult)
                    return (keyResult, keyInner);

                //  Check value dictionary property.
                var (valueResult, valueInner) = ValidateInnerPropertyType(propertyType, propertyType.GetGenericArguments()[1]);
                if (!valueResult)
                    return (valueResult, valueInner);

                //  Return a ok.
                return (true, null);
            }

            //  Check if property type is ICollection/list type.
            else if (propertyType.IsGenericType && propertyType.GetInterfaces().Contains(typeof(ICollection)))
            {
                return ValidateInnerPropertyType(propertyType, propertyType.GetGenericArguments()[0]);
            }

            //  Check if property type is string.
            else if (propertyType == typeof(string))
            {
                return (true, null);
            }

            //  Check if property type is another class type - throw InvalidDataModelProperty.
            else if (propertyType.IsClass)
            {
                return (false, null);
            }

            //  Other inside properties
            else
            {
                return (true, null);
            }
        }

        /// <summary> Validate key property type of dictionary property of data model that will be registered. </summary>
        /// <param name="propertyType"> Type of dictionary property from data model. </param>
        /// <param name="KeyPropertyType"> Type of property inside enumerable or nullable property. </param>
        /// <returns> Tuple: (Is type correct, additional inner type). </returns>
        private (bool, Type) ValidateInnerKeyPropertyType(Type propertyType, Type KeyPropertyType)
        {
            //  Check if inside key property type is nullable type - throw InvalidDataModelProperty.
            if (KeyPropertyType.IsGenericType && KeyPropertyType.GetInterfaces().Contains(typeof(INullable)))
            {
                return (false, KeyPropertyType);
            }

            //  Preform other checks on inside key property.
            else
            {
                return ValidateInnerPropertyType(propertyType, KeyPropertyType);
            }
        }

        /// <summary> Validate property type of enumerable or nullable property of data model that will be registered. </summary>
        /// <param name="propertyType"> Type of enumerable or nullable property from data model. </param>
        /// <param name="innerPropertyType"> Type of property inside enumerable or nullable property. </param>
        /// <returns> Tuple: (Is type correct, additional inner type). </returns>
        private (bool, Type) ValidateInnerPropertyType(Type propertyType, Type innerPropertyType)
        {
            //  Check if inside property type is enumerable type.
            if (innerPropertyType.IsEnum)
            {
                return (true, null);
            }

            //  Check if inside property type is array type - throw InvalidDataModelProperty.
            else if (innerPropertyType.IsArray)
            {
                return (false, innerPropertyType);
            }

            //  Check if inside property type is nullable type.
            else if (innerPropertyType.IsGenericType && innerPropertyType.GetInterfaces().Contains(typeof(INullable)))
            {
                return (true, null);
            }

            //  Check if inside property type is dictionary type - throw InvalidDataModelProperty.
            else if (innerPropertyType.IsGenericType && innerPropertyType.GetInterfaces().Contains(typeof(IDictionary)))
            {
                return (false, innerPropertyType);
            }

            //  Check if inside property type is ICollection/list type - throw InvalidDataModelProperty.
            else if (innerPropertyType.IsGenericType && innerPropertyType.GetInterfaces().Contains(typeof(ICollection)))
            {
                return (false, innerPropertyType);
            }

            //  Check if property type is string.
            else if (innerPropertyType == typeof(string))
            {
                return (true, null);
            }

            //  Check if inside property type is another class type - throw InvalidDataModelProperty.
            else if (innerPropertyType.IsClass)
            {
                return (false, innerPropertyType);
            }

            //  Other inside properties
            else
            {
                return (true, null);
            }
        }

        #endregion TYPES VALIDATION METHODS

        #region VERSION MANAGEMENT

        /// <summary> Get version of XML database file. </summary>
        /// <returns> Version of XML database file. </returns>
        public XMLDatabaseVersion GetFileVersion()
        {
            //  Get version of database XML file.
            var fileVersion = VersionHandler.GetFileVersion(_root);

            //  Convert and return dabase XML file version as XMLDatabaseVersion.
            return XMLDatabaseVersion.FromDictionary(fileVersion);
        }

        /// <summary> Get max version of required XML database file. </summary>
        /// <returns> Max required version of XML database file. </returns>
        public XMLDatabaseVersion GetCurrentVersion()
        {
            //  Get current version of database library.
            var currentVersion = XmlDatabaseInfo.CurrentVersion;

            //  According to custom options return converted current version
            //  of database library or current version from options.
            return _options.CurrentVersion != null
                ? _options.CurrentVersion : XMLDatabaseVersion.FromDictionary(currentVersion);
        }

        /// <summary> Get minimal version of required XML database file. </summary>
        /// <returns> Minimal required version of XML database file. </returns>
        public XMLDatabaseVersion GetMinimalVersion()
        {
            //  Get current minimal version of database library.
            var minimalVersion = XmlDatabaseInfo.MinimalVersion;

            //  According to custom options return converted minimal version
            //  of database required file or minimal version from options.
            return _options.MinimalVersion != null
                ? _options.MinimalVersion : XMLDatabaseVersion.FromDictionary(minimalVersion);
        }

        /// <summary> Perform version check on XML database file. </summary>
        /// <returns> Tuple (If version is correct, version error informations). </returns>
        private (bool, DatabaseVersionError) CheckoutVersion()
        {
            var checkoutResult = VersionHandler.CheckoutVersion(_root,
                _options.CurrentVersion?.AsDictionary(),
                _options.MinimalVersion?.AsDictionary());

            switch (checkoutResult)
            {
                case DatabaseVersionError.NO_VERSION:
                case DatabaseVersionError.VERSION_NEWER:
                case DatabaseVersionError.VERSION_OLDER:
                    return (false, checkoutResult);

                case DatabaseVersionError.VERSION_CURRENT:
                default:
                    return (true, checkoutResult);
            }
        }

        #endregion VERSION MANAGEMENT

    }
}
