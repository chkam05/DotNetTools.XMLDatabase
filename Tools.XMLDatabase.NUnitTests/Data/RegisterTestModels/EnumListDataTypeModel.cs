using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class EnumListDataTypeModel : DataModel
    {
        public List<SimpleEnum> Values { get; set; }
    }
}
