using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.ConversionTestModels
{
    public class ConversionArraysDataModel : DataModel
    {

        public bool[] BoolArray { get; set; }
        public DateTime[] DateTimeArray { get; set; }
        public double[] DoubleArray { get; set; }
        public SimpleEnum[] EnumArray { get; set; }
        public float[] FloatArray { get; set; }
        public int[] IntArray { get; set; }
        public long[] LongArray { get; set; }
        public string[] StringArray { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public ConversionArraysDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public ConversionArraysDataModel(XElement xmlObject) : base(xmlObject) { }

    }
}
