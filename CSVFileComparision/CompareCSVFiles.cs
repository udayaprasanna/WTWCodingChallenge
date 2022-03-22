using System.Collections;
using System.Collections.Generic;

namespace CompareCSV
{
    public class CompareCSVFiles
    {
        static void Main(string[] args)
        {
            //Getting the csv files
            string[] file1 = ReadCSVFile(1);
            string[] file2 = ReadCSVFile(2);

            if (file1.Length > file2.Length)
            {
                Compare(file1, file2);
            }
            else
            {
                Compare(file2, file1,false);
            }
        }

        public static string[] ReadCSVFile(int fileNumber)
        {
            Console.WriteLine("Enter name of csv file " + fileNumber + " that needs to be compared: ");
            string fileName = Console.ReadLine();
            for (int i = 0; i < 3; i++)
            {
                if (string.IsNullOrEmpty(fileName))
                    if (i == 2)
                        throw new Exception("CSV file " + fileNumber + " path is mandatory");
                    else
                    {
                        Console.WriteLine("Enter name of csv file " + fileNumber + " that needs to be compared: ");
                        fileName = Console.ReadLine();

                    }
                else

                    break;
            }
            return System.IO.File.ReadAllLines(".\\" + fileName);

        }

        public static void Compare(string[] file1, string[] file2, bool file1Bigger = true)
        {
            ArrayList difference = new ArrayList();
            string file1Name = file1Bigger ? "File1" : "File2";
            string file2Name = file1Bigger ? "File2" : "File1";
            difference.Add(file1Name + "\t" + file2Name);
            int file2Length = file2.Length;
            for (int i = 0; i < file1.Length; i++)
            {
                string file1Value = file1[i];
                string file2Value = "No corresponding lines";
                bool different = false;
                if (i < file2Length)
                {
                    if (!file1[i].Equals(file2[i]))
                    {
                        different = true;
                        file2Value = file2[i];
                    }
                        
                }
                else
                    different = true;

                if(different)
                {
                    difference.Add(file1Bigger ? file1Value + "\t" + file2Value : file2Value + "\t" + file1Value);
                }

            }
            if(difference.Count==1)
            {
                Console.WriteLine("The files are the same");
            }
            else
            {
                string differenceFileName = "difference_"+ DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";
                Console.WriteLine("The files are different. The difference between File 1 and File 2 can be found in .\\difference\\" + differenceFileName);
                if (!Directory.Exists(".\\difference\\"))
                    Directory.CreateDirectory(".\\difference\\");
                FileStream diffFile = System.IO.File.Create(".\\difference\\" + differenceFileName);
                diffFile.Close();
                System.IO.File.WriteAllLines(".\\difference\\" + differenceFileName, (string[])difference.ToArray(typeof(string)));
            }

        }
    }
}

