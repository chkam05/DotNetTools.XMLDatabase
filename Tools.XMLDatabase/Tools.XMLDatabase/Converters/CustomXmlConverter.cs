using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Statics;

namespace Tools.XMLDatabase.Converters
{
    public static class CustomXmlConverter
    {

        //  VARIABLES

        public delegate object CustomConvertToObjectFunc(Type type, XElement xmlObject);


        //  METHODS

        #region ARRAY XML CONVERTERS

        public static XElement ArrayToXml(string name, Type type, object value)
        {
            //  Setup XML data object.
            var xmlObject = new XElement(name);
            var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, type.Name);
            xmlObject.Add(typeAttribute);

            //  Seed XML array data.
            if (value != null)
                foreach (var xmlElement in ParseArrayObject(type, value))
                    xmlObject.Add(xmlElement);

            //  Return XML object.
            return xmlObject;
        }

        public static object XmlToArray(Type type, XElement xmlObject)
        {
            //  Prepare conversion.
            var convertedValue = ParseArrayXmlObject(type, xmlObject);

            //  Return array object.
            return convertedValue;
        }

        private static List<XElement> ParseArrayObject(Type arrayType, object value)
        {
            //  Get Array inner type.
            Type innerType = arrayType.GetElementType();

            //  Convert object into readable array.
            Array array = (Array)Convert.ChangeType(value, arrayType);

            //  Create output list of XML objects.
            var result = new List<XElement>();

            //  Convert each list object element into XML object.
            foreach (var index in Enumerable.Range(0, array.Length))
                result.Add(CollectibleInnerToXml(index, innerType, array.GetValue(index)));

            //  Return list of XML objects.
            return result;
        }

        private static object ParseArrayXmlObject(Type arrayType, XElement xmlObject)
        {
            //  Recognize array type.
            var innerType = arrayType.GetElementType();
            var elementsCount = xmlObject.HasElements ? xmlObject.Elements().Count() : 0;

            //  Create array and iterator.
            Array array = Array.CreateInstance(innerType, elementsCount);
            int iterator = 0;

            //  Convert each XML array elements into array object value.
            if (xmlObject.HasElements)
                foreach (var xmlElement in xmlObject.Elements())
                {
                    array.SetValue(ParseCollectibleInnertXmlObject(innerType, xmlElement.Value), iterator);
                    iterator++;
                }

            //  Return converted array object.
            return array;
        }

        #endregion ARRAY XML CONVERTERS

        #region DICTIONARY XML CONVERTERS

        public static XElement DictionaryToXml(string name, Type type, object value)
        {
            //  Setup XML data object.
            var xmlObject = new XElement(name);
            var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, type.Name);
            xmlObject.Add(typeAttribute);

            //  Seed XML dictionary data.
            if (value != null)
                foreach (var xmlElement in ParseDictionaryObject(type, value))
                    xmlObject.Add(xmlElement);

            //  Return XML object.
            return xmlObject;
        }

        public static object XmlToDictionary(Type type, XElement xmlObject)
        {
            //  Prepare conversion.
            var convertedValue = ParseDictionaryXmlObject(type, xmlObject);

            //  Return dictionary object.
            return convertedValue;
        }

        private static List<XElement> ParseDictionaryObject(Type dictionaryType, object value)
        {
            //  Get List inner type.
            Type innerKeyType = dictionaryType.GetGenericArguments()[0];
            Type innerValueType = dictionaryType.GetGenericArguments()[1];

            //  Convert object into readable dictionary.
            IDictionary dictionary = (IDictionary)Convert.ChangeType(value, dictionaryType);

            //  Create output list of XML objects and iterator.
            var result = new List<XElement>();
            var iterator = 0;

            //  Convert each list object element into XML object.
            foreach (var dictKey in dictionary.Keys)
            {
                var keyAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeKey, ParseCollectibleInnerObject(innerKeyType, dictKey));
                var xmlElement = CollectibleInnerToXml(iterator, innerValueType, dictionary[dictKey]);

                xmlElement.Add(keyAttribute);
                result.Add(xmlElement);
            }

            //  Return list of XML objects.
            return result;
        }

        private static object ParseDictionaryXmlObject(Type dictionaryType, XElement xmlObject)
        {
            //  Recognize type.
            var baseType = typeof(Dictionary<,>);
            var keyType = dictionaryType.GetGenericArguments()[0];
            var valueType = dictionaryType.GetGenericArguments()[1];
            var genericType = baseType.MakeGenericType(keyType, valueType);

            //  Create dictionary.
            IDictionary dictionary = (IDictionary)Activator.CreateInstance(genericType);

            //  Convert each XML dictionary elements into dictionary object value.
            if (xmlObject.HasElements)
                foreach (var xmlElement in xmlObject.Elements())
                {
                    var convertedKey = ParseCollectibleInnertXmlObject(keyType, xmlElement.Attribute(XmlDatabaseStatics.XmlAttributeKey).Value);
                    var convertedValue = ParseCollectibleInnertXmlObject(valueType, xmlElement.Value);

                    //  Add dictionary entry.
                    dictionary.Add(convertedKey, convertedValue);
                }

            //  Return converted dictionary object.
            return dictionary;
        }

        #endregion DICTIONARY XML CONVERTERS

        #region ENUM XML CONVERTERS

        public static XElement EnumToXml(string name, Type type, object value)
        {
            //  Prepare conversion.
            var convertedValue = ParseEnumObject(type, value);

            //  Setup XML data object.
            var xmlObject = new XElement(name, convertedValue);
            var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, type.Name);
            xmlObject.Add(typeAttribute);

            //  Return XML object.
            return xmlObject;
        }

        public static object XmlToEnum(Type type, XElement xmlObject)
        {
            //  Prepare conversion.
            var convertedValue = ParseEnumXmlObject(type, xmlObject.Value);

            //  Return Enum object.
            return convertedValue;
        }

        private static object ParseEnumObject(Type enumType, object value)
        {
            //  Convert Enum value to integeer and return.
            var enumValue = Enum.Parse(enumType, value.ToString()) as Enum;
            return Convert.ToInt32(enumValue);
        }

        private static object ParseEnumXmlObject(Type enumType, string xmlStringValue)
        {
            //  Convert XML value to Enum object.
            return Enum.Parse(enumType, xmlStringValue);
        }

        #endregion ENUM XML CONVERTERS

        #region GLOBAL COLLECTIBLE CONVERTERS

        private static XElement CollectibleInnerToXml(int iteration, Type innerType, object value)
        {
            //  Convert list inner object.
            object convertedValue = ParseCollectibleInnerObject(innerType, value);

            //  Create and return XML element object.
            return new XElement($"index_{iteration}", convertedValue);
        }

        private static object ParseCollectibleInnerObject(Type innerType, object value)
        {
            //  Convert collectible inner value and return.
            if (innerType.IsEnum)
                return ParseEnumObject(innerType, value);
            else if (innerType.IsGenericType && Nullable.GetUnderlyingType(innerType) != null)
                return ParseNullableObject(innerType, value);
            else
                return value;
        }

        private static object ParseCollectibleInnertXmlObject(Type innerType, string value)
        {
            //  Create result object.
            object convertedValue;

            //  Convert XML value into list inner object.
            if (innerType.IsGenericType && Nullable.GetUnderlyingType(innerType) != null)
                convertedValue = ParseNullableXmlObject(innerType, value);
            else if (innerType.IsEnum)
                convertedValue = ParseEnumXmlObject(innerType, value);
            else
                convertedValue = Convert.ChangeType(value, innerType, CultureInfo.InvariantCulture);

            //  Return converter result object.
            return convertedValue;
        }

        #endregion GLOBAL COLLECTIBLE CONVERTERS

        #region LIST XML CONVERTERS

        public static XElement ListToXml(string name, Type type, object value)
        {
            //  Setup XML data object.
            var xmlObject = new XElement(name);
            var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, type.Name);
            xmlObject.Add(typeAttribute);

            //  Seed XML list data.
            if (value != null)
                foreach (var xmlElement in ParseListObject(type, value))
                    xmlObject.Add(xmlElement);

            //  Return XML object.
            return xmlObject;
        }

        public static object XmlToList(Type type, XElement xmlObject)
        {
            //  Prepare conversion.
            var convertedValue = ParseListXmlObject(type, xmlObject);

            //  Return list object.
            return convertedValue;
        }

        private static List<XElement> ParseListObject(Type listType, object value)
        {
            //  Get List inner type.
            Type innerType = listType.GetGenericArguments()[0];

            //  Convert object into readable list.
            IList list = (IList)Convert.ChangeType(value, listType);

            //  Create output list of XML objects.
            var result = new List<XElement>();

            //  Convert each list object element into XML object.
            foreach (var index in Enumerable.Range(0, list.Count))
                result.Add(CollectibleInnerToXml(index, innerType, list[index]));

            //  Return list of XML objects.
            return result;
        }

        private static object ParseListXmlObject(Type listType, XElement xmlObject)
        {
            //  Recognize list type.
            var baseType = typeof(List<>);
            var innerType = listType.GetGenericArguments()[0];
            var genericType = baseType.MakeGenericType(innerType);

            //  Create list.
            IList list = (IList)Activator.CreateInstance(genericType);

            //  Convert each XML list elements into list object value.
            if (xmlObject.HasElements)
                foreach (var xmlElement in xmlObject.Elements())
                    list.Add(ParseCollectibleInnertXmlObject(innerType, xmlElement.Value));

            //  Return converted list object.
            return list;
        }

        #endregion LIST XML CONVERTERS

        #region NULLABLE XML CONVERTERS

        public static XElement NullableToXml(string name, Type type, object value = null)
        {
            //  Prepare conversion.
            var convertedValue = ParseNullableObject(type, value);

            //  Setup XML data object.
            var xmlObject = new XElement(name, convertedValue);
            var typeAttribute = new XAttribute(XmlDatabaseStatics.XmlAttributeType, type.Name);
            xmlObject.Add(typeAttribute);

            //  Return XML object.
            return xmlObject;
        }

        public static object XmlToNullable(Type type, XElement xmlObject)
        {
            //  Prepare conversion.
            var convertedValue = ParseNullableXmlObject(type, xmlObject.Value);

            //  Return Nullable object.
            return convertedValue;
        }

        private static object ParseNullableObject(Type nullableType, object value = null)
        {
            //  Get Nullable inner type.
            Type innerType = nullableType.GetGenericArguments()[0];

            //  Convert Nullable value and return.
            if (innerType.IsEnum && value != null)
                return ParseEnumObject(innerType, value);
            else
                return value;
        }

        private static object ParseNullableXmlObject(Type nullableType, string xmlStringValue)
        {
            //  Get Nullable inner type.
            Type innerType = nullableType.GetGenericArguments()[0];

            //  Create result object.
            object convertedValue;

            //  Convert XML value to Nullable object.
            if (!string.IsNullOrEmpty(xmlStringValue))
            {
                //  Convert XML value into Nullable inner object.
                if (innerType.IsEnum)
                    convertedValue = Enum.Parse(innerType, xmlStringValue);
                else
                    convertedValue = Convert.ChangeType(xmlStringValue, innerType, CultureInfo.InvariantCulture);

                //  Convert Nullable inner object into Nullable object and return.
                return TypeDescriptor.GetConverter(nullableType).ConvertFrom(convertedValue);
            }
            else
            {
                return null;
            }
        }

        #endregion NULLABLE XML CONVERTERS

    }
}
