using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class DoubleDictionaryDataTypeModel : DataModel
    {
        public Dictionary<string, double> Values { get; set; }
    }
}
