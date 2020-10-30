using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class ListArrayDataTypeModel : DataModel
    {
        public List<int[]> Values { get; set; }
    }
}
