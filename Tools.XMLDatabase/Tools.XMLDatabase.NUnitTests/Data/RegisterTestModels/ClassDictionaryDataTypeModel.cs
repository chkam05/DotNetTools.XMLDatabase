using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class ClassDictionaryDataTypeModel : DataModel
    {
        public Dictionary<string, InvalidDataModel> Values { get; set; }
    }
}
