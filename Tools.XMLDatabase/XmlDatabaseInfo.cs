using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Statics;

namespace Tools.XMLDatabase
{
    static class XmlDatabaseInfo
    {

        public static readonly Dictionary<string, int> CurrentVersion = new Dictionary<string, int>()
        {
            { XmlDatabaseStatics.XmlAttributeVersionMajor, 2 },
            { XmlDatabaseStatics.XmlAttributeVersionMinor, 0 },
            { XmlDatabaseStatics.XmlAttributeVersionRelease, 0 },
            { XmlDatabaseStatics.XmlAttributeVersionRevision, 0 }
        };

        public static readonly Dictionary<string, int> MinimalVersion = new Dictionary<string, int>()
        {
            { XmlDatabaseStatics.XmlAttributeVersionMajor, 1 },
            { XmlDatabaseStatics.XmlAttributeVersionMinor, 0 },
            { XmlDatabaseStatics.XmlAttributeVersionRelease, 0 },
            { XmlDatabaseStatics.XmlAttributeVersionRevision, 0 }
        };

    }
}
