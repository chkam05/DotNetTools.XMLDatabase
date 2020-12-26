using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.XMLDatabase.Exceptions
{
    public class InvalidDataModelPropertyException : Exception
    {

        //  VARIABLES

        private static readonly string _defaultMessage = "DataModel contains a property with an invalid {data_type}type.";
        private static readonly string _containerMessage = "DataModel cannot contain a{data_type}property with {conjunction}{inside_data_type}type.";

        public Type PropertyType { get; private set; }
        public Type InnerPropertyType { get; private set; }


        //  METHODS

        #region CLASS METHODS

        /// <summary> InvalidDataModelPropertyException class initializer. </summary>
        public InvalidDataModelPropertyException() : base(BuildMessage(_defaultMessage))
        {
            PropertyType = null;
        }

        /// <summary> InvalidDataModelPropertyException class with invalid data type property. </summary>
        /// <param name="propertyType"> Type of property. </param>
        public InvalidDataModelPropertyException(Type propertyType) : base(BuildMessage(_defaultMessage, propertyType))
        {
            PropertyType = propertyType;
        }

        /// <summary> InvalidDataModelPropertyException class with invalid data type property with inner property. </summary>
        /// <param name="propertyType"> Type of property. </param>
        /// <param name="innerPropertyType"> Type of property inside propertyType. </param>
        public InvalidDataModelPropertyException(Type propertyType, Type innerPropertyType) 
            : base(BuildMessage(_containerMessage, propertyType, innerPropertyType))
        {
            PropertyType = propertyType;
            InnerPropertyType = innerPropertyType;
        }

        #endregion CLASS METHODS

        #region TOOL METHODS

        /// <summary> Default message builder. </summary>
        /// <param name="message"> Static exception message. </param>
        /// <param name="propertyType"> Message parameter - type of property. </param>
        /// <param name="innerPropertyType"> Message parameter - type of property inside property. </param>
        /// <returns> Message for exception. </returns>
        private static string BuildMessage(string message, Type propertyType = null, Type innerPropertyType = null)
        {
            if (propertyType != null && innerPropertyType != null)
            {
                return message
                        .Replace("{data_type}", $"\"{propertyType.Name}\" ")
                        .Replace("{conjunction}", propertyType.BaseType == innerPropertyType.BaseType ? "another " : "")
                        .Replace("{inside_data_type}", $"\"{innerPropertyType.Name}\" ");
            }
            else if (propertyType != null)
                return message
                    .Replace("{data_type}", $"\"{propertyType.Name}\" ")
                    .Replace("{conjunction}", "")
                    .Replace("{inside_data_type}", "");
            else
                return message
                    .Replace("{data_type}", "")
                    .Replace("{conjunction}", "")
                    .Replace("{inside_data_type}", "");
        }

        #endregion TOOL METHODS

    }
}
