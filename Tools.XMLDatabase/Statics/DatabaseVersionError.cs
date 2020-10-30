using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.XMLDatabase.Statics
{
    public enum DatabaseVersionError
    {
        VERSION_CURRENT = 0,
        VERSION_NEWER = 1,
        VERSION_OLDER = 2,
        NO_VERSION = 3
    }
}
