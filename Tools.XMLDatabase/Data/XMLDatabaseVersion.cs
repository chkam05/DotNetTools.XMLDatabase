using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tools.XMLDatabase.Exceptions;
using Tools.XMLDatabase.Statics;

namespace Tools.XMLDatabase.Data
{
    public class XMLDatabaseVersion
    {

        //  VARIABLES

        public int Major { get; set; } = 2;
        public int Minor { get; set; } = 0;
        public int Release { get; set; } = 0;
        public int Revision { get; set; } = 0;


        //  METHODS

        #region CLASS METHODS
        
        /// <summary> XMLDatabaseVersion constructor. </summary>
        public XMLDatabaseVersion() { }

        #endregion CLASS METHODS

        #region CONVERSION METHODS

        /// <summary> Create XMLDatabaseVersion from dictionary equivalent structure. </summary>
        /// <param name="dictionaryVersion"> XML database version as dictionary. </param>
        /// <returns> XMLDatabaseVersion instance. </returns>
        public static XMLDatabaseVersion FromDictionary(Dictionary<string, int> dictionaryVersion)
        {
            //  Create result version object.
            var result = new XMLDatabaseVersion();

            foreach (var versionPart in XmlDatabaseStatics.XmlAttributesVersion)
            {
                //  Check if input dictionary has particular part version.
                if (!dictionaryVersion.ContainsKey(versionPart))
                    throw new IncorrectXmlVersionPartException(versionPart);

                //  Set version part.
                var property = typeof(XMLDatabaseVersion).GetProperty(versionPart);
                if (property != null)
                    typeof(XMLDatabaseVersion).GetProperty(versionPart).SetValue(result, dictionaryVersion[versionPart]);
            }

            return result;
        }

        /// <summary> Get XMLDatabaseVersion as dictionary equivalent structure. </summary>
        /// <returns> Dictionary equivalent structure for XMLDatabaseVersion. </returns>
        public Dictionary<string, int> AsDictionary()
        {
            return new Dictionary<string, int>()
            {
                { XmlDatabaseStatics.XmlAttributeVersionMajor, Major },
                { XmlDatabaseStatics.XmlAttributeVersionMinor, Minor },
                { XmlDatabaseStatics.XmlAttributeVersionRelease, Release },
                { XmlDatabaseStatics.XmlAttributeVersionRevision, Revision }
            };
        }

        #endregion CONVERSION METHODS

    }
}
