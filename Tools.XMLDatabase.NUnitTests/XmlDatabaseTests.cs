using chkam05.DotNetTools.XMLDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.XMLDatabase.NUnitTests.Data.ComplexTestModels;

namespace Tools.XMLDatabase.NUnitTests
{
    public class XmlDatabaseTests
    {

        //  VARIABLES

        IXmlDatabase _database;


        //  METHODS

        #region SETUP METHODS

        [SetUp]
        public void Setup()
        {
            _database = new XmlDatabase();

            _database.RegisterDataModel<EventDataModel>();
            _database.RegisterDataModel<FileDataModel>();
            _database.RegisterDataModel<MemberDataModel>();
        }

        #endregion SETUP METHODS

        #region TEST METHODS

        [Test]
        public void AddGetDataTest()
        {
            //  Clear database.
            _database.ClearDatabase();

            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);
            Assert.True(_database.GetObjects<FileDataModel>().Count == 0);
            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);

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

            //  Create Event test data.
            var eventTest1 = new EventDataModel()
            {
                Date = new DateTime(2016, 01, 23, 19, 00, 00),
                Name = "Engagement",
                Description = "Snowy winter evening"
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

            //  Add models into database.
            Assert.True(_database.AddObject(memberTest1));
            Assert.True(_database.AddObject(eventTest1));
            Assert.True(_database.AddObject(fileTest1));

            //  Get models from database.
            var resultMemberTest = _database.GetObject<MemberDataModel>(memberTest1.Id);
            var resultEventTest = _database.GetObject<EventDataModel>(eventTest1.Id);
            var resultFileTest = _database.GetObject<FileDataModel>(fileTest1.Id);

            //  Test data.
            Assert.NotNull(resultMemberTest);
            Assert.NotNull(resultEventTest);
            Assert.NotNull(resultFileTest);

            Assert.AreEqual(resultMemberTest.Id, memberTest1.Id);
            Assert.AreEqual(resultMemberTest.FirstName, memberTest1.FirstName);
            Assert.AreEqual(resultMemberTest.Surname, memberTest1.Surname);
            Assert.AreEqual(resultMemberTest.BirthDate, memberTest1.BirthDate);
            Assert.AreEqual(resultMemberTest.Balance, memberTest1.Balance);
            Assert.NotNull(resultMemberTest.EventDates);
            Assert.AreEqual(resultMemberTest.EventDates.Count, memberTest1.EventDates.Count);
            Assert.True(resultMemberTest.EventDates.Contains(memberTest1.EventDates[0]));
            Assert.NotNull(resultMemberTest.Files);

            Assert.AreEqual(resultEventTest.Id, eventTest1.Id);
            Assert.AreEqual(resultEventTest.Name, eventTest1.Name);
            Assert.AreEqual(resultEventTest.Description, eventTest1.Description);
            Assert.AreEqual(resultEventTest.Date, eventTest1.Date);

            Assert.AreEqual(resultFileTest.Id, fileTest1.Id);
            Assert.AreEqual(resultFileTest.Name, fileTest1.Name);
            Assert.AreEqual(resultFileTest.Type, fileTest1.Type);
            Assert.AreEqual(resultFileTest.Extension, fileTest1.Extension);
            Assert.AreEqual(resultFileTest.Path, fileTest1.Path);
        }

        [Test]
        public void GetListDataTest()
        {
            //  Clear database.
            _database.ClearDatabase();

            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);
            Assert.True(_database.GetObjects<FileDataModel>().Count == 0);
            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);

            //  Create Members test data.
            var memberTest1 = new MemberDataModel()
            {
                FirstName = "Kamil",
                Surname = "Karpinski",
                Description = "Developer"
            };

            var memberTest2 = new MemberDataModel()
            {
                FirstName = "Jan",
                Surname = "Kowalski",
                Description = "Test object 2"
            };

            var memberTest3 = new MemberDataModel()
            {
                FirstName = "Marcin",
                Surname = "Nowak",
                Description = "Test object 3"
            };

            var memberTest4 = new MemberDataModel()
            {
                FirstName = "Marcin",
                Surname = "Wójcik",
                Description = "Test object 3"
            };

            //  Add models into database.
            Assert.True(_database.AddObject(memberTest1));
            Assert.True(_database.AddObject(memberTest2));
            Assert.True(_database.AddObject(memberTest3));
            Assert.False(_database.AddObject(memberTest3));
            Assert.True(_database.AddObject(memberTest4));

            //  Get models from database.
            var resultMembersTest = _database.GetObjects<MemberDataModel>();
            var resultMemberQueryTest = _database.GetObjects<MemberDataModel>(q => q.FirstName.ToLower() == "marcin");

            //  Test data.
            Assert.NotNull(resultMembersTest);
            Assert.NotNull(resultMemberQueryTest);
            Assert.True(resultMembersTest.Count == 4);
            Assert.True(resultMemberQueryTest.Count == 2);

            Assert.True(resultMembersTest.Where(q => q.Id == memberTest1.Id).Any());
            Assert.True(resultMembersTest.Where(q => q.Id == memberTest2.Id).Any());
            Assert.True(resultMembersTest.Where(q => q.Id == memberTest3.Id).Any());
            Assert.True(resultMembersTest.Where(q => q.Id == memberTest4.Id).Any());

            Assert.False(resultMemberQueryTest.Where(q => q.Id == memberTest1.Id).Any());
            Assert.False(resultMemberQueryTest.Where(q => q.Id == memberTest2.Id).Any());
            Assert.True(resultMemberQueryTest.Where(q => q.Id == memberTest3.Id).Any());
            Assert.True(resultMemberQueryTest.Where(q => q.Id == memberTest4.Id).Any());
        }

        [Test]
        public void RemoveDataTest()
        {
            //  Clear database.
            _database.ClearDatabase();

            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);
            Assert.True(_database.GetObjects<FileDataModel>().Count == 0);
            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);

            //  Create Members test data.
            var memberTest1 = new MemberDataModel()
            {
                FirstName = "Kamil",
                Surname = "Karpinski",
                Description = "Developer"
            };

            var memberTest2 = new MemberDataModel()
            {
                FirstName = "Jan",
                Surname = "Kowalski",
                Description = "Test object 2"
            };

            var memberTest3 = new MemberDataModel()
            {
                FirstName = "Marcin",
                Surname = "Nowak",
                Description = "Test object 3"
            };

            //  Add models into database.
            Assert.True(_database.AddObject(memberTest1));
            Assert.True(_database.AddObject(memberTest2));
            Assert.True(_database.AddObject(memberTest3));

            //  Get models from database.
            var firstResultMembersTest = _database.GetObjects<MemberDataModel>();

            //  First test data.
            Assert.NotNull(firstResultMembersTest);
            Assert.True(firstResultMembersTest.Count == 3);

            //  Remove data.
            Assert.True(_database.RemoveObject(memberTest2));
            Assert.True(firstResultMembersTest.Where(q => q.Id == memberTest2.Id).Any());

            //  Get models from database.
            var secondResultMembersTest = _database.GetObjects<MemberDataModel>();

            //  Second test data.
            Assert.NotNull(secondResultMembersTest);
            Assert.True(secondResultMembersTest.Count == 2);
            Assert.False(secondResultMembersTest.Where(q => q.Id == memberTest2.Id).Any());
        }

        [Test]
        public void UpdateDataTest()
        {
            //  Clear database.
            _database.ClearDatabase();

            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);
            Assert.True(_database.GetObjects<FileDataModel>().Count == 0);
            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);

            //  Create Members test data.
            var memberTest1 = new MemberDataModel()
            {
                FirstName = "Jan",
                Surname = "Kowalski",
                Description = "Test object 2",
                BirthDate = new DateTime(1997, 01, 01, 0, 0, 0),
                Balance = 912.46,
                EventDates = new List<string>()
            };

            var memberTest2 = new MemberDataModel()
            {
                FirstName = "Marcin",
                Surname = "Nowak",
                Description = "Test object 3"
            };

            //  Create Events test data.
            var eventTest1 = new EventDataModel()
            {
                Date = new DateTime(2016, 01, 23, 19, 00, 00),
                Name = "Wedding",
                Description = "Snowy winter evening"
            };

            var eventTest2 = new EventDataModel()
            {
                Date = new DateTime(2013, 11, 20, 0, 0, 0),
                Name = "Engagement",
                Description = "Cold autumn evening"
            };

            //  Correct test data.
            memberTest1.EventDates.Add(eventTest1.Id);
            memberTest1.EventDates.Add(eventTest2.Id);

            //  Add models into database.
            Assert.True(_database.AddObject(memberTest1));
            Assert.True(_database.AddObject(memberTest2));
            Assert.True(_database.AddObject(eventTest1));
            Assert.True(_database.AddObject(eventTest2));

            //  Get models from database.
            var resultMember1Test = _database.GetObject<MemberDataModel>(memberTest1.Id);
            var resultMember2Test = _database.GetObject<MemberDataModel>(memberTest2.Id);
            var resultEvent1Test = _database.GetObject<EventDataModel>(eventTest1.Id);
            var resultEvent2Test = _database.GetObject<EventDataModel>(eventTest2.Id);

            //  First test data.
            Assert.NotNull(resultMember1Test);
            Assert.NotNull(resultMember2Test);
            Assert.NotNull(resultEvent1Test);
            Assert.NotNull(resultEvent2Test);

            Assert.NotNull(resultMember1Test.EventDates);
            Assert.True(resultMember1Test.EventDates.Count == 2);
            Assert.AreEqual(resultMember1Test.FirstName, memberTest1.FirstName);
            Assert.AreEqual(resultMember1Test.Surname, memberTest1.Surname);
            Assert.AreEqual(resultMember1Test.BirthDate, memberTest1.BirthDate);
            Assert.AreEqual(resultMember1Test.Description, memberTest1.Description);

            Assert.True(resultMember1Test.EventDates.Where(q => q == resultEvent1Test.Id).Any());
            Assert.True(resultMember1Test.EventDates.Where(q => q == resultEvent2Test.Id).Any());

            //  Update models data.
            memberTest1.FirstName = "Kamil";
            memberTest1.Surname = "Karpinski";
            memberTest1.Description = "Developer";
            memberTest1.BirthDate = new DateTime(1994, 12, 05, 0, 0, 0);
            memberTest1.EventDates.Remove(eventTest2.Id);

            eventTest1.Name = "Engagement";

            var eventTest3 = new EventDataModel()
            {
                Date = new DateTime(2014, 04, 30, 0, 0, 0),
                Name = "End School",
                Description = "Technikum"
            };

            memberTest1.EventDates.Add(eventTest3.Id);

            //  Update models in database.
            Assert.True(_database.UpdateObject(memberTest1));
            Assert.True(_database.UpdateObject(eventTest1));
            Assert.True(_database.AddObject(eventTest3));

            //  Get models from database.
            var secondResultMember1Test = _database.GetObject<MemberDataModel>(memberTest1.Id);
            var secondResultEvent1Test = _database.GetObject<EventDataModel>(eventTest1.Id);
            var resultEvent3Test = _database.GetObject<EventDataModel>(eventTest3.Id);

            Assert.NotNull(secondResultMember1Test);
            Assert.NotNull(secondResultEvent1Test);
            Assert.NotNull(resultEvent3Test);

            Assert.NotNull(secondResultMember1Test.EventDates);
            Assert.True(secondResultMember1Test.EventDates.Count == 2);

            Assert.AreNotEqual(secondResultMember1Test.FirstName, resultMember1Test.FirstName);
            Assert.AreNotEqual(secondResultMember1Test.Surname, resultMember1Test.Surname);
            Assert.AreNotEqual(secondResultMember1Test.BirthDate, resultMember1Test.BirthDate);
            Assert.AreNotEqual(secondResultMember1Test.Description, resultMember1Test.Description);
            Assert.AreEqual(secondResultMember1Test.FirstName, memberTest1.FirstName);
            Assert.AreEqual(secondResultMember1Test.Surname, memberTest1.Surname);
            Assert.AreEqual(secondResultMember1Test.BirthDate, memberTest1.BirthDate);
            Assert.AreEqual(secondResultMember1Test.Description, memberTest1.Description);

            Assert.AreNotEqual(secondResultEvent1Test.Name, resultEvent1Test.Name);
            Assert.AreEqual(secondResultEvent1Test.Name, eventTest1.Name);

            Assert.True(secondResultMember1Test.EventDates.Where(q => q == resultEvent1Test.Id).Any());
            Assert.False(secondResultMember1Test.EventDates.Where(q => q == resultEvent2Test.Id).Any());
            Assert.True(secondResultMember1Test.EventDates.Where(q => q == resultEvent3Test.Id).Any());
        }

        [Test]
        public void ClearDataTest()
        {
            //  Clear database.
            _database.ClearDatabase();

            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);
            Assert.True(_database.GetObjects<FileDataModel>().Count == 0);
            Assert.True(_database.GetObjects<MemberDataModel>().Count == 0);

            //  Create Members test data.
            var memberTest1 = new MemberDataModel()
            {
                FirstName = "Kamil",
                Surname = "Karpinski",
                Description = "Developer"
            };

            var memberTest2 = new MemberDataModel()
            {
                FirstName = "Jan",
                Surname = "Kowalski",
                Description = "Test object 2"
            };

            var memberTest3 = new MemberDataModel()
            {
                FirstName = "Marcin",
                Surname = "Nowak",
                Description = "Test object 3"
            };

            //  Add models into database.
            Assert.True(_database.AddObject(memberTest1));
            Assert.True(_database.AddObject(memberTest2));
            Assert.True(_database.AddObject(memberTest3));

            //  Get models from database.
            var firstResultMembersTest = _database.GetObjects<MemberDataModel>();

            //  First test data.
            Assert.NotNull(firstResultMembersTest);
            Assert.True(firstResultMembersTest.Count == 3);

            _database.ClearDatabase();

            //  Get models from database.
            var secondResultMembersTest = _database.GetObjects<MemberDataModel>();

            //  Second test data.
            Assert.NotNull(firstResultMembersTest);
            Assert.True(secondResultMembersTest.Count == 0);
        }

        #endregion TEST METHODS

    }
}
