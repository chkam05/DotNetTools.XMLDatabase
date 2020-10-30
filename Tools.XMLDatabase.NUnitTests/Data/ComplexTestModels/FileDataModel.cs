using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.ComplexTestModels
{
    public class FileDataModel : DataModel
    {

        public string Name { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public FileDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public FileDataModel(XElement xmlObject) : base(xmlObject) { }

    }
}
