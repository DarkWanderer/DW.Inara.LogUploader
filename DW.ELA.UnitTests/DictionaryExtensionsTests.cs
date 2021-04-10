namespace DW.ELA.UnitTests
{
    using System;
    using System.Collections.Generic;
    using DW.ELA.Utility.Extensions;
    using NUnit.Framework;

    public class DictionaryExtensionsTests
    {
        [Test]
        public void AddIfNotNullShouldAdd()
        {
            var dictionary = GetTestDictionary();
            dictionary.AddIfNotNull("D", "4");
            Assert.AreEqual("4", dictionary["D"]);
        }

        [Test]
        public void AddIfNotNullShouldNotAdd()
        {
            var dictionary = GetTestDictionary();
            dictionary.AddIfNotNull("D", null);
            Assert.IsFalse(dictionary.ContainsKey("D"));
        }

        [Test]
        public void AddIfNotNullShouldThrow()
        {
            var dictionary = GetTestDictionary();
            Assert.Throws<ArgumentException>(() => dictionary.AddIfNotNull("C", "4"));
        }

        private IDictionary<string, string> GetTestDictionary() => new Dictionary<string, string> { { "A", "1" }, { "B", null }, { "C", "3" } };
    }
}
