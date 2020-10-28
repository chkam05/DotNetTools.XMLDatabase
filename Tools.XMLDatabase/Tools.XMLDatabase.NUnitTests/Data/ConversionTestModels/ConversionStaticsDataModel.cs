using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.ConversionTestModels
{
    public class ConversionStaticsDataModel : DataModel
    {

        public bool BoolValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public double DoubleValue { get; set; }
        public SimpleEnum EnumValue { get; set; }
        public float FloatValue { get; set; }
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public string StringValue { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public ConversionStaticsDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public ConversionStaticsDataModel(XElement xmlObject) : base(xmlObject) { }

    }
}
