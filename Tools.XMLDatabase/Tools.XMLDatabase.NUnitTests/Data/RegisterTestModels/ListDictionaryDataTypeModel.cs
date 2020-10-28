using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class ListDictionaryDataTypeModel : DataModel
    {
        public List<Dictionary<string, int>> Values { get; set; }
    }
}
