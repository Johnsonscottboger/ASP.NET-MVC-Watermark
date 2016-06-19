using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 水印2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            WaterText waterText = new WaterText();
            string text = "我是水印！！！！！！！";
            Session["WaterText"] = text;
            waterText.CreateWaterText(@"D:\OneDrive\C#程序设计\水印2\水印2\App_Data\Paris la Tour Eiffel.jpg","水印！！！", @"D:\WaterText.jpg",255,200);
            return View();
        }

        public FileContentResult GetImage()
        {
            string mimeType = "image/jpeg";

            FileStream fs = new FileStream(@"D:\WaterText.jpg", FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            var fileBytes = br.ReadBytes((int)fs.Length);
            
            return File(fileBytes,mimeType);
        }
    }
}