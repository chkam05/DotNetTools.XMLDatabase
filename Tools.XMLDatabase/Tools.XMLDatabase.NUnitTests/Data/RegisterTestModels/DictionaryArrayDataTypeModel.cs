﻿using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;

namespace Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels
{
    public class DictionaryArrayDataTypeModel : DataModel
    {
        public Dictionary<string, int[]> Values { get; set; }
    }
}
