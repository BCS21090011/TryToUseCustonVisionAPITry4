namespace Program
{

    using System.Drawing;
    using ConsoleApp1.Models;
    using LemmeDoIt;
    using APIing;

    public class MainProgram
    {

        public static void Main(string[] args)
        {

            float probToPass = 0.9f;
            string inputFolder = "../../../Inputs";
            string outputFolder = "../../../Outputs";
            string[] files = Directory.GetFiles(inputFolder, "*.jpg");

            LemmeProcess pred = new LemmeProcess();

            int index = 0;
            foreach (string file in files)
            {
                index++;
            }

            Console.WriteLine("Press [Enter] to exit:");
            Console.ReadLine();

        }

    }

}