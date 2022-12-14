namespace LemmeDoIt
{

    using System.Drawing;
    using System.IO;
    using System.Xml.Linq;
    using System;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using ConsoleApp1.Models;
    using APIing;

    public class LemmeProcess
    {

        public List<List<ImgObj>> imgs = new List<List<ImgObj>>();

        public async Task<List<ImgObj>> GimmeImg(string imgFile)
        {

            Console.WriteLine($"GimmeImg({imgFile})");

            ComunicateAPI imgAPI = new ComunicateAPI();
            var res = await imgAPI.HelloAPI(imgFileName:imgFile);

            Bitmap oriImg = (Bitmap)Image.FromFile(imgFile);
            List<Prediction> predictions = await imgAPI.GiveUPredictions(res: res);

            List<ImgObj> imgs = new List<ImgObj>();

            for (int i = 0; i < predictions.Count; i++)
            {
                imgs.Add(new ImgObj()
                {
                    img = CutImg(img: oriImg, ruler: GetRect(img: oriImg, predictions[i].boundingBox)),
                    saveFileName = Path.GetFileNameWithoutExtension(imgFile) + "Output" + Convert.ToString(i),
                    predict = predictions[i]
                });
            }

            return imgs;

        }

        public async void GimmeImg(string[] imgFiles)
        {

            foreach(string imgFile in imgFiles)
            {
                List<ImgObj> taggedImgs = await GimmeImg(imgFile: imgFile);
                imgs.Add(taggedImgs);
            }

        }

        public void SaveImg(ImgObj img,string outputFolder)
        {
            img.img.Save(outputFolder + "/" + img.saveFileName);
        }

        public Rectangle GetRect(Bitmap img, BoundingBox bdBox)
        {
            int calcedLeft = Convert.ToInt32(img.Width * bdBox.left);
            int calcedTop = Convert.ToInt32(img.Height * bdBox.top);
            int calcedWidth = Convert.ToInt32(img.Width * bdBox.width);
            int calcedHeight = Convert.ToInt32(img.Height * bdBox.height);

            Rectangle ret = new Rectangle(calcedLeft, calcedTop, calcedWidth, calcedHeight);

            return ret;
        }

        public Bitmap CutImg(Bitmap img, Rectangle ruler)
        {
            Bitmap cuttedImg = img.Clone(ruler, img.PixelFormat);

            return cuttedImg;
        }

    }

    public class ImgObj
    {
        public Bitmap img { get; set; }
        public string saveFileName { get; set; }
        public Prediction predict { get; set; }
    }

}