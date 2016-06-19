using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace 水印2
{
    public class WaterText
    {
        /// <summary>
        /// 创建水印图片
        /// </summary>
        /// <param name="validateCode">验证码</param>
        /// <returns></returns>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 创建水印图片
        /// </summary>
        /// <param name="validateCode">验证码</param>
        /// <returns></returns>
        public void CreateWaterText(string oldpath, string text, string newpath, int Alpha, int fontsize)
        {
            text = text + "版权所有";
            FileStream fs = new FileStream(oldpath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            MemoryStream ms = new MemoryStream(bytes);

            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(ms);
            int imgPhotoWidth = imgPhoto.Width;
            int imgPhotoHeight = imgPhoto.Height;

            Bitmap bmPhoto = new Bitmap(imgPhotoWidth, imgPhotoHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(72, 72);
            Graphics gbmPhoto = Graphics.FromImage(bmPhoto);
            //gif背景色
            gbmPhoto.Clear(Color.FromName("white"));
            gbmPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            gbmPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gbmPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, imgPhotoWidth, imgPhotoHeight), 0, 0, imgPhotoWidth, imgPhotoHeight, GraphicsUnit.Pixel);
            System.Drawing.Font font = null;
            System.Drawing.SizeF crSize = new SizeF();
            font = new Font("宋体", fontsize, FontStyle.Bold);
            //测量指定区域
            crSize = gbmPhoto.MeasureString(text, font);
            float y = imgPhotoHeight - crSize.Height;
            float x = imgPhotoWidth - crSize.Width;
            System.Drawing.StringFormat StrFormat = new System.Drawing.StringFormat();
            StrFormat.Alignment = System.Drawing.StringAlignment.Center;

            //画两次制造透明效果
            System.Drawing.SolidBrush semiTransBrush2 = new System.Drawing.SolidBrush(Color.FromArgb(Alpha, 56, 56, 56));
            gbmPhoto.DrawString(text, font, semiTransBrush2, x + 1, y + 1);

            System.Drawing.SolidBrush semiTransBrush = new System.Drawing.SolidBrush(Color.FromArgb(Alpha, 176, 176, 176));
            gbmPhoto.DrawString(text, font, semiTransBrush, x, y);
            bmPhoto.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);
            gbmPhoto.Dispose();
            imgPhoto.Dispose();
            bmPhoto.Dispose();
        }
    }
}