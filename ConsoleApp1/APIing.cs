namespace APIing
{

    using System.Drawing;
    using System.IO;
    using System.Xml.Linq;
    using System;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using ConsoleApp1.Models;

    public class ComunicateAPI
    {

        string predictionKey = "35532dc500874d7a86cf0e18b789bc5a";
        string predictionUrl = "https://yysprediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/f79b57d5-32a9-4566-949d-b144204c7ae0/detect/iterations/Iteration4/image";

        #region These are just some example of Task and async:
        public Task<string> Test()
        {
            return Task.FromResult("test");
        }

        public async Task<string> Test2()
        {
            return "test";
        }
        #endregion

        public Task<HttpResponseMessage> HelloAPI(string imgFileName)
        {

            Console.WriteLine($"HelloAPI({imgFileName})");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
            var file = File.OpenRead(imgFileName);
            byte[] imgBytes = FileReader.ReadFully(file);
            var content = new ByteArrayContent(imgBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return client.PostAsync(predictionUrl, content);

        }

        public async Task<List<Prediction>> GiveUPredictions(HttpResponseMessage res)
        {

            List<Prediction> pred = new List<Prediction>();

            try
            {

                Console.WriteLine("GiveUPredictions()");

                var str = await res.Content.ReadAsStringAsync();
                var myPredictionModel = JsonConvert.DeserializeObject<MyPredictionModel>(str);
                pred = myPredictionModel.predictions;

                #region These are useless for now:
                /*int index = 0;
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

                }*/
                #endregion

            }
            catch (Exception e)
            {

            }

            return pred;

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