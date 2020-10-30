using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.XMLDatabase.Exceptions
{
    public class IncorrectXmlVersionPartException : ArgumentException
    {

        //  VARIABLES

        private static readonly string _message = "Failed to convert XMLDatabase version from dictionary, missing or incorrect {part_name} {part_count}.";

        public string PartName { get; private set; }


        //  METHODS

        #region CLASS METHODS

        /// <summary> InvalidDataModelException class with version part initializer. </summary>
        /// <param name="partName"> Name of version part such as "major", "minor", "release", "revision". </param>
        public IncorrectXmlVersionPartException(string partName) : base(BuildMessage(_message, partName))
        {
            PartName = partName;
        }

        #endregion CLASS METHODS

        #region TOOL METHODS

        /// <summary> Default message builder. </summary>
        /// <param name="message"> Static exception message. </param>
        /// <param name="partName"> Message parameter - Name of version part such as "major", "minor", "release", "revision". </param>
        /// <returns> Message for exception. </returns>
        private static string BuildMessage(string message, string partName)
        {
            if (!string.IsNullOrEmpty(partName))
                return message
                    .Replace("{part_name}", $"{partName}")
                    .Replace("{part_count}", "part");
            else
                return message
                    .Replace("{part_name}", $"one of")
                    .Replace("{part_count}", "parts");
        }

        #endregion TOOL METHODS

    }
}
