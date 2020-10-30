using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Exceptions;

namespace Tools.XMLDatabase.Tools
{
    class DataExporter
    {

        //  METHODS

        /// <summary> Import XML data from XElement root to file. </summary>
        /// <param name="root"> XElement root with XML data. </param>
        /// <param name="filePath"> Path to XML file. </param>
        public static void ToFile(XElement root, string filePath)
        {
            //  Check if filePath has been passed into method.
            if (string.IsNullOrEmpty(filePath))
                throw new InvalidFilePathException();

            //  Check if filePath is valid filePath.
            if (!File.Exists(filePath))
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    throw new InvalidFilePathException(filePath);

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    //  Save XElement root as XML data in file.
                    root.Save(streamWriter);
                    streamWriter.Close();
                }
            }
            catch (Exception)
            {
                var fileName = Path.GetFileName(filePath);
                throw new SaveFileException(fileName);
            }
        }

    }
}
