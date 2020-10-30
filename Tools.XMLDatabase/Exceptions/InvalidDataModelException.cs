using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.XMLDatabase.Exceptions
{
    public class InvalidDataModelException : ArgumentException
    {

        //  VARIABLES

        private static readonly string _message = "Cannot register{data_model}data model type. Check if selected data model is a class inherited from DataModel.";

        public Type DataModelType { get; private set; }


        //  METHODS

        #region CLASS METHODS

        /// <summary> InvalidDataModelException class initializer. </summary>
        public InvalidDataModelException() : base(BuildMessage(_message))
        {
            DataModelType = null;
        }

        /// <summary> InvalidDataModelException class with data model type initializer. </summary>
        /// <param name="dataModelType"> Type of data model. </param>
        public InvalidDataModelException(Type dataModelType) : base(BuildMessage(_message, dataModelType))
        {
            DataModelType = dataModelType;
        }

        #endregion CLASS METHODS

        #region TOOL METHODS

        /// <summary> Default message builder. </summary>
        /// <param name="message"> Static exception message. </param>
        /// <param name="dataModelType"> Message parameter - type of data model. </param>
        /// <returns> Message for exception. </returns>
        private static string BuildMessage(string message, Type dataModelType = null)
        {
            if (dataModelType != null)
                return message.Replace("{data_model}", $" \"{dataModelType.Name}\"");
            else
                return message.Replace("{data_model}", " \"null\" ");
        }

        #endregion TOOL METHODS

    }
}
