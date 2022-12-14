namespace LemmeDoIt
{

    using System.Drawing;
    using System.IO;
    using System.Xml.Linq;
    using System;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using ConsoleApp1.Models;
    using System.Drawing;

    public class LemmeProcess
    {

        string predictionKey = "35532dc500874d7a86cf0e18b789bc5a";
        string predictionUrl = "https://yysprediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/f79b57d5-32a9-4566-949d-b144204c7ae0/detect/iterations/Iteration4/image";

        public async void GimmeImg(string imgFileName, float passProbability, string saveFileName = "",string saveFileFolder = "")
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
            var file = File.OpenRead(imgFileName);
            byte[] imgBytes = FileReader.ReadFully(file);
            var content = new ByteArrayContent(imgBytes);
            Bitmap oriImg = (Bitmap)Image.FromFile(imgFileName);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            try
            {

                var res = await client.PostAsync(predictionUrl, content);
                var str = await res.Content.ReadAsStringAsync();
                var myPredictionModel = JsonConvert.DeserializeObject<MyPredictionModel>(str);
                var predictions = myPredictionModel.predictions;

                int index = 0;
                if (predictions != null)
                {
                    foreach (Prediction prediction in predictions)
                    {
                        if (prediction.probability >= passProbability)
                        {
                            if (prediction.tagName == "people")
                            {

                                string cleanFileName = Path.GetFileNameWithoutExtension(imgFileName);
                                string outputFileName = saveFileFolder+"/" + cleanFileName + "Output" + index + ".jpg";

                                Console.WriteLine($"\nFile: {cleanFileName}");
                                Console.WriteLine($"Prediction number: {index}");
                                Console.WriteLine($"Save as: {outputFileName}");
                                Console.WriteLine($"{prediction.tagName}:{prediction.probability}");
                                Console.WriteLine("Bounding box:");
                                Console.WriteLine($"Left: {prediction.boundingBox.left}\tTop: {prediction.boundingBox.top}");
                                Console.WriteLine($"Width: {prediction.boundingBox.width}\tHeight: {prediction.boundingBox.height}");

                                Rectangle rect = GetRect(oriImg, prediction.boundingBox);
                                Bitmap peopleImg = CutImg(oriImg, rect);
                                peopleImg.Save(outputFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                index++;

                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {

            }

        }

        Rectangle GetRect(Bitmap img, BoundingBox bdBox)
        {
            int calcedLeft = Convert.ToInt32(img.Width * bdBox.left);
            int calcedTop = Convert.ToInt32(img.Height * bdBox.top);
            int calcedWidth = Convert.ToInt32(img.Width * bdBox.width);
            int calcedHeight = Convert.ToInt32(img.Height * bdBox.height);

            Rectangle ret = new Rectangle(calcedLeft, calcedTop, calcedWidth, calcedHeight);

            return ret;
        }

        Bitmap CutImg(Bitmap img, Rectangle ruler)
        {
            Bitmap cuttedImg = img.Clone(ruler, img.PixelFormat);

            return cuttedImg;
        }

        public class FileReader
        {
            public static byte[] ReadFully(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }

    }

}