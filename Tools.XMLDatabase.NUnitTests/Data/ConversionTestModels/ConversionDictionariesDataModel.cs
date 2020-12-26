using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.ConversionTestModels
{
    public class ConversionDictionariesDataModel : DataModel
    {

        public Dictionary<string, bool> BoolDict { get; set; }
        public Dictionary<string, DateTime> DateTimeDict { get; set; }
        public Dictionary<string, double> DoubleDict { get; set; }
        public Dictionary<string, SimpleEnum> EnumDict { get; set; }
        public Dictionary<string, float> FloatDict { get; set; }
        public Dictionary<string, int> IntDict { get; set; }
        public Dictionary<string, long> LongDict { get; set; }
        public Dictionary<int, string> StringDict { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public ConversionDictionariesDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public ConversionDictionariesDataModel(XElement xmlObject, XmlDatabaseOptions options = null) : base(xmlObject, options) { }

    }
}
