namespace Program
{

    using System;
    using System.Drawing;
    using ConsoleApp1.Models;
    using LemmeDoIt;
    using APIing;

    public class MainProgram
    {

        public static void Main(string[] args)
        {

            float probToPass = 0.9f;
            string tagWanted = "people";
            string inputFolder = "../../../Inputs";
            string outputFolder = "../../../Outputs";
            string[] files = Directory.GetFiles(inputFolder, "*.jpg");

            LemmeProcess pred = new LemmeProcess();

            pred.GimmeImg(files);
            while (true)
            {
                foreach (List<ImgObj> cropedImgs in pred.imgs)
                {
                    foreach (ImgObj img in cropedImgs)
                    {
                        if (img.predict.probability >= probToPass)
                        {
                            if (img.predict.tagName == tagWanted)
                            {
                                Console.WriteLine($"\n\n\nOutput: {img.saveFileName}");
                                Console.WriteLine($"Probability: {img.predict.probability}");
                                Console.WriteLine($"Width: {img.predict.boundingBox.width}\tHeight: {img.predict.boundingBox.height}");
                                Console.WriteLine($"Left: {img.predict.boundingBox.left}\tTop: {img.predict.boundingBox.top}");
                                pred.SaveImg(img: img, outputFolder: outputFolder);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Press [Enter] to exit:");
            Console.ReadLine();

        }

    }

}