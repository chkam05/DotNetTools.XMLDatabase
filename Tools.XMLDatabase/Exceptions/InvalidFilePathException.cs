using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.XMLDatabase.Exceptions
{
    public class InvalidFilePathException : ArgumentException
    {

        //  VARIABLES

        private static readonly string _message = "Selected path{path}is not valid file or directory path.";

        public string Path { get; private set; }


        //  METHODS

        #region CLASS METHODS

        /// <summary> InvalidFilePathException class initializer. </summary>
        public InvalidFilePathException() : base(BuildMessage(_message))
        {
            Path = null;
        }

        /// <summary> InvalidFilePathException class with path initializer. </summary>
        /// <param name="path"> Path to the file or directory. </param>
        public InvalidFilePathException(string path) : base(BuildMessage(_message, path))
        {
            Path = path;
        }

        #endregion CLASS METHODS

        #region TOOL METHODS

        /// <summary> Default message builder. </summary>
        /// <param name="message"> Static exception message. </param>
        /// <param name="path"> Message parameter - path. </param>
        /// <returns> Message for exception. </returns>
        private static string BuildMessage(string message, string path = null)
        {
            if (path != null)
                return message.Replace("{path}", $" \"{path}\" ");
            else
                return message.Replace("{path}", " ");
        }

        #endregion TOOL METHODS

    }
}
