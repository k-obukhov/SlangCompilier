using NUnit.Framework;
using System;
using System.IO;

namespace AntlrSyntaxTests
{
    public class Tests
    {
        private string[] allFiles;
        [SetUp]
        public void Setup()
        {
            allFiles = Directory.GetFiles(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Files"), "*.sl");
        }

        private void TestFiles()
        {
            for (int i = 0; i < allFiles.Length; ++i)
            {
                Console.WriteLine($"Test #{i + 1}");
                GrammarTester.Test(allFiles[i]);
            }
        }

        [Test]
        public void TestFilesFound()
        {
            foreach (var file in allFiles)
            {
                TestContext.Out.WriteLine(file);
            }
            Assert.IsTrue(allFiles.Length != 0);
        }

        [Test]
        public void Test1()
        {
            Assert.DoesNotThrow(TestFiles);
        }
    }
}