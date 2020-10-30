using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.XMLDatabase.Exceptions
{
    public class InvalidDataModelPropertyConversionException : ArgumentException
    {

        //  VARIABLES

        private static readonly string _message = "Unable to convert and set{data_value}value with{field_type}type on{field_name}field in{data_type}data model instance.";

        public Type DataModelType { get; private set; }
        public Type PropertyType { get; private set; }
        public string PropertyName { get; private set; }
        public string PropertyValue { get; private set; }


        //  METHODS

        #region CLASS METHODS

        /// <summary> InvalidDataModelPropertyConversionException class initializer. </summary>
        /// <param name="dataModelType"> Type of data model. </param>
        /// <param name="propertyType"> Type of property. </param>
        /// <param name="propertyName"> Name of property. </param>
        /// <param name="propertyValue"> Property value. </param>
        public InvalidDataModelPropertyConversionException(Type dataModelType, Type propertyType, string propertyName, string propertyValue) : base(
            BuildMessage(_message, dataModelType, propertyType, propertyName, propertyValue))
        {
            DataModelType = dataModelType;
            PropertyType = propertyType;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        #endregion CLASS METHODS

        #region TOOL METHODS

        /// <summary> Default message builder. </summary>
        /// <param name="message"> Static exception message. </param>
        /// <param name="dataModelType"> Type of data model. </param>
        /// <param name="propertyType"> Type of property. </param>
        /// <param name="propertyName"> Name of property. </param>
        /// <param name="propertyValue"> Property value. </param>
        /// <returns> Message for exception. </returns>
        private static string BuildMessage(string message, Type dataModelType = null, Type propertyType = null,
            string propertyName = null, string propertyValue = null)
        {
            var modelName = dataModelType != null ? $" {dataModelType.Name} " : " ";
            var typeName = propertyType != null ? $" {propertyType.Name} " : " specified type ";
            var fieldName = !string.IsNullOrEmpty(propertyName) ? $" {propertyName} " : " ";
            var value = !string.IsNullOrEmpty(propertyValue) ? $" \"{propertyValue}\" " : " ";

            return message
                .Replace("{data_type}", modelName)
                .Replace("{field_type}", typeName)
                .Replace("{field_name}", fieldName)
                .Replace("{data_value}", value);
        }

        #endregion TOOL METHODS

    }
}
