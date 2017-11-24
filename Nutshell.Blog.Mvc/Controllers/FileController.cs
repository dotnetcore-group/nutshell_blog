using Nutshell.Blog.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            var data = new { code = 1, msg = "", url = "", fileName = "" };
            var date = DateTime.Now;
            string msg = string.Empty;
            var url = SaveFileToLocal("image", $"/upload/photos/original/{date.Year}/{date.Month}/{date.Day}/", 400, 400, out msg);
            if (url != null)
            {
                data = new { code = 0, msg = msg, url = url, fileName = "" };
            }

            return Json(data);
        }

        string SaveFileToLocal(string name, string saveUrl, int thumMaxWidth, int thumMaxHeight, out string msg)
        {
            //文件保存目录路径
            string savePath = Server.MapPath(saveUrl);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1000000;

            HttpPostedFileBase imgFile = Request.Files[name];
            if (imgFile == null)
            {
                msg = "请选择文件！";
                return null;
            }

            string fileName = imgFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                msg = "上传文件大小超过限制！";
                return null;
            }
            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(extTable["image"].ToString().Split(','), fileExt.Substring(1).ToLower()) < 0)
            {
                msg = $"上传文件扩展名是不允许的扩展名！\n只允许{extTable["image"].ToString()}。";
                return null;
            }


            string newFileName = Guid.NewGuid().ToString("N") + fileExt;
            string filePath = savePath + newFileName;

            var image = ImageHelper.GetThumbNailImage(Image.FromStream(imgFile.InputStream, true, true), thumMaxWidth, thumMaxHeight);
            image.Save(filePath);
            //imgFile.SaveAs();

            string fileUrl = Path.Combine(saveUrl, newFileName);
            msg = "上传成功！";
            return fileUrl;
        }
    }
}