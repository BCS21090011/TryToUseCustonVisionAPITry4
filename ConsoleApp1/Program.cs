// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using System;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ConsoleApp1.Models;

var predictionKey = "";
var predictionUrl = "";
HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
var file = File.OpenRead("img.jpg");
byte[] imgBytes = FileReader.ReadFully(file);
using (var content = new ByteArrayContent(imgBytes))
{
    // request API 
    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
    try
    {
        var res = await client.PostAsync(predictionUrl, content);
        var str = await res.Content.ReadAsStringAsync();
        var myPredictionModel = JsonConvert.DeserializeObject<MyPredictionModel>(str);
        var predictions = myPredictionModel.predictions;
        foreach (var prediction in predictions)
        {
            if (prediction.probability > 0.7)
            {
                //100px by 100px
                //prediction.boundingBox 0.75
            }
        }
    }
    catch (Exception e)
    {
       
    }
    
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
