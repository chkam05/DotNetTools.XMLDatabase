using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.ConversionTestModels
{
    public class ConversionIterableNullableDataModel : DataModel
    {

        public bool?[] BoolNullableArray { get; set; }
        public Dictionary<int, bool?> BoolNullableDict { get; set; }
        public List<bool?> BoolNullableList { get; set; }

        public DateTime?[] DateTimeNullableArray { get; set; }
        public Dictionary<string, DateTime?> DateTimeNullableDict { get; set; }
        public List<DateTime?> DateTimeNullableList { get; set; }

        public SimpleEnum?[] EnumNullableArray { get; set; }
        public Dictionary<double, SimpleEnum?> EnumNullableDict { get; set; }
        public List<SimpleEnum?> EnumNullableList { get; set; }

        public int?[] IntNullableArray { get; set; }
        public Dictionary<SimpleEnum, int?> IntNullableDict { get; set; }
        public List<int?> IntNullableList { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public ConversionIterableNullableDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public ConversionIterableNullableDataModel(XElement xmlObject, XmlDatabaseOptions options = null) : base(xmlObject, options) { }

    }
}
