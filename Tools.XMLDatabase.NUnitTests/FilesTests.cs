using chkam05.DotNetTools.XMLDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tools.XMLDatabase.NUnitTests.Data.ComplexTestModels;

namespace Tools.XMLDatabase.NUnitTests
{
    public class FilesTests
    {

        //  VARIABLES
        private string _testLoadFilePath = Path.Combine(AppContext.BaseDirectory, "Resources", "LoadTestFile.xml");
        private string _testSaveFilePath = Path.Combine(AppContext.BaseDirectory, "TestFile.xml");


        //  METHODS

        #region SETUP METHODS

        [SetUp]
        public void Setup()
        {
        }

        #endregion SETUP METHODS

        #region TEST METHODS

        [Test]
        public void LoadTest()
        {
            //  Create load database, and load data.
            var loadDatabase = new XmlDatabase(_testLoadFilePath);
            loadDatabase.RegisterDataModel<MemberDataModel>();
            loadDatabase.RegisterDataModel<EventDataModel>();
            loadDatabase.RegisterDataModel<FileDataModel>();

            //  Get models from database.
            var resultMember1Test = loadDatabase.GetObject<MemberDataModel>("ae57ad7fe82c47e99e5a91fe817d19e1");
            var resultEvent1Test = loadDatabase.GetObject<EventDataModel>("df188954af5b4303b9ac71fd209caddb");
            var resultFile1Test = loadDatabase.GetObject<FileDataModel>("6db4ac068f3548a6bb6acb4fb8ba0925");

            //  Test data.
            Assert.NotNull(resultMember1Test);
            Assert.NotNull(resultEvent1Test);
            Assert.NotNull(resultFile1Test);

            Assert.AreEqual(resultMember1Test.FirstName, "Kamil");
            Assert.True(string.IsNullOrEmpty(resultMember1Test.SecondName));
            Assert.AreEqual(resultMember1Test.BirthDate, new DateTime(1994, 12, 05, 0, 0, 0));
            Assert.AreEqual(resultMember1Test.Description, "Developer");
            Assert.AreEqual(resultEvent1Test.Name, "Engagement");
            Assert.AreEqual(resultEvent1Test.Description, "Snowy winter evening");
            Assert.AreEqual(resultEvent1Test.Date, new DateTime(2016, 01, 23, 19, 0, 0));
            Assert.AreEqual(resultFile1Test.Name, "DSC_0001.jpg");
            Assert.AreEqual(resultFile1Test.Type, "JPEG/IMAGE");
        }

        [Test]
        public void SaveTest()
        {
            //  Create Member test data.
            var memberTest1 = new MemberDataModel()
            {
                FirstName = "Kamil",
                Surname = "Karpinski",
                BirthDate = new DateTime(1994, 12, 05, 0, 0, 0),
                Description = "Developer",
                Balance = 1234.56,
                EventDates = new List<string>()
            };

            var memberTest2 = new MemberDataModel()
            {
                FirstName = "Jan",
                Surname = "Kowalski",
                BirthDate = new DateTime(1999, 08, 27, 0, 0, 0),
                Files = new List<string>()
            };

            //  Create Event test data.
            var eventTest1 = new EventDataModel()
            {
                Date = new DateTime(2016, 01, 23, 19, 00, 00),
                Name = "Engagement",
                Description = "Snowy winter evening"
            };

            var eventTest2 = new EventDataModel()
            {
                Date = new DateTime(1997, 01, 01, 0, 0, 0),
                Name = "Fiancee birthday",
                Description = "No description"
            };

            //  Create File test data.
            var fileTest1 = new FileDataModel()
            {
                Name = "DSC_0001.jpg",
                Extension = ".jpg",
                Path = @"C:\Users\Kamil\Pictures\",
                Type = "JPEG/IMAGE"
            };

            //  Correct test data.
            memberTest1.EventDates.Add(eventTest1.Id);
            memberTest1.EventDates.Add(eventTest2.Id);
            memberTest2.Files.Add(fileTest1.Id);

            //  Create save database, seed data, and save.
            var saveDatabase = new XmlDatabase();
            saveDatabase.RegisterDataModel<MemberDataModel>();
            saveDatabase.RegisterDataModel<EventDataModel>();
            saveDatabase.RegisterDataModel<FileDataModel>();
            saveDatabase.AddObject(memberTest1);
            saveDatabase.AddObject(memberTest2);
            saveDatabase.AddObject(eventTest1);
            saveDatabase.AddObject(eventTest2);
            saveDatabase.AddObject(fileTest1);
            saveDatabase.SaveToFile(_testSaveFilePath);

            //  Create load database, and load data.
            var loadDatabase = new XmlDatabase(_testSaveFilePath);
            loadDatabase.RegisterDataModel<MemberDataModel>();
            loadDatabase.RegisterDataModel<EventDataModel>();
            loadDatabase.RegisterDataModel<FileDataModel>();

            //  Get models from database.
            var resultMember1Test = loadDatabase.GetObject<MemberDataModel>(memberTest1.Id);
            var resultMember2Test = loadDatabase.GetObject<MemberDataModel>(memberTest2.Id);
            var resultEvent1Test = loadDatabase.GetObject<EventDataModel>(eventTest1.Id);
            var resultEvent2Test = loadDatabase.GetObject<EventDataModel>(eventTest2.Id);
            var resultFile1Test = loadDatabase.GetObject<FileDataModel>(fileTest1.Id);
            var resultFile2Test = loadDatabase.GetObject<FileDataModel>("d94860a9feda43e2be393b6d5335cc3a");

            //  Test data.
            Assert.NotNull(resultMember1Test);
            Assert.NotNull(resultMember2Test);
            Assert.NotNull(resultEvent1Test);
            Assert.NotNull(resultEvent2Test);
            Assert.NotNull(resultFile1Test);
            Assert.Null(resultFile2Test);

            Assert.AreEqual(resultMember1Test.FirstName, memberTest1.FirstName);
            Assert.AreEqual(resultMember2Test.SecondName, memberTest2.SecondName);
            Assert.AreEqual(resultMember1Test.BirthDate, memberTest1.BirthDate);
            Assert.AreEqual(resultMember2Test.Description, memberTest2.Description);
            Assert.AreEqual(resultEvent1Test.Name, eventTest1.Name);
            Assert.AreEqual(resultEvent2Test.Name, eventTest2.Name);
            Assert.AreEqual(resultEvent1Test.Date, eventTest1.Date);
            Assert.AreEqual(resultFile1Test.Name, resultFile1Test.Name);
            Assert.AreEqual(resultFile1Test.Type, resultFile1Test.Type);

            Assert.NotNull(resultMember1Test.EventDates);
            Assert.True(resultMember1Test.EventDates.Count == 2);

            Assert.NotNull(resultMember2Test.EventDates);
            Assert.True(resultMember2Test.EventDates.Count == 0);

            Assert.NotNull(resultMember1Test.Files);
            Assert.True(resultMember1Test.Files.Count == 0);

            Assert.NotNull(resultMember2Test.Files);
            Assert.True(resultMember2Test.Files.Count == 1);

            Assert.True(resultMember1Test.EventDates.Where(q => q == eventTest1.Id).Any());
            Assert.True(resultMember1Test.EventDates.Where(q => q == eventTest2.Id).Any());
            Assert.True(resultMember2Test.Files.Where(q => q == fileTest1.Id).Any());
            Assert.False(resultMember2Test.Files.Where(q => q == "d94860a9feda43e2be393b6d5335cc3a").Any());
        }

        #endregion TEST METHODS

    }
}
