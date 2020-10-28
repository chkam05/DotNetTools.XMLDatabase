using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Tools.XMLDatabase.Exceptions;

namespace Tools.XMLDatabase.Tools
{
    class DataImporter
    {

        //  METHODS

        /// <summary> Import XML data from file. </summary>
        /// <param name="filePath"> Path to XML file. </param>
        /// <returns> XML data as root XElement. </returns>
        public static XElement FromFile(string filePath)
        {
            //  Check if filePath has been passed into method.
            if (string.IsNullOrEmpty(filePath))
                throw new Exceptions.FileNotFoundException();

            //  Check if file exists.
            if (!File.Exists(filePath))
                throw new Exceptions.FileNotFoundException(filePath);

            try
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    //  Load XML data as root XElement from file.
                    var root = XElement.Load(streamReader);
                    streamReader.Close();

                    return root;
                }
            }
            catch (Exception)
            {
                throw new LoadFileException(filePath);
            }
        }

    }
}
