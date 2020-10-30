using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.Statics;

namespace Tools.XMLDatabase.Tools
{
    static class VersionHandler
    {

        //  VARIABLES

        //  METHODS

        #region CHECKOUT VERSION METHODS

        /// <summary> Check if version from XML database file is required version to work with this version of XML database library. </summary>
        /// <param name="root"> Root of XML database file. </param>
        /// <param name="maxVersion"> Custom current application version. </param>
        /// <param name="minVersion"> Custom minimal required version. </param>
        /// <returns> Information about file version mismatch between application and file. </returns>
        public static DatabaseVersionError CheckoutVersion(
            XElement root, Dictionary<string, int> maxVersion = null, Dictionary<string, int> minVersion = null)
        {
            //  Get type of version.
            Type type = typeof(XMLDatabaseVersion);

            //  Get file version instance.
            var fileVersion = GetFileVersion(root);
            var currentVersion = maxVersion != null ? maxVersion : XmlDatabaseInfo.CurrentVersion;
            var minimalVersion = minVersion != null ? minVersion : XmlDatabaseInfo.MinimalVersion;

            if (fileVersion != null)
            {
                foreach (var keyValuePair in fileVersion)
                {
                    var current = currentVersion[keyValuePair.Key];
                    var minimal = minimalVersion[keyValuePair.Key];

                    if (current < keyValuePair.Value) return DatabaseVersionError.VERSION_NEWER;
                    else if (minimal > keyValuePair.Value) return DatabaseVersionError.VERSION_OLDER;
                    else if (current == keyValuePair.Value) return CheckUp(fileVersion, currentVersion);
                    else if (minimal == keyValuePair.Value) return CheckDown(fileVersion, minimalVersion);
                    else break;
                }

                return DatabaseVersionError.VERSION_CURRENT;
            }

            return DatabaseVersionError.NO_VERSION;
        }

        /// <summary> Check if version from XML database file is not higher for version of XML database library. </summary>
        /// <param name="fileVersion"> XML database file version. </param>
        /// <param name="currentVersion"> Current version of XML database library. </param>
        /// <returns> Information about file version mismatch between application and file. </returns>
        private static DatabaseVersionError CheckUp(Dictionary<string, int> fileVersion, Dictionary<string, int> currentVersion)
        {
            foreach (var keyValuePair in fileVersion)
            {
                var current = currentVersion[keyValuePair.Key];
                if (current < keyValuePair.Value) return DatabaseVersionError.VERSION_NEWER;
                else if (current > keyValuePair.Value) break;
            }

            return DatabaseVersionError.VERSION_CURRENT;
        }

        /// <summary> Check if version from XML database file is not lower than version of XML database library. </summary>
        /// <param name="fileVersion"> XML database file version. </param>
        /// <param name="minimalVersion"> Minimal required version of XML database file. </param>
        /// <returns> Information about file version mismatch between application and file. </returns>
        private static DatabaseVersionError CheckDown(Dictionary<string, int> fileVersion, Dictionary<string, int> minimalVersion)
        {
            foreach (var keyValuePair in fileVersion)
            {
                var minimal = minimalVersion[keyValuePair.Key];
                if (minimal > keyValuePair.Value) return DatabaseVersionError.VERSION_OLDER;
                else if (minimal < keyValuePair.Value) break;
            }

            return DatabaseVersionError.VERSION_CURRENT;
        }

        #endregion CHECKOUT VERSION METHODS

        #region GET VERSION METHODS

        /// <summary> Get current version of XML database library. </summary>
        /// <returns> Version of XML database library. </returns>
        public static Dictionary<string, int> GetCurrentVersion()
        {
            //  Get version from Assembly informations.
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var verionParts = from v in version.Split(new[] { "." }, StringSplitOptions.None) select int.Parse(v);

            //  Build and return dictionary.
            return XmlDatabaseStatics.XmlAttributesVersion.Zip(verionParts, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
        }

        /// <summary> Get version from XML database file. </summary>
        /// <param name="root"> Root of XML database file. </param>
        /// <returns> Version of XML database file. </returns>
        public static Dictionary<string, int> GetFileVersion(XElement root)
        {
            //  Get type of version.
            Type type = typeof(XMLDatabaseVersion);

            //  Check if database contains version instance.
            if (root.Elements().Any(item => item.Name == type.Name))
            {
                //  Get item version instance.
                var version = root.Element(type.Name);

                //  Create and return version of file.
                return (from attrib in version.Attributes() select attrib)
                    .ToDictionary(x => x.Name.ToString(), x => int.Parse(x.Value));
            }
            else
            {
                return null;
            }
        }

        #endregion GET VERSION METHODS

        #region SET VERSION METHODS

        /// <summary> Set new XML database file version. </summary>
        /// <param name="root"> Root of XML database file. </param>
        /// <param name="version"> Custom version setted by user. </param>
        /// <returns> Root of XML database file with assigned version. </param>
        public static XElement SetVersion(XElement root, Dictionary<string, int> version = null)
        {
            //  Get type of version.
            Type type = typeof(XMLDatabaseVersion);

            //  Remove old version instance if exists.
            root = RemoveVersion(root);

            //  Create new version instance.
            var versionItem = new XElement(type.Name);

            //  Fill version instance with version information.
            var currentVersion = version != null ? version : XmlDatabaseInfo.CurrentVersion;

            foreach (var keyValuePair in currentVersion)
                versionItem.Add(new XAttribute(keyValuePair.Key, keyValuePair.Value));

            //  Assign a version instance to the xml root.
            root.Add(versionItem);

            return root;
        }

        /// <summary> Remove old assigned version information from XML database file. </summary>
        /// <param name="root"> Root of XML database file. </param>
        /// <returns> Root of XML database file without assigned version. </param>
        private static XElement RemoveVersion(XElement root)
        {
            //  Get type of version.
            Type type = typeof(XMLDatabaseVersion);

            //  Check if database contains version instance and remove it.
            if (root.Elements().Any(item => item.Name == type.Name))
                root.Elements(type.Name).Remove();

            return root;
        }

        #endregion SET VERSION METHODS

    }
}
