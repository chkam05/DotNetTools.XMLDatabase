using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Statics;

namespace Tools.XMLDatabase.Exceptions
{
    public class IncorrectXmlVersionException : ArgumentException
    {

        //  VARIABLES

        private static readonly string _message = "XML database file version {state} for the file to load properly.";

        public DatabaseVersionError VersionError { get; private set; } = DatabaseVersionError.VERSION_CURRENT;


        //  METHODS

        #region CLASS METHODS

        /// <summary> IncorrectXmlVersionException class with version error type initializer. </summary>
        /// <param name="versionError"> Type of data model. </param>
        public IncorrectXmlVersionException(DatabaseVersionError versionError) : base(BuildMessage(_message, versionError))
        {
            VersionError = versionError;
        }

        #endregion CLASS METHODS

        #region TOOL METHODS

        /// <summary> Default message builder. </summary>
        /// <param name="message"> Static exception message. </param>
        /// <param name="versionError"> Message parameter - version error type. </param>
        /// <returns> Message for exception. </returns>
        private static string BuildMessage(string message, DatabaseVersionError versionError = DatabaseVersionError.NO_VERSION)
        {
            switch (versionError)
            {
                case DatabaseVersionError.NO_VERSION:
                    return message.Replace("{state}", "can not be checked");

                case DatabaseVersionError.VERSION_NEWER:
                    return message.Replace("{state}", "is too high");

                case DatabaseVersionError.VERSION_OLDER:
                    return message.Replace("{state}", "is too old");

                default:
                    return message.Replace("{state}", "can not be checked");
            }
        }

        #endregion TOOL METHODS

    }
}
