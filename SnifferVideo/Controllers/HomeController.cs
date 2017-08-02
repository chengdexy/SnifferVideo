using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace SnifferVideo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: GetPreview
        public ActionResult GetPreview()
        {
            string url = Request.QueryString["txtUrl"];
            string result = string.Empty;

            Uri uri = new Uri(url);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sReader = new StreamReader(stream, Encoding.UTF8);
            result = sReader.ReadToEnd();

            //(?<="url":").*?(?=")
            //(?<="vid=").*?(?=?)

            result = Regex.Match(result, "(?<=vid=).*?(?=&)").Value;

            //至此已经得到视频vid
            string vid = result;

            Process pro = new Process();
            pro.StartInfo.RedirectStandardOutput = true;
            //pro.StartInfo.CreateNoWindow = true;
            pro.StartInfo.UseShellExecute = false;

            pro.StartInfo.FileName = Server.MapPath("/ExeTool/sniffertool.exe");// @"D:\我的文档\visual studio 2017\Projects\SnifferTool\SnifferTool\bin\Debug\SnifferTool.exe";
            pro.StartInfo.Arguments = vid;
            pro.Start();
            result = pro.StandardOutput.ReadToEnd();
            pro.WaitForExit();
            pro.Close();


            //至此已得到视频文件真是地址(伪,有效时间由vkey过期时间决定)

            ViewBag.Message = result;
            return View();
        }
    }
}