using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.ComplexTestModels
{
    public class EventDataModel : DataModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public EventDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public EventDataModel(XElement xmlObject) : base(xmlObject) { }

    }
}
