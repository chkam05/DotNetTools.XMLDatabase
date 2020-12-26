using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Statics;

namespace Tools.XMLDatabase.Data
{
    public class XmlDatabaseOptions
    {

        //  VARIABLES

        public XMLDatabaseVersion CurrentVersion = null;
        public XMLDatabaseVersion MinimalVersion = null;
        public TypesCoding TypesCoding = TypesCoding.SIMPLE;


        //  METHODS

        #region CLASS METHODS

        public XmlDatabaseOptions() { }

        #endregion CLASS METHODS

    }
}
