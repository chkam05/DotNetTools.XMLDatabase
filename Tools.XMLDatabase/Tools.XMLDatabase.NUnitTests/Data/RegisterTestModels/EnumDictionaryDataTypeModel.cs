using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class EnumDictionaryDataTypeModel : DataModel
    {
        public Dictionary<string, SimpleEnum> Values { get; set; }
    }
}
