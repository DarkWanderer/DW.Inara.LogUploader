﻿namespace DW.ELA.UnitTests.Utility
{
    using System;
    using System.IO;
    using DW.ELA.Interfaces;

    public class TestDirectoryProvider : ILogDirectoryNameProvider
    {
        private static readonly Random random = new Random();

        private readonly string seed = GenerateSeed();

        private static string GenerateSeed()
        {
            byte[] seedBytes = new byte[8];
            random.NextBytes(seedBytes);
            return BitConverter.ToString(seedBytes);
        }

        public string Directory
        {
            get
            {
                string dir = Path.Combine(Path.GetTempPath(), "ELA-TEST-" + seed);
                System.IO.Directory.CreateDirectory(dir);
                return dir;
            }
        }
    }
}
