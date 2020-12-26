using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Converters;
using Tools.XMLDatabase.Exceptions;
using Tools.XMLDatabase.Statics;

using CustomConvertToObjectFunc = Tools.XMLDatabase.Converters.CustomXmlConverter.CustomConvertToObjectFunc;

namespace Tools.XMLDatabase.Data
{
    public class DataModel
    {

        //  VARIABLES

        public string Id { get; set; }


        //  METHODS

        #region CLASS METHODS

        /// <summary> Base DataModel contructor. </summary>
        protected DataModel()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        /// <summary> DataModel constructor from XML database object. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        /// <param name="options"> Opcje bazy danych. </param>
        protected DataModel(XElement xmlObject, XmlDatabaseOptions options = null)
        {
            ConvertFromXml(xmlObject, options);
        }

        #endregion CLASS METHODS

        #region CONVERSION METHODS

        /// <summary> Create XML object instance with data from this object. </summary>
        /// <param name="options"> Opcje bazy danych. </param>
        /// <returns> XML data element as XElement. </returns>
        public XElement AsXml(XmlDatabaseOptions options = null)
        {
            //  Create empty XElement object instance with class name.
            var xmlObject = new XElement(this.GetType().Name);

            //  Set identifier attribute.
            var identifierAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeIdentifier, Id);
            xmlObject.Add(identifierAttribute);

            //  Get properties
            var propertyInfos = this.GetType().GetProperties(XmlDatabaseStatics.PropertyTypes);

            foreach (var propertyInfo in propertyInfos)
            {
                //  Ignore virtual properties.
                if (propertyInfo.GetGetMethod().IsVirtual)
                    continue;

                //  Get type of single data model property.
                var propertyType = propertyInfo.PropertyType;

                //  Get type name.
                var propertyName = options != null
                    ? CustomXmlConverter.TypeToString(propertyType, options.TypesCoding)
                    : CustomXmlConverter.TypeToString(propertyType, TypesCoding.SIMPLE);

                //  Perform an action on enum type.
                if (propertyType.IsEnum)
                {
                    var xmlVariable = CustomXmlConverter.EnumToXml(
                        propertyInfo.Name, propertyType, propertyInfo.GetValue(this));

                    //  Add type.
                    var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, propertyName);
                    xmlVariable.Add(typeAttribute);

                    xmlObject.Add(xmlVariable);
                }

                //  Perform an action on array type.
                else if (propertyType.IsArray)
                {
                    var xmlVariable = CustomXmlConverter.ArrayToXml(
                        propertyInfo.Name, propertyType, propertyInfo.GetValue(this));

                    //  Add type.
                    var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, propertyName);
                    xmlVariable.Add(typeAttribute);

                    xmlObject.Add(xmlVariable);
                }

                //  Perform an action on nullable type.
                else if (propertyType.IsGenericType && Nullable.GetUnderlyingType(propertyType) != null)
                {
                    var xmlVariable = CustomXmlConverter.NullableToXml(
                        propertyInfo.Name, propertyType, propertyInfo.GetValue(this));

                    //  Add type.
                    var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, propertyName);
                    xmlVariable.Add(typeAttribute);

                    xmlObject.Add(xmlVariable);
                }

                //  Perform an action on dictionary type.
                else if (propertyType.IsGenericType && propertyType.GetInterfaces().Contains(typeof(IDictionary)))
                {
                    var xmlVariable = CustomXmlConverter.DictionaryToXml(
                        propertyInfo.Name, propertyType, propertyInfo.GetValue(this));

                    //  Add type.
                    var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, propertyName);
                    xmlVariable.Add(typeAttribute);

                    xmlObject.Add(xmlVariable);
                }

                //  Perform an action on ICollection/list type.
                else if (propertyType.IsGenericType && propertyType.GetInterfaces().Contains(typeof(ICollection)))
                {
                    var xmlVariable = CustomXmlConverter.ListToXml(
                        propertyInfo.Name, propertyType, propertyInfo.GetValue(this));

                    //  Add type.
                    var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, propertyName);
                    xmlVariable.Add(typeAttribute);

                    xmlObject.Add(xmlVariable);
                }

                //  Perform an action on typical value.
                else
                {
                    //  Convert value and setup XML data object.
                    var xmlVariable = new XElement(propertyInfo.Name, propertyInfo.GetValue(this));
                    var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, propertyName);

                    //  Add XML value into current XML object representation.
                    xmlVariable.Add(typeAttribute);
                    xmlObject.Add(xmlVariable);
                }
            }

            //  Return XElement object instance.
            return xmlObject;
        }

        /// <summary> Load data to this object from XML XElement object instance. </summary>
        /// <param name="options"> Opcje bazy danych. </param>
        /// <param name="xmlObject"> XML data element instance as XElement </param>
        private void ConvertFromXml(XElement xmlObject, XmlDatabaseOptions options = null)
        {
            //  Get identifier of xml data object instance.
            Id = xmlObject.Attribute(XmlDatabaseStatics.XmlAttributeIdentifier).Value;

            //  Setup rest of cuurent class instance fields/properties/variables.
            var xmlElements = xmlObject.Elements();

            foreach (var element in xmlElements)
                TrySetVariable(element, options);
        }

        #endregion CONVERSION METHODS

        #region DATA MANAGEMENT METHODS

        /// <summary> Try set current class field/property/variable with data from XML object. </summary>
        /// <param name="options"> Opcje bazy danych. </param>
        /// <param name="xmlObject"> XML object with single data to set. </param>
        private void TrySetVariable(XElement xmlObject, XmlDatabaseOptions options = null)
        {
            //  Get property information from XML object.
            var propertyName = xmlObject.Name.ToString();
            var propertyTypeName = xmlObject.Attribute(XmlDatabaseStatics.XmlAttributeType).Value;
            var currentType = GetType();

            //  Get current class property by name.
            var propertyInfo = currentType.GetProperty(propertyName);

            if (propertyInfo != null)
            {
                //  Ignore virtual properties.
                if (propertyInfo.GetGetMethod().IsVirtual)
                    return;

                //  Get type of selected class property.
                var propertyType = propertyInfo.PropertyType;

                //  Validate current property type and property type from XML object.
                var currentTypeName = options != null
                    ? CustomXmlConverter.TypeToString(propertyType, options.TypesCoding)
                    : CustomXmlConverter.TypeToString(propertyType, TypesCoding.SIMPLE);

                if (currentTypeName != propertyTypeName)
                    return;

                //  Perform an action on enum type.
                if (propertyType.IsEnum)
                    propertyInfo.SetValue(this, CustomXmlConverter.XmlToEnum(propertyType, xmlObject));

                //  Perform an action on array type.
                else if (propertyType.IsArray)
                    propertyInfo.SetValue(this, CustomXmlConverter.XmlToArray(propertyType, xmlObject));

                //  Perform an action on nullable type.
                else if (propertyType.IsGenericType && Nullable.GetUnderlyingType(propertyType) != null)
                    propertyInfo.SetValue(this, CustomXmlConverter.XmlToNullable(propertyType, xmlObject));

                //  Perform an action on dictionary type.
                else if (propertyType.IsGenericType && propertyType.GetInterfaces().Contains(typeof(IDictionary)))
                    propertyInfo.SetValue(this, CustomXmlConverter.XmlToDictionary(propertyType, xmlObject));

                //  Perform an action on ICollection/list type.
                else if (propertyType.IsGenericType && propertyType.GetInterfaces().Contains(typeof(ICollection)))
                    TryInvokeConvertAndSetVariable(CustomXmlConverter.XmlToList, propertyInfo, propertyType, xmlObject);

                //  Perform an action on typical value.
                else
                    TryConvertAndSetBaseVariable(propertyInfo, propertyType, xmlObject.Value);
            }
        }

        /// <summary> Try to convert and set base property value from XML object into current data model class instance. </summary>
        /// <param name="property"> Property/field/variable. </param>
        /// <param name="type"> Type of selected property/field/variable. </param>
        /// <param name="value"> Value to convert and set into property/field/variable. </param>
        private void TryConvertAndSetBaseVariable(PropertyInfo property, Type type, string value)
        {
            //  If value is null or empty, leave it.
            if (string.IsNullOrEmpty(value))
                return;

            try
            {
                //  Try to convert and set value.
                var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                property.SetValue(this, convertedValue);
            }
            catch (Exception)
            {
                throw new InvalidDataModelPropertyConversionException(
                    GetType(), type, property.Name, value);
            }
        }

        /// <summary> Try to convert and set property value from XML object into current data model class instance with error aviodance. </summary>
        /// <param name="func"> Convert function </param>
        /// <param name="property"> Property/field/variable. </param>
        /// <param name="type"> Type of selected property/field/variable. </param>
        /// <param name="xmlObject"> XML object with data to set. </param>
        private void TryInvokeConvertAndSetVariable(CustomConvertToObjectFunc func, PropertyInfo property, Type type, XElement xmlObject)
        {
            if (func != null)
                try
                {
                    //  Try to convert and set value.
                    property.SetValue(this, func.Invoke(type, xmlObject));
                }
                catch (Exception)
                {
                    throw new InvalidDataModelPropertyConversionException(
                        GetType(), type, property.Name, xmlObject.Value);
                }
        }

        #endregion DATA MANAGEMENT METHODS

    }
}
