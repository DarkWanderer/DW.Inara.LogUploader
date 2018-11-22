﻿namespace DW.ELA.UnitTests
{
    using System.IO;
    using DW.ELA.Controller;
    using DW.ELA.UnitTests.Utility;
    using NUnit.Framework;

    public partial class LogEnumeratorTests
    {
        [Test]
        public void ShouldPlayEvents()
        {
            var directoryProvider = new TestDirectoryProvider();
            Directory.Delete(directoryProvider.Directory, true);

            string testFile1 = Path.Combine(directoryProvider.Directory, "Journal.1234.log");
            string testFile2 = Path.Combine(directoryProvider.Directory, "JournalBeta.2345.log");

            File.WriteAllText(testFile1, "asd");
            File.WriteAllText(testFile2, "asd");

            var files = LogEnumerator.GetLogFiles(directoryProvider.Directory);
            CollectionAssert.Contains(files, testFile1);
            CollectionAssert.DoesNotContain(files, testFile2);
        }
    }
}
