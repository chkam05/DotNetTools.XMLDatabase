using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class DateTimeDictionaryDataTypeModel : DataModel
    {
        public Dictionary<string, DateTime> Values { get; set; }
    }
}
