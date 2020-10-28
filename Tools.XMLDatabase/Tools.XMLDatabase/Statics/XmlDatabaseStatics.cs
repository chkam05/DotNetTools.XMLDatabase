using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tools.XMLDatabase.Statics
{
    class XmlDatabaseStatics
    {

        public static readonly BindingFlags PropertyTypes = BindingFlags.Instance | BindingFlags.Public;

        public static readonly string XmlAttributeIdentifier = "id";
        public static readonly string XmlAttributeKey = "key";
        public static readonly string XmlAttributeType = "type";

    }
}
