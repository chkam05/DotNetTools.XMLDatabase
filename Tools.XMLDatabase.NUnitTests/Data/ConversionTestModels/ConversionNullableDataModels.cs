using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.ConversionTestModels
{
    public class ConversionNullableDataModels : DataModel
    {

        public bool? BoolNullableValue { get; set; }
        public DateTime? DateTimeNullableValue { get; set; }
        public double? DoubleNullableValue { get; set; }
        public SimpleEnum? EnumNullableValue { get; set; }
        public float? FloatNullableValue { get; set; }
        public int? IntNullableValue { get; set; }
        public long? LongNullableValue { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public ConversionNullableDataModels() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public ConversionNullableDataModels(XElement xmlObject, XmlDatabaseOptions options = null) : base(xmlObject, options) { }

    }
}
