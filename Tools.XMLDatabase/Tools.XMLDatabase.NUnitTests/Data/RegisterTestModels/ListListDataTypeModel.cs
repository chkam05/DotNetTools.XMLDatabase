using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class ListListDataTypeModel : DataModel
    {
        public List<List<int>> Values { get; set; }
    }
}
