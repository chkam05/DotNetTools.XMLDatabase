using chkam05.DotNetTools.XMLDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Tools.XMLDatabase.Data;
using Tools.XMLDatabase.Exceptions;
using Tools.XMLDatabase.Statics;

namespace Tools.XMLDatabase.NUnitTests
{
    public class VersioningTests
    {

        //  VARIABLES

        private string _testFilePath = Path.Combine(AppContext.BaseDirectory, "Resources", "VersionTestFile.xml");
        private string _testFileInside;


        //  METHODS

        #region SETUP METHODS

        [SetUp]
        public void Setup()
        {
            LoadTestFileInsides();
        }

        [TearDown]
        public void Final()
        {
            RevertTestFileInsides();
        }

        #endregion SETUP METHODS

        #region TEST METHODS

        [Test]
        public void TestCreateVersion()
        {
            //  Create database without options.
            var database = new XmlDatabase();

            //  Get versions.
            var fileVersion = database.GetFileVersion();
            var currentVersion = database.GetCurrentVersion();

            Assert.AreEqual(fileVersion.Major, currentVersion.Major);
            Assert.AreEqual(fileVersion.Minor, currentVersion.Minor);
            Assert.AreEqual(fileVersion.Release, currentVersion.Release);
            Assert.AreEqual(fileVersion.Revision, currentVersion.Revision);
        }

        [Test]
        public void TestCreateCustomVersion()
        {
            //  Create database without options.
            var database = new XmlDatabase(new XmlDatabaseOptions()
            {
                CurrentVersion = new XMLDatabaseVersion()
                {
                    Major = 1,
                    Minor = 3,
                    Release = 4,
                    Revision = 0
                }
            });

            //  Get versions.
            var fileVersion = database.GetFileVersion();
            var currentVersion = database.GetCurrentVersion();

            Assert.AreEqual(fileVersion.Major, currentVersion.Major);
            Assert.AreEqual(fileVersion.Minor, currentVersion.Minor);
            Assert.AreEqual(fileVersion.Release, currentVersion.Release);
            Assert.AreEqual(fileVersion.Revision, currentVersion.Revision);
        }

        [Test]
        public void TestHigherVersion()
        {
            //  Major test.
            ChangeTestFileVersion(9, 0, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_NEWER);

            //  Minor test.
            ChangeTestFileVersion(2, 9, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_NEWER);

            //  Release test.
            ChangeTestFileVersion(2, 0, 9, 0);
            TestVersionException(DatabaseVersionError.VERSION_NEWER);

            //  Revision test.
            ChangeTestFileVersion(2, 0, 0, 9);
            TestVersionException(DatabaseVersionError.VERSION_NEWER);
        }

        [Test]
        public void TestCorrectVersion()
        {
            //  Test correct version.
            ChangeTestFileVersion(1, 2, 3, 4);
            TestVersionNoException();
        }

        [Test]
        public void TestLowerVersion()
        {
            //  Major test.
            ChangeTestFileVersion(0, 9, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_OLDER);

            //  Minor test.
            ChangeTestFileVersion(1, -1, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_OLDER);

            //  Release test.
            ChangeTestFileVersion(1, 0, -1, 0);
            TestVersionException(DatabaseVersionError.VERSION_OLDER);

            //  Revision test.
            ChangeTestFileVersion(1, 0, 0, -1);
            TestVersionException(DatabaseVersionError.VERSION_OLDER);
        }

        [Test]
        public void TestCustomHigherVersion()
        {
            //  Options.
            var options = new XmlDatabaseOptions()
            {
                CurrentVersion = new XMLDatabaseVersion()
                {
                    Major = 2,
                    Minor = 3,
                    Release = 1,
                    Revision = 0
                }
            };

            //  Major test.
            ChangeTestFileVersion(3, 0, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_NEWER, options);

            //  Minor test.
            ChangeTestFileVersion(2, 4, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_NEWER, options);

            //  Release test.
            ChangeTestFileVersion(2, 3, 2, 0);
            TestVersionException(DatabaseVersionError.VERSION_NEWER, options);

            //  Revision test.
            ChangeTestFileVersion(2, 3, 1, 1);
            TestVersionException(DatabaseVersionError.VERSION_NEWER, options);
        }

        [Test]
        public void TestCustomCorrectVersion()
        {
            //  Options.
            var options = new XmlDatabaseOptions()
            {
                CurrentVersion = new XMLDatabaseVersion()
                {
                    Major = 3,
                    Minor = 3,
                    Release = 1,
                    Revision = 0
                },
                MinimalVersion = new XMLDatabaseVersion()
                {
                    Major = 1,
                    Minor = 5,
                    Release = 0,
                    Revision = 2
                }
            };

            //  Major test.
            ChangeTestFileVersion(3, 3, 0, 0);
            TestVersionNoException(options);

            //  Minor test.
            ChangeTestFileVersion(2, 2, 0, 0);
            TestVersionNoException(options);

            //  Release test.
            ChangeTestFileVersion(3, 2, 2, 0);
            TestVersionNoException(options);

            //  Revision test.
            ChangeTestFileVersion(3, 3, 0, 1);
            TestVersionNoException(options);
        }

        [Test]
        public void TestCustomLowerVersion()
        {
            //  Options.
            var options = new XmlDatabaseOptions()
            {
                CurrentVersion = new XMLDatabaseVersion()
                {
                    Major = 3,
                    Minor = 0,
                    Release = 0,
                    Revision = 0
                },
                MinimalVersion = new XMLDatabaseVersion()
                {
                    Major = 1,
                    Minor = 2,
                    Release = 3,
                    Revision = 4
                }
            };

            //  Major test.
            ChangeTestFileVersion(0, 9, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_OLDER, options);

            //  Minor test.
            ChangeTestFileVersion(1, 1, 0, 0);
            TestVersionException(DatabaseVersionError.VERSION_OLDER, options);

            //  Release test.
            ChangeTestFileVersion(1, 2, 2, 0);
            TestVersionException(DatabaseVersionError.VERSION_OLDER, options);

            //  Revision test.
            ChangeTestFileVersion(1, 2, 3, 3);
            TestVersionException(DatabaseVersionError.VERSION_OLDER, options);
        }

        #endregion TEST METHODS

        #region TEST VERSION EXCEPTION

        private void TestVersionNoException(XmlDatabaseOptions options = null)
        {
            try
            {
                var database = new XmlDatabase(_testFilePath, options);
            }
            catch (IncorrectXmlVersionException)
            {
                Assert.Fail();
            }
        }

        private void TestVersionException(DatabaseVersionError versionError, XmlDatabaseOptions options = null)
        {
            try
            {
                var database = new XmlDatabase(_testFilePath, options);
            }
            catch (IncorrectXmlVersionException exc)
            {
                Assert.AreEqual(exc.VersionError, versionError);
                return;
            }

            Assert.Fail();
        }

        #endregion TEST VERSION EXCEPTION

        #region CHANGE FILE VERSION METHODS

        private void LoadTestFileInsides()
        {
            if (File.Exists(_testFilePath))
                _testFileInside = File.ReadAllText(_testFilePath);
            else
                SetupTestFileInsides();
        }

        private void SetupTestFileInsides()
        {
            _testFileInside = @"<?xml version=""1.0\"" encoding=""utf - 8""?>
<XMLDatabase>
	<XMLDatabaseVersion Major=""{major}"" Minor=""{minor}"" Release=""{release}"" Revision=""{revision}"" />
</XMLDatabase>";
        }

        private void ChangeTestFileVersion(int major, int minor, int release, int revision)
        {
            var fileContent = _testFileInside
                .Replace("{major}", $"{major}")
                .Replace("{minor}", $"{minor}")
                .Replace("{release}", $"{release}")
                .Replace("{revision}", $"{revision}");

            File.WriteAllText(_testFilePath, fileContent);
        }

        private void RevertTestFileInsides()
        {
            File.WriteAllText(_testFilePath, _testFileInside);
        }

        #endregion CHANGE FILE VERSION METHODS

    }
}
