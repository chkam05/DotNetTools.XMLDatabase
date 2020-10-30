using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tools.XMLDatabase.Statics
{
    static class XmlDatabaseStatics
    {

        public static readonly BindingFlags PropertyTypes = BindingFlags.Instance | BindingFlags.Public;

        public static readonly string XmlDatabaseRoot = "XMLDatabase";
        public static readonly string XmlAttributeIdentifier = "id";
        public static readonly string XmlAttributeKey = "key";
        public static readonly string XmlAttributeType = "type";

        public static readonly string XmlAttributeVersionMajor = "Major";
        public static readonly string XmlAttributeVersionMinor = "Minor";
        public static readonly string XmlAttributeVersionRelease = "Release";
        public static readonly string XmlAttributeVersionRevision = "Revision";

        public static readonly string[] XmlAttributesVersion = new[]
        {
            XmlDatabaseStatics.XmlAttributeVersionMajor,
            XmlDatabaseStatics.XmlAttributeVersionMinor,
            XmlDatabaseStatics.XmlAttributeVersionRelease,
            XmlDatabaseStatics.XmlAttributeVersionRevision
        };

    }
}
