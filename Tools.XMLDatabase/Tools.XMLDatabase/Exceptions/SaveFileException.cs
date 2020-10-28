using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.XMLDatabase.Exceptions
{
    public class SaveFileException : ArgumentException
    {

        //  VARIABLES

        private static readonly string _message = "Unable to save file{file_path}. Access denied or data has been corrupted.";

        public string FilePath { get; private set; }


        //  METHODS

        #region CLASS METHODS

        /// <summary> SaveFileException class initializer. </summary>
        public SaveFileException() : base(BuildMessage(_message))
        {
            FilePath = null;
        }

        /// <summary> SaveFileException class with file path initializer. </summary>
        /// <param name="filePath"> Path to the file. </param>
        public SaveFileException(string filePath) : base(BuildMessage(_message, filePath))
        {
            FilePath = filePath;
        }

        #endregion CLASS METHODS

        #region TOOL METHODS

        /// <summary> Default message builder. </summary>
        /// <param name="message"> Static exception message. </param>
        /// <param name="filePath"> Message parameter - path to the file. </param>
        /// <returns> Message for exception. </returns>
        private static string BuildMessage(string message, string filePath = null)
        {
            if (filePath != null)
                return message.Replace("{file_path}", $" \"{filePath}\"");
            else
                return message.Replace("{file_path}", "");
        }

        #endregion TOOL METHODS

    }
}
