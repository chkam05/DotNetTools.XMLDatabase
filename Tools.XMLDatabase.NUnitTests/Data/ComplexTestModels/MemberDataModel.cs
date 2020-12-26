using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.ComplexTestModels
{
    public class MemberDataModel : DataModel
    {

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Description { get; set; }
        public double Balance { get; set; }

        public List<string> EventDates { get; set; }
        public List<string> Files { get; set; }


        /// <summary> Required constructor for Data Model. </summary>
        public MemberDataModel() : base() { }

        /// <summary> Required constructor with XElement object for Data Model. </summary>
        /// <param name="xmlObject"> XML database object with data to insert inside class instance. </param>
        public MemberDataModel(XElement xmlObject, XmlDatabaseOptions options = null) : base(xmlObject, options) { }

    }
}
