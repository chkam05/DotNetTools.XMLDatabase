using chkam05.DotNetTools.XMLDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.Exceptions;
using Tools.XMLDatabase.NUnitTests.Data.RegisterTestModels;

namespace Tools.XMLDatabase.NUnitTests
{
    public class RegisterDataModelTests
    {

        //  VARIABLES

        IXmlDatabase _database;


        //  METHODS

        #region SETUP METHODS

        [SetUp]
        public void Setup()
        {
            //  Create XML database.
            _database = new XmlDatabase();
        }

        #endregion SETUP METHODS

        #region HELPER METHODS

        private void AssertFailRegister<TDataModel>()
            where TDataModel : DataModel
        {
            try
            {
                _database.RegisterDataModel<TDataModel>();
            }
            catch (InvalidDataModelPropertyException exc)
            {
                Console.WriteLine($"Test correct exception detected: {exc.Message}");
                return;
            }

            Assert.Fail();
        }

        private void AssertPassRegister<TDataModel>()
            where TDataModel : DataModel
        {
            try
            {
                _database.RegisterDataModel<TDataModel>();
            }
            catch (InvalidDataModelPropertyException exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        #endregion HELPER METHODS

        #region TEST METHODS

        [Test]
        public void RegisterStandardModelsTest()
        {
            AssertPassRegister<BoolDataTypeModel>();
            AssertPassRegister<DateTimeDataTypeModel>();
            AssertPassRegister<DoubleDataTypeModel>();
            AssertPassRegister<EnumDataTypeModel>();
            AssertPassRegister<IntDataTypeModel>();
            AssertPassRegister<StringDataTypeModel>();
        }

        [Test]
        public void RegisterArrayModelsTest()
        {
            AssertPassRegister<BoolArrayDataTypeModel>();
            AssertPassRegister<DateTimeArrayDataTypeModel>();
            AssertPassRegister<DoubleArrayDataTypeModel>();
            AssertPassRegister<EnumArrayDataTypeModel>();
            AssertPassRegister<IntArrayDataTypeModel>();
            AssertPassRegister<StringArrayDataTypeModel>();
        }

        [Test]
        public void RegisterListModelsTest()
        {
            AssertPassRegister<BoolListDataTypeModel>();
            AssertPassRegister<DateTimeListDataTypeModel>();
            AssertPassRegister<DoubleListDataTypeModel>();
            AssertPassRegister<EnumListDataTypeModel>();
            AssertPassRegister<IntListDataTypeModel>();
            AssertPassRegister<StringListDataTypeModel>();
        }

        [Test]
        public void RegisterDictionaryModelsTest()
        {
            AssertPassRegister<BoolDictionaryDataTypeModel>();
            AssertPassRegister<DateTimeDictionaryDataTypeModel>();
            AssertPassRegister<DoubleDictionaryDataTypeModel>();
            AssertPassRegister<EnumDictionaryDataTypeModel>();
            AssertPassRegister<IntDictionaryDataTypeModel>();
            AssertPassRegister<StringDictionaryDataTypeModel>();
        }

        [Test]
        public void RegisterNullableModelsTest()
        {
            AssertPassRegister<BoolNullableDataTypeModel>();
            AssertPassRegister<DateTimeNullableDataTypeModel>();
            AssertPassRegister<DoubleNullableDataTypeModel>();
            AssertPassRegister<EnumNullableDataTypeModel>();
            AssertPassRegister<IntNullableDataTypeModel>();
        }

        [Test]
        public void RegisterForbiddenModelsTest()
        {
            AssertFailRegister<ArrayArrayDataTypeModel>();
            AssertFailRegister<ArrayDictionaryDataTypeModel>();
            AssertFailRegister<ArrayListDataTypeModel>();
            AssertFailRegister<ClassDataTypeModel>();
            AssertFailRegister<ClassArrayDataTypeModel>();
            AssertFailRegister<ClassDictionaryDataTypeModel>();
            AssertFailRegister<ClassListDataTypeModel>();
            AssertFailRegister<DictionaryArrayDataTypeModel>();
            AssertFailRegister<DictionaryDictionaryDataTypeModel>();
            AssertFailRegister<DictionaryListDataTypeModel>();
            AssertFailRegister<ListArrayDataTypeModel>();
            AssertFailRegister<ListDictionaryDataTypeModel>();
            AssertFailRegister<ListListDataTypeModel>();
        }

        #endregion TEST METHODS

    }
}
