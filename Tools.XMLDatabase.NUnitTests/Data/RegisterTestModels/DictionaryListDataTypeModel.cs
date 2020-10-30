using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class DictionaryListDataTypeModel : DataModel
    {
        public Dictionary<string, List<int>> Values { get; set; }
    }
}
