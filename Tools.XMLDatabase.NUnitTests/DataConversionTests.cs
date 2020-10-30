using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.NUnitTests.Data.ConversionTestModels;
using Tools.XMLDatabase.NUnitTests.Data.SimpleModels;

namespace Tools.XMLDatabase.NUnitTests
{
    public class DataConversionTests
    {

        //  VARIABLES

        //  METHODS

        #region SETUP METHODS

        [SetUp]
        public void Setup()
        {
        }

        #endregion SETUP METHODS

        #region TEST METHODS

        [Test]
        public void ArraysAsXmlTest()
        {
            //  Create Array data.
            var testData = new ConversionArraysDataModel()
            {
                BoolArray = new[] { true, false, true },
                DateTimeArray = new[]
                {
                    new DateTime(1994, 12, 05, 0, 0, 0),
                    new DateTime(1997, 01, 01, 0, 0, 0),
                    new DateTime(1999, 08, 27, 0, 0, 0)
                },
                DoubleArray = new[] { 1.99, 2.87, 3.16 },
                EnumArray = new[]
                {
                    SimpleEnum.CANCEL,
                    SimpleEnum.NO,
                    SimpleEnum.YES
                },
                FloatArray = new[] { 1.99f, 2.87f, 3.16f },
                IntArray = new[] { 6, 7, 19 },
                LongArray = new long[] { 5, 9, 30 },
                StringArray = new[] { "test", "array", "of", "string" }
            };

            //  Convert to XML.
            var xmlObject = testData.AsXml();

            //  Convert back to List.
            var resultData = new ConversionArraysDataModel(xmlObject);
        }

        [Test]
        public void DictionariesAsXmlTest()
        {
            //  Create Dictionary data.
            var testData = new ConversionDictionariesDataModel()
            {
                BoolDict = new Dictionary<string, bool>()
                {
                    { "1st", true },
                    { "2nd", false },
                    { "3rd", true }
                },
                DateTimeDict = new Dictionary<string, DateTime>()
                {
                    { "Self", new DateTime(1994, 12, 5, 0, 0, 0) },
                    { "Ion", new DateTime(1997, 1, 1, 0, 0, 0) },
                    { "Math", new DateTime(1999, 8, 27, 0, 0, 0) }
                },
                DoubleDict = new Dictionary<string, double>()
                {
                    { "Low", 1.99 },
                    { "Mid", 2.87 },
                    { "High", 3.16 }
                },
                EnumDict = new Dictionary<string, SimpleEnum>()
                {
                    { "Cancel", SimpleEnum.CANCEL },
                    { "No", SimpleEnum.NO },
                    { "Yes", SimpleEnum.YES }
                },
                FloatDict = new Dictionary<string, float>()
                {
                    { "Low", 1.99f },
                    { "Mid", 2.87f },
                    { "High", 3.16f }
                },
                IntDict = new Dictionary<string, int>()
                {
                    { "Brynow - Karb", 6 },
                    { "Zawodzie - pl. Sikorskiego", 7 },
                    { "pl. Wolnosci - Stroszek", 19 }
                },
                LongDict = new Dictionary<string, long>()
                {
                    { "pl. Sikorskiego - Zaborze", 5 },
                    { "pl. Sikorskiego - Chorzow", 9 },
                    { "pl. Sikorskiego - Biskupice", 30 }
                },
                StringDict = new Dictionary<int, string>()
                {
                    { 0, "Test" },
                    { 1, "dictionary" },
                    { 2, "of" },
                    { 3, "string" }
                }
            };

            //  Convert to XML.
            var xmlObject = testData.AsXml();

            //  Convert back to List.
            var resultData = new ConversionDictionariesDataModel(xmlObject);
        }

        [Test]
        public void IterableNullablesAsXmlTest()
        {
            //  Create Iterable Nullable data.
            var testData = new ConversionIterableNullableDataModel()
            {
                BoolNullableArray = new bool?[] { true, null, false },
                BoolNullableDict = new Dictionary<int, bool?>
                {
                    { 0, true },
                    { 1, false },
                    { 2, null }
                },
                BoolNullableList = new List<bool?>() { false, null, true },
                DateTimeNullableArray = new DateTime?[]
                {
                    new DateTime(1994, 12, 05, 0, 0, 0),
                    null,
                    new DateTime(1999, 08, 27, 0, 0, 0)
                },
                DateTimeNullableDict = new Dictionary<string, DateTime?>()
                {
                    { "Self", new DateTime(1994, 12, 5, 0, 0, 0) },
                    { "Ion", null },
                    { "Math", new DateTime(1999, 8, 27, 0, 0, 0) }
                },
                DateTimeNullableList = new List<DateTime?>()
                {
                    new DateTime(1994, 12, 05, 0, 0, 0),
                    null,
                    new DateTime(1999, 08, 27, 0, 0, 0)
                },
                EnumNullableArray = new SimpleEnum?[]
                {
                    SimpleEnum.CANCEL,
                    null,
                    SimpleEnum.YES
                },
                EnumNullableDict = new Dictionary<double, SimpleEnum?>()
                {
                    { 1.1, SimpleEnum.CANCEL },
                    { 1.2, SimpleEnum.NO },
                    { 1.3, null }
                },
                EnumNullableList = new List<SimpleEnum?>()
                {
                    SimpleEnum.CANCEL,
                    null,
                    SimpleEnum.YES
                },
                IntNullableArray = new int?[] { 6, null, 19 },
                IntNullableDict = new Dictionary<SimpleEnum, int?>()
                {
                    { SimpleEnum.CANCEL, 6 },
                    { SimpleEnum.NO, 7 },
                    { SimpleEnum.YES, null }
                },
                IntNullableList = new List<int?>() { 6, null, 19 }
            };

            //  Convert to XML.
            var xmlObject = testData.AsXml();

            //  Convert back to List.
            var resultData = new ConversionIterableNullableDataModel(xmlObject);
        }

        [Test]
        public void ListsAsXmlTest()
        {

            //  Create List data.
            var testData = new ConversionListsDataModel()
            {
                BoolList = new List<bool>() { true, false, true },
                DateTimeList = new List<DateTime>()
                {
                    new DateTime(1994, 12, 05, 0, 0, 0),
                    new DateTime(1997, 01, 01, 0, 0, 0),
                    new DateTime(1999, 08, 27, 0, 0, 0)
                },
                DoubleList = new List<double>() { 1.99, 2.87, 3.16 },
                EnumList = new List<SimpleEnum>()
                {
                    SimpleEnum.CANCEL,
                    SimpleEnum.NO,
                    SimpleEnum.YES
                },
                FloatList = new List<float>() { 1.99f, 2.87f, 3.16f },
                IntList = new List<int>() { 6, 7, 19 },
                LongList = new List<long>() { 5, 9, 30 },
                StringList = new List<string>() { "test", "list", "of", "string" }
            };

            //  Convert to XML.
            var xmlObject = testData.AsXml();

            //  Convert back to List.
            var resultData = new ConversionListsDataModel(xmlObject);
        }

        [Test]
        public void NullablesAsXmlFullTest()
        {
            var testData = new ConversionNullableDataModels()
            {
                BoolNullableValue = true,
                DateTimeNullableValue = new DateTime(1994, 12, 05, 0, 0, 0),
                DoubleNullableValue = 1.99,
                EnumNullableValue = SimpleEnum.YES,
                FloatNullableValue = 1.99f,
                IntNullableValue = 6,
                LongNullableValue = 19
            };

            //  Convert to XML.
            var xmlObject = testData.AsXml();

            //  Convert back to List.
            var resultData = new ConversionNullableDataModels(xmlObject);
        }

        [Test]
        public void NullablesAsXmlEmptyTest()
        {
            var testData = new ConversionNullableDataModels()
            {
                BoolNullableValue = null,
                DateTimeNullableValue = null,
                DoubleNullableValue = null,
                EnumNullableValue = null,
                FloatNullableValue = null,
                IntNullableValue = null,
                LongNullableValue = null
            };

            //  Convert to XML.
            var xmlObject = testData.AsXml();

            //  Convert back to List.
            var resultData = new ConversionNullableDataModels(xmlObject);
        }

        [Test]
        public void StaticsAsXmlTest()
        {
            var testData = new ConversionStaticsDataModel()
            {
                BoolValue = true,
                DateTimeValue = new DateTime(1994, 12, 05, 0, 0, 0),
                DoubleValue = 1.99,
                EnumValue = SimpleEnum.YES,
                FloatValue = 1.99f,
                IntValue = 6,
                LongValue = 19,
                StringValue = "test static"
            };

            //  Convert to XML.
            var xmlObject = testData.AsXml();

            //  Convert back to List.
            var resultData = new ConversionStaticsDataModel(xmlObject);
        }

        #endregion TEST METHODS

    }
}
