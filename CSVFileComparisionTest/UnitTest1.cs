using NUnit.Framework;

using CompareCSV;
using System.IO;
using System;

namespace CSVFileComparisionTest
{   
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestReadFileSuccessFirstAttempt()
        {
            var stringReader = new StringReader("File1.csv\r\n");
            Console.SetIn(stringReader);
            string[] file =CompareCSV.CompareCSVFiles.ReadCSVFile(1);
            Assert.That(file.Length>0);
        }

        [Test]
        public void TestReadFileSuccessSecondAttempt()
        {
            var stringReader = new StringReader("\r\nFile1.csv\r\n");
            Console.SetIn(stringReader);
            string[] file = CompareCSV.CompareCSVFiles.ReadCSVFile(1);
            Assert.That(file.Length > 0);
        }

        [Test]
        public void TestReadFileSuccessFourthAttempt()
        {
            var stringReader = new StringReader("\r\n\r\n\r\nFile1.csv\r\n");
            Console.SetIn(stringReader);
            Exception exception = new Exception();
            int fileNumber = 1;
            try
            {
                CompareCSV.CompareCSVFiles.ReadCSVFile(fileNumber);
            }
            catch(Exception ex)
            {
                exception = ex;
            }
            Assert.IsTrue((exception.Message).Contains("CSV file " + fileNumber + " path is mandatory"));

        }

        [Test]
        public void TestCompareBiggerFile1()
        {
            var stringReader = new StringReader("File1.csv\r\nFile2.csv\r\n");
            Console.SetIn(stringReader);
            string[] file1 = CompareCSV.CompareCSVFiles.ReadCSVFile(1);
            string[] file2 = CompareCSV.CompareCSVFiles.ReadCSVFile(2);
            CompareCSV.CompareCSVFiles.Compare(file1,file2);

        }

        [Test]
        public void TestCompareBiggerFile2()
        {
            var stringReader = new StringReader("File1.csv\r\nFile2.csv\r\n");
            Console.SetIn(stringReader);
            string[] file1 = CompareCSV.CompareCSVFiles.ReadCSVFile(1);
            string[] file2 = CompareCSV.CompareCSVFiles.ReadCSVFile(2);
            CompareCSV.CompareCSVFiles.Compare(file1, file2);
        }
    }
}