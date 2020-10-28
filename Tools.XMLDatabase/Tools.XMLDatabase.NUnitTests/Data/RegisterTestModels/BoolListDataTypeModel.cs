using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class BoolListDataTypeModel : DataModel
    {
        public List<bool> Values { get; set; }
    }
}
