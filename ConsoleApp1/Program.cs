namespace Program
{

    using LemmeDoIt;

    public class MainProgram
    {

        public static void Main(string[] args)
        {

            float probToPass = 0.95f;
            string inputFolder = "../../../Inputs";
            string outputFolder = "../../../Outputs";
            string[] files = Directory.GetFiles(inputFolder, "*.jpg");

            LemmeProcess pred = new LemmeProcess();

            int index = 0;
            foreach (string file in files)
            {
                pred.GimmeImg(imgFileName: file, passProbability: probToPass, saveFileName: Convert.ToString(index), saveFileFolder: outputFolder);
                index++;
                Thread.Sleep(1000);
            }

            Console.WriteLine("Press [Enter] to exit:");
            Console.ReadLine();

        }

    }

}