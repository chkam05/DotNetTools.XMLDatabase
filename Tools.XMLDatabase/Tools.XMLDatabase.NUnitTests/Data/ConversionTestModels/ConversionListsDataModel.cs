using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.ConversionTestModels
{
    public class ConversionListsDataModel : DataModel
    {

        public List<bool> BoolList { get; set; }
        public List<DateTime> DateTimeList { get; set; }
        public List<double> DoubleList { get; set; }
        public List<SimpleEnum> EnumList { get; set; }
        public List<float> FloatList { get; set; }
        public List<int> IntList { get; set; }
        public List<long> LongList { get; set; }
        public List<string> StringList { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public ConversionListsDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public ConversionListsDataModel(XElement xmlObject) : base(xmlObject) { }

    }
}
